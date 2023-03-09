using DynmapImageExport;
using ModpacksCH;
using ModpacksCH.Commands;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Text;

internal class Program
{
    private static readonly Parser Parser;

    static Program()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var RootCommand = new RootCommand("Modpacks.ch Downloader") {
            new SearchCommand(),
            new InfoCommand(),
            new DownloadCommand(),
            new AboutCommand()
        };

        Parser = new CommandLineBuilder(RootCommand)
            .UseTrace()
            .UseParseErrorReporting()
            .UseExceptionHandler(ExceptionHandler.Handle)
            .UseHelp()
            .Build();
    }

    internal static int Invoke(string[] args) => Invoke(string.Join(' ', args));

    internal static int Invoke(string args)
    {
        args = args.Replace("ModpackCH.exe", "", StringComparison.InvariantCultureIgnoreCase);
        args = args.Replace("ModpackCH", "", StringComparison.InvariantCultureIgnoreCase);
        return Parser.Invoke(args);
    }

    private static int Main(string[] args)
    {
#if DEBUG
        Debug.Invoke();
#endif
        if (args.Length > 0)
        {
            return Invoke(args);
        }
        else
        {
            Invoke("-h");
            while (true)
            {
                AnsiConsole.MarkupLine("[green]Input command:[/]");
                var Input = AnsiConsole.Prompt(new TextPrompt<string>("[green]>[/]"));
                AnsiConsole.WriteLine();
                Invoke(Input);
            }
        }
    }
}