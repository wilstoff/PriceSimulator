using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Objects
{
    /// <summary>
    /// Single snapshot of prices, quantities, and volumes for a particular instrument
    /// </summary>
    public class PriceSnapshot
    {
        public uint InstrumentId { get; set; }
        public double BidPx { get; set; }
        public uint BidQty { get; set; }
        public double AskPx { get; set; }
        public uint AskQty { get; set; }
        public uint TotalVolume { get; set; }
    }
}
