namespace ModpacksCH.API.Model;

public static class Extensions
{
    public static string ForgeVersion(this Version version) => version.Targets.FirstOrDefault(T => T.Name == "forge")?.Version;

    public static string MinecraftVersion(this Version version) => version.Targets.FirstOrDefault(T => T.Name == "minecraft")?.Version;

    public static string JavaVersion(this Version version) => version.Targets.FirstOrDefault(T => T.Name == "java")?.Version;

    public static Version LatestVersion(this ModpackManifest modpack) => modpack.Versions.OrderByDescending(V => V.ID).FirstOrDefault();
}