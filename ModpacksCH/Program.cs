using DynmapImageExport;
using ModpacksCH;
using ModpacksCH.Commands;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var RootCommand = new RootCommand("Modpacks.ch Downloader") {
    new SearchCommand(),
    new InfoCommand(),
    new DownloadCommand(),
    new AboutCommand()
};

var Parser = new CommandLineBuilder(RootCommand)
    .UseTrace()
    .UseParseErrorReporting()
    .UseExceptionHandler(ExceptionHandler.Handle)
    .UseHelp()
    .Build();

if (args.Length > 0)
{
    return Parser.Invoke(args);
}
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
    }
}