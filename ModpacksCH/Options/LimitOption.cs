using System.CommandLine;
using System.CommandLine.Parsing;

namespace ModpacksCH.Options
{
    internal class LimitOption : Option<int?>
    {
        /// <summary>
        /// Initializes a new instance of "--limit" option
        /// </summary>
        public LimitOption() : base(new[] { "--limit", "-l" }, Parce, false, "Entry limit") { }

        private static int? Parce(ArgumentResult result)
        {
            if (result.Tokens.Count == 0) { return null; }
            try
            {
                return int.Parse(result.Tokens[0].Value);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return null;
            }
        }
    }
}