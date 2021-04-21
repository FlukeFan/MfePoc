namespace MfePoc.Generation
{
    public class StockDb
    {
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }

        public void GenerateRed()
        {
            Red++;
        }

        public void GenerateGreen()
        {
            Green++;
        }

        public void GenerateBlue()
        {
            Blue++;
        }
    }
}
