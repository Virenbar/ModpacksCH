using ModpacksCH.API;
using ModpacksCH.Models;
using ModpacksCH.Options;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Diagnostics;

namespace ModpacksCH.Commands
{
    internal class DownloadCommand : Command
    {
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

            var ModpackName = $"{Modpack.Name} - {Version.Name}{(server ? "(server)" : "")}";
            var ModpackPath = Path.Combine(path.FullName, ModpackName);
            var Info = new DownloadInfo(Modpack, Version, ModpackPath, server);

            var SW = new Stopwatch();
            SW.Start();
            await AnsiConsole.Progress()
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new SpinnerColumn()
                })
                .StartAsync(async ctx =>
                {
                    var T = ctx.AddTask($"Downloading files 0/{Info.Files.Count}", true, Info.Files.Count);
                    var P = new Progress<int>(I =>
                    {
                        T.Value = I;
                        T.Description = T.Description.RegexReplace(@"\d+/\d+", $"{T.Value}/{T.MaxValue}");
                    });
                    using var MD = new ModpackDownloader(Info);
                    await MD.Download(P);
                    T.StopTask();
                });
            SW.Stop();

            var Out = new Grid()
                .AddColumns(2)
                .AddRow("[white]Download done:[/]", $@"[yellow]{SW.Elapsed:hh\:mm\:ss}[/]")
                .AddRow(new Markup("[white]Modpack path:[/]"), new TextPath(ModpackPath).LeafColor(Color.Yellow));
            AnsiConsole.Write(Out);

            var Specification = new Grid()
                .AddColumns(2)
                .AddRow("Java version:", $"{Version.JavaVersion()}")
                .AddRow("Forge version:", $"{Version.ForgeVersion()}")
                .AddRow("Minimum memory:", $"{Version.Specs.Minimum}")
                .AddRow("Recommended memory:", $"{Version.Specs.Recommended}");
            AnsiConsole.Write(new Panel(Specification) { Header = new PanelHeader("[yellow]Specification[/]", Justify.Center) });

            var Note = $"Java version: {Version.JavaVersion()}\nForge version: {Version.ForgeVersion()}\nMinimum memory: {Version.Specs.Minimum}\nRecommended memory: {Version.Specs.Recommended}";
            File.WriteAllText(Path.Combine(ModpackPath, "note.txt"), Note);

            return 0;
        }
    }
}