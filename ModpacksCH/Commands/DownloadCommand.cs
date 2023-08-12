using ModpacksCH.API;
using ModpacksCH.API.Model;
using ModpacksCH.Models;
using ModpacksCH.Options;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Diagnostics;
using System.Text;

namespace ModpacksCH.Commands
{
    internal class DownloadCommand : Command
    {
        private readonly Stopwatch SW = new();

        public DownloadCommand() : base("download", "Download modpack")
        {
            AddAlias("d");
            AddArgument(new Argument<int>("modpackID", "Modpack ID"));
            AddArgument(new Argument<int?>("versionID", "Version ID"));
            AddOption(new ServerOption());
            AddOption(new DirectoryOption());

            Handler = CommandHandler.Create(HandleCommand);
        }

        private async Task<int> HandleCommand(int modpackID, int? versionID, bool server, DirectoryInfo path)
        {
            Trace.WriteLine($"Download: {modpackID}-{versionID}-{server}-{path}");
            var (modpack, version) = await AnsiConsole.Status()
                .StartAsync("Fetching modpack info...", async ctx =>
                {
                    using var CH = new CHClient();
                    var M = await CH.GetModpack(modpackID);
                    versionID ??= M.LatestVersion().ID;
                    var V = await CH.GetVersion(modpackID, versionID.Value);
                    return (M, V);
                });
            AnsiConsole.MarkupLine($"Modpack: [white]{modpack.Name}[/] (Version: [yellow]{version.Name})[/]");
            SW.Start();
            var info = new DownloadInfo(modpack, version, server);
            var progress = AnsiConsole.Progress();
            progress.Columns(new ProgressColumn[] { new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new SpinnerColumn() });
            var (modpackPath, errors) = await progress.StartAsync(async ctx =>
                {
                    var T = ctx.AddTask($"Downloading files 0/{info.Files.Count}", true, info.Files.Count);
                    var P = new Progress<int>(I =>
                    {
                        T.Value = I;
                        T.Description = T.Description.RegexReplace(@"\d+/\d+", $"{T.Value}/{T.MaxValue}");
                    });
                    using ModpackDownloader MD = ModpackDownloader.Create(info);
                    var result = await MD.Download(path.FullName, P);
                    T.StopTask();
                    return result;
                });
            SW.Stop();

            var downloadInfo = new Grid();
            downloadInfo.AddColumns(2);
            downloadInfo.AddRow("[white]Download done:[/]", $@"[yellow]{SW.Elapsed:hh\:mm\:ss}[/]");
            downloadInfo.AddRow(new Markup("[white]Modpack path:[/]"), new TextPath(modpackPath).LeafColor(Color.Yellow));
            AnsiConsole.Write(downloadInfo);

            var game = version.Game();
            var runtime = version.Runtime();
            var modLoader = version.ModLoader();

            var specification = new Grid()
                .AddColumns(2)
                .AddRow($"{game.Name.ToTitleCase()} version:", $"{game.Version}")
                .AddRow($"{runtime.Name.ToTitleCase()} version:", $"{runtime.Version}")
                .AddRow($"{modLoader.Name.ToTitleCase()} version:", $"{modLoader.Version}");
            if (version is CHVersion CH)
            {
                specification.AddRow("Minimum memory:", $"{CH.Specs.Minimum}");
                specification.AddRow("Recommended memory:", $"{CH.Specs.Recommended}");
            }
            AnsiConsole.Write(new Panel(specification) { Header = new PanelHeader("[yellow]Specification[/]", Justify.Center) });
            if (errors.Count > 0)
            {
                AnsiConsole.MarkupLine("[red]Failed to download some files. More details in error.txt[/]");
                var error = string.Join("\n\n", errors);
                var file = Path.Combine(modpackPath, "error.txt");
                File.WriteAllText(file, error);
            }

            var note = new StringBuilder();
            note.AppendLine($"{game.Name.ToTitleCase()} version: {game.Version}");
            note.AppendLine($"{runtime.Name.ToTitleCase()} version: {runtime.Version}");
            note.AppendLine($"{modLoader.Name.ToTitleCase()} version: {modLoader.Version}");
            File.WriteAllText(Path.Combine(modpackPath, "note.txt"), note.ToString());

            return 0;
        }
    }
}