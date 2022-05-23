using ModpacksCH.API.Model;
using Version = ModpacksCH.API.Model.Version;

namespace ModpacksCH
{
    internal static class Extensions
    {
        public static string ToMarkup(this ModpackManifest M) => $"[white]ID: [/][yellow]{M.ID}[/][white] - {M.Name} ({M.Versions.OrderBy(V => V.ID).Last().Name})[/]";

        public static string ToMarkup(this Version V) => $"[white]ID: [/][yellow]{V.ID}[/][white] - {V.Name}[/]";
    }
}