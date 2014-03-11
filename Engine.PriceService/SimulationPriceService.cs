using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Data.Objects;

namespace Engine.PriceService
{
    /// <summary>
    /// Price feed simulation spins up random prices and quantities every 100ms.
    /// Todo: Figure out better way to handle locking, seems a bit overkill
    /// </summary>
    public class SimulationPriceService : IPriceService
    {
        public bool IsStarted { get; private set; }

        private readonly Random _random;

        // instrumentId -> uniquekey -> subscriptions
        private readonly Dictionary<uint, Dictionary<string, Subscription>> _subscriptions = new Dictionary<uint, Dictionary<string, Subscription>>();

        private readonly Dictionary<uint, uint> _totalVolumes = new Dictionary<uint, uint>(); 

        private readonly Timer _timer;

        public SimulationPriceService()
        {
            _random = new Random();
            _timer = new Timer(100);
            _timer.Elapsed += CreateNewPriceSnapshot;
        }

        public void Start()
        {
            _timer.Start();
            IsStarted = true;
        }

        public void Stop()
        {
            _timer.Stop();
            IsStarted = false;
        }

        public bool HasSubscription(string receiverUniqueKey, uint instrumentId)
        {
            lock (_subscriptions)
            {
                Dictionary<string, Subscription> instrumentSubscriptions;
                if (!_subscriptions.TryGetValue(instrumentId, out instrumentSubscriptions))
                {
                    return false;
                }
                
                Subscription testSubscription;
                if (!instrumentSubscriptions.TryGetValue(receiverUniqueKey, out testSubscription))
                {
                    return false;
                }
                return true;
            }
        }

        public bool Subscribe(string receiverUniqueKey, uint instrumentId, Action<PriceSnapshot> action)
        {
            try
            {
                lock (_subscriptions)
                {
                    var subscription = new Subscription
                        {
                            UniqueKey = receiverUniqueKey,
                            InstrumentId = instrumentId,
                            Action = action
                        };
                    Dictionary<string, Subscription> instrumentSubscriptions;
                    if (!_subscriptions.TryGetValue(instrumentId, out instrumentSubscriptions))
                    {
                        instrumentSubscriptions = new Dictionary<string, Subscription>();
                        _subscriptions.Add(instrumentId, instrumentSubscriptions);
                        uint totalVolume;
                        if (!_totalVolumes.TryGetValue(instrumentId, out totalVolume))
                        {
                            _totalVolumes.Add(instrumentId, 0);
                        }
                    }

                    Subscription testSubscription;
                    if (instrumentSubscriptions.TryGetValue(receiverUniqueKey, out testSubscription))
                    {
                        Console.WriteLine("Receiver: {0} is already subscribed to instrument id: {1}", receiverUniqueKey, instrumentId);
                        return false;
                    }
                    instrumentSubscriptions.Add(receiverUniqueKey, subscription);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }


        public bool Unsubscribe(string receiverUniqueKey, uint instrumentId)
        {
            try
            {
                lock (_subscriptions)
                {
                    _subscriptions[instrumentId].Remove(receiverUniqueKey);
                    if (_subscriptions[instrumentId].Count == 0)
                    {
                        _subscriptions.Remove(instrumentId);
                        //_totalVolumes.Remove(instrumentId); Keep totalVolumesAround
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        private void CreateNewPriceSnapshot(object sender, ElapsedEventArgs e)
        {
            lock (_subscriptions)
            {
                foreach (var instrumentId in _subscriptions.Keys)
                {
                    var priceSnapshot = new PriceSnapshot
                        {
                            InstrumentId = instrumentId,
                            BidPx = _random.Next(1, 100)/10.0,
                            AskPx = _random.Next(1, 100)/10.0,
                            BidQty = (uint) _random.Next(1, 10),
                            AskQty = (uint) _random.Next(1, 10),
                            TotalVolume = (uint) _random.Next(1, 10) + _totalVolumes[instrumentId]
                        };
                    _totalVolumes[instrumentId] = priceSnapshot.TotalVolume;

                    foreach (var subscription in _subscriptions[instrumentId].Values)
                    {
                        subscription.Action(priceSnapshot);
                    }
                }
            }
        }

        // Unused fields now but potentially could be used later
        private class Subscription
        {
            public string UniqueKey;
            public uint InstrumentId;
            public Action<PriceSnapshot> Action;
        }
    }
}
