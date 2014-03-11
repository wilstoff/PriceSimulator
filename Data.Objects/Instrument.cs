using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Objects
{
    /// <summary>
    /// Holds display and ticker symbol information. Id is the primary key.
    /// </summary>
    public class Instrument
    {
        public uint Id { get; set; }
        public string DisplayName { get; set; }
        public string TickerSymbol { get; set; }

        public static IEnumerable<Instrument> GetInstruments()
        {
            return new[]
                {
                    new Instrument
                        {
                            Id = 1,
                            DisplayName = "Apple",
                            TickerSymbol = "AAPL"
                        },
                    new Instrument
                        {
                            Id = 2,
                            DisplayName = "Amazon.com",
                            TickerSymbol = "AMZN"
                        },
                    new Instrument
                        {
                            Id = 3,
                            DisplayName = "Electronic Arts",
                            TickerSymbol = "EA"
                        },
                    new Instrument
                        {
                            Id = 4,
                            DisplayName = "Google",
                            TickerSymbol = "GOOG"
                        },
                    new Instrument
                        {
                            Id = 5,
                            DisplayName = "JPMorgan Chase & Co.",
                            TickerSymbol = "JPM"
                        },
                    new Instrument
                        {
                            Id = 6,
                            DisplayName = "Netflix",
                            TickerSymbol = "NFLX"
                        },
                    new Instrument
                        {
                            Id = 7,
                            DisplayName = "Sony",
                            TickerSymbol = "SNE"
                        },
                    new Instrument
                        {
                            Id = 8,
                            DisplayName = "Brent Crude Futures",
                            TickerSymbol = "FUT:B"
                        },
                    new Instrument
                        {
                            Id = 9,
                            DisplayName = "Henry Index Future",
                            TickerSymbol = "FUT:HIS"
                        },
                    new Instrument
                        {
                            Id = 10,
                            DisplayName = "Russell 1000(R) Index Mini Futures",
                            TickerSymbol = "FUT:RF"
                        }
                };
        } 
    }
}
