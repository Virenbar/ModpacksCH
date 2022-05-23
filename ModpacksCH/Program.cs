using ModpacksCH.Commands;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

var RootCommand = new RootCommand("modpacks.ch modpack downloader") {
    new SearchCommand(),
    new InfoCommand(),
    new DownloadCommand()
};
var Parser = new CommandLineBuilder(RootCommand)
    .UseParseErrorReporting()
    .UseExceptionHandler()
    .UseHelp()
    .Build();
//CBL.Invoke(args);
#if DEBUG
Parser.Invoke("s \"FTB Direwolf20\"");
//Parser.Invoke("s gjljh;dfjh");
//Parser.Invoke("i 95");
//Parser.Invoke("d 95 --path \"C:\\000\"");
//Parser.Invoke("i 281999");
//Parser.Invoke("d 95");
#else
Parser.Invoke(args);
#endif