using ModpacksCH.API;
using ModpacksCH.API.Model;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

namespace ModpacksCH.Commands
{
    internal class DownloadCommand : Command
    {
        public DownloadCommand() : base("download", "Download modpack")
        {
            AddAlias("d");

            AddArgument(new Argument<int>("modpackID", "Modpack ID"));
            AddArgument(new Argument<int?>("versionID", "Version ID"));
            AddOption(new Option<bool>(new[] { "--server", "-s" }, "Download server version") { Arity = ArgumentArity.Zero });
            var PathOption = new Option<DirectoryInfo>(new[] { "--path", "-p" }, () => new DirectoryInfo(Directory.GetCurrentDirectory()), "Directory to save modpack")
            {
                Arity = ArgumentArity.ExactlyOne
            }.ExistingOnly();
            AddOption(PathOption);

            Handler = CommandHandler.Create(HandleCommand);
        }

        private async Task<int> HandleCommand(int modpackID, int? versionID, bool server, DirectoryInfo path)
        {
            (var Modpack, var Version) = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Getting modpack info...", async ctx =>
                {
                    using var CH = new CHClient();
                    var M = await CH.GetModpack(modpackID);
                    versionID ??= M.LatestVersion().ID;
                    var V = await CH.GetVersion(modpackID, versionID.Value);
                    return (M, V);
                });
            AnsiConsole.MarkupLine($"Modpack: {Modpack.Name} (Version: {Version.Name})");

            var ModpackName = $"{Modpack.Name} - {Version.Name}{(server ? "(server)" : "")}";
            var ModpackPath = Path.Combine(path.FullName, ModpackName);
            var Info = new DownloadInfo(Modpack, Version, ModpackPath, server);

            await AnsiConsole.Progress()
                .Columns(new ProgressColumn[] { new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new SpinnerColumn() })
                .StartAsync(async ctx =>
                {
                    var FilesTask = ctx.AddTask("Downloading files");
                    var P = new Progress<(int Count, int Max)>(I =>
                    {
                        FilesTask.MaxValue = I.Max;
                        FilesTask.Value = I.Count;
                    });
                    FilesTask.StartTask();
                    using var MD = new ModpackDownloader();
                    await MD.Download(Info, P);
                    FilesTask.StopTask();
                });
            AnsiConsole.MarkupLine("[yellow]Download complete.[/]");

            var Note = $"""
                Java version: {Version.JavaVersion()}
                Forge version: {Version.ForgeVersion()}
                Minimum memory: {Version.Specs.Minimum}
                Recommended memory: {Version.Specs.Recommended}
                """;
            AnsiConsole.WriteLine("Modpack specs:");
            AnsiConsole.WriteLine(Note);
            File.WriteAllText(Path.Combine(ModpackPath, "note.txt"), Note);

            return 0;
        }
    }
}