using ModpacksCH.API.Model;
using System.Globalization;
using System.Text;
using Version = ModpacksCH.API.Model.Version;

namespace ModpacksCH.API
{
    public static class Extensions
    {
        private static readonly TextInfo TI = CultureInfo.CurrentCulture.TextInfo;

        public static string Decode(this string base64) => Encoding.UTF8.GetString(Convert.FromBase64String(base64));

        public static Target Game(this Version version) => version.Targets.FirstOrDefault(T => T.Type == "game");

        public static Version LatestVersion(this ModpackManifest modpack) => modpack.Versions.OrderByDescending(V => V.ID).FirstOrDefault();

        public static string MinecraftVersion(this Version version) => version.Targets.FirstOrDefault(T => T.Name == "minecraft")?.Version;

        public static Target ModLoader(this Version version) => version.Targets.FirstOrDefault(T => T.Type == "modloader");

        public static Target Runtime(this Version version) => version.Targets.FirstOrDefault(T => T.Type == "runtime");

        public static string ToTitleCase(this string text) => TI.ToTitleCase(text);
    }
}