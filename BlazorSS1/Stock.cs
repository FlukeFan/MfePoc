namespace MfePoc.BlazorSS1
{
    public static class Stock
    {
        public static int Red { get; private set; }
        public static int Green { get; private set; }
        public static int Blue { get; private set; }

        public static void GenerateRed()
        {
            Red++;
        }

        public static void GenerateGreen()
        {
            Green++;
        }

        public static void GenerateBlue()
        {
            Blue++;
        }
    }
}
