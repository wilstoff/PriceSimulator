using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Data.Objects;
using PriceViewer.ViewModel;

namespace PriceViewer
{
    /// <summary>
    /// Note sorry for any non conventional implementations.  This is my first WPF app, as i normally work in winforms.  I thought this would be a good challenge.
    /// Many things I did not get to continue or expand upon where UI facing.  I would like to clean up the instrument selection to include not just a drop down but a separate window for selecting instruments.
    /// This window would have some quick seach feature by symbol or name.  Would also like to play with the idea of being able to drag and drop instrument rows around so that users can see what they like where
    /// they like it.  Needless to say there are tons of improvements that could be done to this UI with more time.  User feedback would be the driving force.
    /// </summary>
    public partial class PriceViewerWindow : Window
    {
        private readonly PriceViewerViewModel _vm;
        public PriceViewerWindow()
        {
            InitializeComponent();
            _vm = new PriceViewerViewModel();
            DataContext = _vm;
        }

        //TODO: check up if i should be doing click events in the view or the viewmodel.  Seems like i should pass this to the viewmodel and keep the view simple.
        //Running out of time....
        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string) StartStopButton.Content == "Start Sim")
            {
                _vm.PriceService.Start();
                StartStopButton.Content = "Stop Sim";
            }
            else
            {
                _vm.PriceService.Stop();
                StartStopButton.Content = "Start Sim";
            }
        }

        private void InstrumentSelectionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _vm.CurrentlySelectedInstrument = (Instrument)InstrumentSelectionBox.SelectedValue;
            if (_vm.PriceService.HasSubscription("PriceViewerWindow", _vm.CurrentlySelectedInstrument.Id))
            {
                SubscribeUnsubscribeButton.Content = "Unsubscribe";
            }
            else
            {
                SubscribeUnsubscribeButton.Content = "Subscribe";
            }
        }

        private void SubscribeUnsubscribe_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.CurrentlySelectedInstrument != null)
            {
                if ((string) SubscribeUnsubscribeButton.Content == "Subscribe")
                {
                    _vm.Quotes.Add(new QuoteViewModel(_vm.CurrentlySelectedInstrument));
                    _vm.PriceService.Subscribe("PriceViewerWindow", _vm.CurrentlySelectedInstrument.Id, UpdatePricesForUi);
                    SubscribeUnsubscribeButton.Content = "Unsubscribe";
                }
                else
                {
                    _vm.PriceService.Unsubscribe("PriceViewerWindow", _vm.CurrentlySelectedInstrument.Id);
                    var quoteToRemove = _vm.Quotes.FirstOrDefault(x => x.InstrumentId == _vm.CurrentlySelectedInstrument.Id);
                    if (quoteToRemove != null)
                    {
                        _vm.Quotes.Remove(quoteToRemove);
                    }
                    SubscribeUnsubscribeButton.Content = "Subscribe";
                }
            }
        }

        private void UpdatePricesForUi(PriceSnapshot newPriceSnapshot)
        {
            var quoteToUpdate = _vm.Quotes.FirstOrDefault(x => x.InstrumentId == newPriceSnapshot.InstrumentId);
            if (quoteToUpdate != null)
            {
                quoteToUpdate.PriceSnapshot = newPriceSnapshot;
            }
        }
    }
}
