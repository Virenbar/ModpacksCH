using ModpacksCH.API;
using ModpacksCH.Options;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Diagnostics;

namespace ModpacksCH.Commands
{
    internal class SearchCommand : Command
    {
        public SearchCommand() : base("search", "Search modpacks by name")
        {
            AddAlias("s");
            AddArgument(new Argument<string>("name", "Modpack Name"));
            AddOption(new LimitOption());

            Handler = CommandHandler.Create(HandleCommand);
        }

        private async Task<int> HandleCommand(string name, int? limit)
        {
            Trace.WriteLine($"Search: {name}");
            var Limit = limit ?? 10;
            (var CHModpacks, var CFModpacks) = await AnsiConsole.Status()
                .StartAsync("Searching modpacks...", async ctx =>
                {
                    using var CH = new CHClient();
                    var Result = await CH.Search(name, Limit);
                    var CHModpacks = await Task.WhenAll(Result.Packs.Select(ID => CH.GetCHModpack(ID)));
                    var CFModpacks = await Task.WhenAll(Result.CurseForge.Select(ID => CH.GetCFModpack(ID)));
                    ctx.Status("Search complete");
                    return (CHModpacks, CFModpacks);
                });
            Trace.WriteLine($"Search done: {CHModpacks.Length} {CFModpacks.Length}");
            if (CHModpacks.Length + CFModpacks.Length == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No modpacks found[/]");
                return 0;
            }

            if (CHModpacks.Length > 0)
            {
                var T = new Table { Title = new TableTitle("[white]FTB Modpacks[/]") };
                T.AddColumns("ID", "Name", "Version", "MC Version");
                foreach (var M in CHModpacks)
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