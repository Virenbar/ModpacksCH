using System.CommandLine;

namespace ModpacksCH.Options
{
    internal class ServerOption : Option<bool>
    {
        /// <summary>
        /// Initializes a new instance of "--server" option
        /// </summary>
        public ServerOption() : base(new[] { "--server", "-s" }, "Download server version")
        {
            Arity = ArgumentArity.Zero;
        }
    }
}