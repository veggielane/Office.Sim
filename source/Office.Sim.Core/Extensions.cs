namespace Office.Sim.Core
{
    public static class Extensions
    {
        public static string Fmt(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}
