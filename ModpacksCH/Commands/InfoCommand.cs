using Spectre.Console;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

namespace ModpacksCH.Commands
{
    internal class InfoCommand : Command
    {
        public InfoCommand() : base("info", "Show info about modpack")
        {
            AddAlias("i");
            AddArgument(new Argument<int>("modpackID", "Modpack ID"));

            Handler = CommandHandler.Create(HandleCommand);
        }

        private async Task<int> HandleCommand(int modpackID)
        {
            var Modpack = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Getting modpack info...", async ctx =>
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

            var Versions = Modpack.Versions.OrderByDescending(V => V.ID);
            AnsiConsole.MarkupLine($"Modpack: {Modpack.Name} (ID: [yellow]{modpackID}[/])");
            AnsiConsole.WriteLine($"Synopsis:");
            AnsiConsole.WriteLine(Modpack.Synopsis);
            AnsiConsole.WriteLine($"Latest version:");
            AnsiConsole.MarkupLine(Versions.First().ToMarkup());

            var Root = new Tree("Other versions");
            foreach (var V in Versions.Skip(1)) { Root.AddNode(V.ToMarkup()); }
            AnsiConsole.Write(Root);

            return 0;
        }
    }
}