using ModpacksCH.API.Model;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

namespace ModpacksCH.Commands
{
    internal class SearchCommand : Command
    {
        public SearchCommand() : base("search", "Search modpacks by name")
        {
            AddAlias("s");
            AddArgument(new Argument<string>("name", "Modpack Name"));

            Handler = CommandHandler.Create<string>(HandleCommand);
        }

        private async Task<int> HandleCommand(string name)
        {
            (var Modpacks, var CFModpacks) = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Searching modpacks...", async ctx =>
                {
                    using var CH = new CHClient();
                    var Result = await CH.Search(name);
                    var Modpacks = await Task.WhenAll(Result.Packs.Select(ID => CH.GetCHModpack(ID)));
                    var CFModpacks = await Task.WhenAll(Result.CurseForge.Select(ID => CH.GetCFModpack(ID)));
                    ctx.Status("Search complete");
                    return (Modpacks, CFModpacks);
                });

            if (Modpacks.Length + CFModpacks.Length == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No modpacks found[/]");
                return 0;
            }

            if (Modpacks.Length > 0)
            {
                var T = new Table { Title = new TableTitle("[white]FTB Modpacks[/]") };
                T.AddColumns("ID", "Name", "Version", "MC Version");
                foreach (var M in Modpacks)
                {
                    T.AddRow($"[yellow]{M.ID}[/]", M.Name, $"{M.LatestVersion().Name}", M.LatestVersion().MinecraftVersion() ?? "Empty");
                }
                AnsiConsole.Write(T);
            }
            if (CFModpacks.Length > 0)
            {
                var T = new Table { Title = new TableTitle("[white]CurseForge Modpacks[/]") };
                T.AddColumns("ID", "Name", "Version");
                foreach (var M in CFModpacks)
                {
                    T.AddRow(new Markup($"[yellow]{M.ID}[/]"), new Text(M.Name), new Text(M.LatestVersion().Name));
                }
                AnsiConsole.Write(T);
            }

            return 0;
        }
    }
}