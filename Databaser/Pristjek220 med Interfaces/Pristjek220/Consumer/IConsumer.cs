using System.Collections.Generic;
using Pristjek220Data;

namespace Consumer
{
    public interface IConsumer
    {
        Store FindCheapestStore(string productName);
    }
}
