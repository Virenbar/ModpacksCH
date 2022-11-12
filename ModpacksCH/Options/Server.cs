using System.CommandLine;

namespace ModpacksCH.Options
{
    internal class ServerOption : Option<bool>
    {
        public ServerOption() : base(new[] { "--server", "-s" }, "Download server version")
        {
            Arity = ArgumentArity.Zero;
        }
    }
}