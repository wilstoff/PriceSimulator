using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Objects;
using Engine.PriceService;

namespace PriceViewer.ViewModel
{
    public class PriceViewerViewModel
    {
        public ObservableCollection<QuoteViewModel> Quotes { get; private set; }

        public IPriceService PriceService { get; set; }

        public Instrument CurrentlySelectedInstrument { get; set; }

        public ObservableCollection<Instrument> AllInstruments { get; private set; } 

        public PriceViewerViewModel()
        {
            AllInstruments = new ObservableCollection<Instrument>(Instrument.GetInstruments());
            Quotes = new ObservableCollection<QuoteViewModel>();

            PriceService = new SimulationPriceService();
        }
    }
}
