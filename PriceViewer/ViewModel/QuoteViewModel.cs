using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Data.Objects;

namespace PriceViewer.ViewModel
{
    public class QuoteViewModel : INotifyPropertyChanged
    {
        private readonly Instrument _instrument;

        private PriceSnapshot _currentPriceSnapshot;

        public QuoteViewModel(Instrument instrument)
        {
            _instrument = instrument;
            //TODO: Figure out what should be the best default value (0s seem too close to real numbers)
            _currentPriceSnapshot = new PriceSnapshot
                {
                    InstrumentId = _instrument.Id,
                    BidPx = 0,
                    BidQty = 0,
                    AskPx = 0,
                    AskQty = 0,
                    TotalVolume = 0
                };
        }

        public uint InstrumentId { get { return _instrument.Id; } }
        public string DisplayName { get { return _instrument.DisplayName; } }
        public string TickerSymbol { get { return _instrument.TickerSymbol; } }

        public double BidPx { get { return _currentPriceSnapshot.BidPx; } }
        public uint BidQty { get { return _currentPriceSnapshot.BidQty; } }
        public double AskPx { get { return _currentPriceSnapshot.AskPx; } }
        public uint AskQty { get { return _currentPriceSnapshot.AskQty; } }
        public uint TotalVolume { get { return _currentPriceSnapshot.TotalVolume; } }

        public PriceSnapshot PriceSnapshot
        {
            get { return _currentPriceSnapshot; }
            set
            {
                if (value != _currentPriceSnapshot && value.InstrumentId == _instrument.Id)
                {
                    _currentPriceSnapshot = value;
                    //TODO: Limit this to only the fields that are actually different
                    OnPropertyChanged("BidPx");
                    OnPropertyChanged("BidQty");
                    OnPropertyChanged("AskPx");
                    OnPropertyChanged("AskQty");
                    OnPropertyChanged("TotalVolume");
                    OnPropertyChanged("PriceSnapshot");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
