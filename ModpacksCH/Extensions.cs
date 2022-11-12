using DynmapImageExport.Options;
using ModpacksCH.API;
using ModpacksCH.API.Model;
using System.CommandLine.Builder;
using System.Text.RegularExpressions;
using Version = ModpacksCH.API.Model.Version;

namespace ModpacksCH
{
    internal static class Extensions
    {
        internal static string RegexReplace(this string str, string pattern, string replacement) => Regex.Replace(str, pattern, replacement);

        internal static string RemoveInvalidChars(this string str) => string.Join("_", str.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));

        internal static string ToMarkup(this ModpackManifest M) => $"[white]ID: [/][yellow]{M.ID}[/][white] - {M.Name} ({M.Versions.OrderBy(V => V.ID).Last().Name})[/]";

        internal static string ToMarkup(this Version V) => $"ID: [yellow]{V.ID}[/] - [white]{V.Name} ({V.Type.ToTitleCase()})[/]";

        internal static CommandLineBuilder UseTrace(this CommandLineBuilder builder) => TraceOption.AddToBuilder(builder);
    }
}