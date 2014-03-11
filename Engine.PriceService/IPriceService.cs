using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Objects;

namespace Engine.PriceService
{
    /// <summary>
    /// Interface for a price feed service
    /// </summary>
    public interface IPriceService
    {
        /// <summary>
        /// Start listening to price data
        /// </summary>
        void Start();

        /// <summary>
        /// Stop listening to price data
        /// </summary>
        void Stop();

        /// <summary>
        /// Subscribe an action to be peformed when a new PriceSnaphshot occurs for instrumentId
        /// </summary>
        /// <param name="receiverUniqueKey">A key for each subscriber object</param>
        /// <param name="instrumentId">The instrument to subscribe to</param>
        /// <param name="action">The action to perform when a new PriceSnapshot occurs</param>
        /// <returns></returns>
        bool Subscribe(string receiverUniqueKey, uint instrumentId, Action<PriceSnapshot> action);

        /// <summary>
        /// Unsubscribe an action for the instrument using the key when initially setup
        /// </summary>
        /// <param name="receiverUniqueKey">The key to unsubscribe from</param>
        /// <param name="instrumentId">The instrument to unsubscribe from</param>
        /// <returns></returns>
        bool Unsubscribe(string receiverUniqueKey, uint instrumentId);

        /// <summary>
        /// Is listening to price data
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Does there exist a subscribed action for key and instrument
        /// </summary>
        /// <param name="receiverUniqueKey">The key used to subscribe</param>
        /// <param name="instrumentId">The instrument the key is subscribed on</param>
        /// <returns></returns>
        bool HasSubscription(string receiverUniqueKey, uint instrumentId);
    }
}
