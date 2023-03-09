namespace ModpacksCH
{
    internal static class Debug
    {
        internal static void Invoke()
        {
            // Valid
            //Program.Invoke("a");
            //Program.Invoke("s stoneblock");
            //Program.Invoke("i 100");
            //Program.Invoke("d 100");
            Program.Invoke("s ATM8");
            //Program.Invoke("i 520914");
            //Program.Invoke("d 520914");
            // Invalid
            //
            Environment.Exit(0);
        }
    }
}