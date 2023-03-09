using System.CommandLine;

namespace ModpacksCH.Options
{
    internal class DirectoryOption : Option<DirectoryInfo>
    {
        /// <summary>
        /// Initializes a new instance of "--path" option
        /// </summary>
        public DirectoryOption() : base(new[] { "--path", "-p" }, "Directory to save modpack")
        {
            SetDefaultValue(new DirectoryInfo(Directory.GetCurrentDirectory()));
            Arity = ArgumentArity.ExactlyOne;
            this.ExistingOnly();
        }
    }
}