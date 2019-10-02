using Reti.Lab.FoodOnKontainers.Events.DTO.Orders;

namespace Reti.Lab.FoodOnKontainers.Orders.Api.Events
{
    public interface IOrdersEventManager
    {
        /// <summary>
        /// Pubblica l'evento "Ordine Accettato da gestore" su coda Delivery
        /// </summary>
        /// <param name="orderAccepted">Dati del messaggio</param>
        void OrderAccepted(OrderAcceptedEvent orderAccepted);

        /// <summary>
        /// Pubblica l'evento "Ordine Rifiutato da gestore" su coda Payment e User
        /// </summary>
        /// <param name="orderRejected">Dati del messaggio</param>
        void OrderRejected(OrderRejectedEvent orderRejected);
    }
}
