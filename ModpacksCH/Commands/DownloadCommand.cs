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
            (var Modpack, var Version) = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Fetching modpack info...", async ctx =>
                {
                    using var CH = new CHClient();
                    var M = await CH.GetModpack(modpackID);
                    versionID ??= M.LatestVersion().ID;
                    var V = await CH.GetVersion(modpackID, versionID.Value);
                    return (M, V);
                });
            AnsiConsole.MarkupLine($"Modpack: [white]{Modpack.Name}[/] (Version: [yellow]{Version.Name})[/]");
            var Info = new DownloadInfo(Modpack, Version, server);
            var Progress = AnsiConsole.Progress();
            Progress.Columns(new ProgressColumn[] { new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new SpinnerColumn() });

            SW.Start();
            var ModpackPath = await Progress.StartAsync(async ctx =>
                {
                    var T = ctx.AddTask($"Downloading files 0/{Info.Files.Count}", true, Info.Files.Count);
                    var P = new Progress<int>(I =>
                    {
                        T.Value = I;
                        T.Description = T.Description.RegexReplace(@"\d+/\d+", $"{T.Value}/{T.MaxValue}");
                    });
                    using ModpackDownloader MD = ModpackDownloader.Create(Info);
                    var Path = await MD.Download(path.FullName, P);
                    T.StopTask();
                    return Path;
                });
            SW.Stop();

            var Out = new Grid();
            Out.AddColumns(2);
            Out.AddRow("[white]Download done:[/]", $@"[yellow]{SW.Elapsed:hh\:mm\:ss}[/]");
            Out.AddRow(new Markup("[white]Modpack path:[/]"), new TextPath(ModpackPath).LeafColor(Color.Yellow));
            AnsiConsole.Write(Out);

            var Game = Version.Game();
            var Runtime = Version.Runtime();
            var ModLoader = Version.ModLoader();

            var Specification = new Grid()
                .AddColumns(2)
                .AddRow($"{Game.Name.ToTitleCase()} version:", $"{Game.Version}")
                .AddRow($"{Runtime.Name.ToTitleCase()} version:", $"{Runtime.Version}")
                .AddRow($"{ModLoader.Name.ToTitleCase()} version:", $"{ModLoader.Version}");
            if (Version is CHVersion CH)
            {
                Specification.AddRow("Minimum memory:", $"{CH.Specs.Minimum}");
                Specification.AddRow("Recommended memory:", $"{CH.Specs.Recommended}");
            }
            AnsiConsole.Write(new Panel(Specification) { Header = new PanelHeader("[yellow]Specification[/]", Justify.Center) });

            var Note = new StringBuilder();
            Note.AppendLine($"{Game.Name.ToTitleCase()} version: {Game.Version}");
            Note.AppendLine($"{Runtime.Name.ToTitleCase()} version: {Runtime.Version}");
            Note.AppendLine($"{ModLoader.Name.ToTitleCase()} version: {ModLoader.Version}");
            File.WriteAllText(Path.Combine(ModpackPath, "note.txt"), Note.ToString());

            return 0;
        }
    }
}