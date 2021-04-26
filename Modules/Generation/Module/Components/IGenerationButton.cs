using System;

namespace MfePoc.Generation.Components
{
    public interface IGenerationButton
    {
        Func<StockLevels> StockLevels { get; set; }
    }
}
