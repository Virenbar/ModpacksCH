using System.CommandLine;

namespace ModpacksCH.Options
{
    internal class DirectoryOption : Option<DirectoryInfo>
    {
        public DirectoryOption() : base(new[] { "--path", "-p" }, "Directory to save modpack")
        {
            SetDefaultValue(new DirectoryInfo(Directory.GetCurrentDirectory()));
            Arity = ArgumentArity.ExactlyOne;
            this.ExistingOnly();
        }
    }
}