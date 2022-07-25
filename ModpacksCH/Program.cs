using ModpacksCH.Commands;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

var RootCommand = new RootCommand("Modpacks.ch Downloader") {
    new SearchCommand(),
    new InfoCommand(),
    new DownloadCommand()
};
var Parser = new CommandLineBuilder(RootCommand)
    .UseParseErrorReporting()
    .UseExceptionHandler()
    .UseHelp()
    .Build();

if (args.Length > 0) { Parser.Invoke(args); }
else
{
    Parser.Invoke("-h");
    while (true)
    {
        AnsiConsole.MarkupLine("[green]Input command:[/]");
        var Input = AnsiConsole.Prompt(new TextPrompt<string>("[green]>[/]"));
        Input = Input.Replace("ModpacksCH", "");
        AnsiConsole.WriteLine();
        Parser.Invoke(Input);
        AnsiConsole.WriteLine();
    }
}