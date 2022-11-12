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

        public static string ForgeVersion(this Version version) => version.Targets.FirstOrDefault(T => T.Name == "forge")?.Version;

        public static string JavaVersion(this Version version) => version.Targets.FirstOrDefault(T => T.Name == "java")?.Version;

        public static Version LatestVersion(this ModpackManifest modpack) => modpack.Versions.OrderByDescending(V => V.ID).FirstOrDefault();

        public static string MinecraftVersion(this Version version) => version.Targets.FirstOrDefault(T => T.Name == "minecraft")?.Version;

        public static string ToTitleCase(this string text) => TI.ToTitleCase(text);
    }
}