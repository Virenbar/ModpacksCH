using System.CommandLine;
using System.CommandLine.Builder;
using System.Diagnostics;
using System.Text;

namespace DynmapImageExport.Options
{
    internal class TraceOption : Option<bool>
    {
        private static TraceListener TL;

        /// <summary>
        /// Initializes a new instance of "--trace" option
        /// </summary>
        public TraceOption() : base(new[] { "--trace", "-t" }, "Write trace log") { }

        public static CommandLineBuilder AddToBuilder(CommandLineBuilder builder)
        {
            var Option = new TraceOption();
            builder.Command.AddGlobalOption(Option);
            builder.AddMiddleware((context, next) =>
            {
                var trace = context.ParseResult.FindResultFor(Option) is not null;
                if (trace && !Trace.Listeners.Contains(TL))
                {
                    TL ??= new LogListener();
                    Trace.Listeners.Add(TL);
                }
                else
                {
                    Trace.Listeners.Remove(TL);
                }
                return next(context);
            });
            return builder;
        }

        private class LogListener : TraceListener
        {
            private readonly TextWriter Writer;

            public LogListener()
            {
                Writer = new StreamWriter("trace.log", true, Encoding.UTF8) { AutoFlush = true };
            }

            private static string TimeStamp => $"[{DateTime.Now:HH:mm:ss.fff}]";

            public override void Write(string message) => Writer.Write($"{TimeStamp} {message}");

            public override void WriteLine(string message) => Writer.WriteLine($"{TimeStamp} {message}");

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                Writer.Dispose();
            }
        }
    }
}