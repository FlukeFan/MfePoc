﻿namespace MfePoc.Mixing.Client.Comms
{
    public interface IRequests
    {
        string RequestHostDetail();
        StockLevelResponse RequestStockLevels();
    }
}
