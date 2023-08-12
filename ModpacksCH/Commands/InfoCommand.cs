using ModpacksCH.API;
using ModpacksCH.Options;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Diagnostics;

namespace ModpacksCH.Commands
{
    internal class InfoCommand : Command
    {
        public InfoCommand() : base("info", "Show info about modpack")
        {
            AddAlias("i");
            AddArgument(new Argument<int>("modpackID", "Modpack ID"));
            AddOption(new LimitOption());

            Handler = CommandHandler.Create(HandleCommand);
        }

        private async Task<int> HandleCommand(int modpackID, int? limit)
        {
            Trace.WriteLine($"Info: {modpackID}");
            var Limit = limit ?? 10;
            var Modpack = await AnsiConsole.Status()
                .StartAsync("Fetching modpack info...", async ctx =>
                {
                    using var CH = new CHClient();
                    var Modpack = await CH.GetModpack(modpackID);
                    return Modpack;
                });

            if (Modpack.Status != "success" || Modpack.Name is null)
            {
                AnsiConsole.MarkupLine("[red]Modpack not found[/]");
                return 0;
            }

            var Versions = Modpack.Versions.OrderByDescending(V => V.ID).Take(Limit);
            AnsiConsole.MarkupLine($"Modpack: [white]{Modpack.Name}[/] (ID: [yellow]{modpackID}[/])");
            Panel P = new(Modpack.Synopsis) { Header = new("[yellow]Synopsis[/]", Justify.Center) };
            AnsiConsole.Write(P);
            AnsiConsole.MarkupLine($"Latest version: {Versions.First().ToMarkup()}");

            var Root = new Tree("Other versions");
            foreach (var version in Versions.Skip(1))
            {
                Root.AddNode(version.ToMarkup());
            }
            AnsiConsole.Write(Root);

            return 0;
        }
    }
}