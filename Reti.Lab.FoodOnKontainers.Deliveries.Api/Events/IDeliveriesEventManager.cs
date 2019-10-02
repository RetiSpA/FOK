using Reti.Lab.FoodOnKontainers.Events.DTO.Delivery;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Api.Events
{
    public interface IDeliveriesEventManager
    {
        /// <summary>
        /// Pubblica l'evento "Presa in consegna da rider" su coda Order
        /// </summary>
        /// <param name="deliveryPickedUp">Dati del messaggio</param>
        void DeliveryPickedUp(DeliveryPickedUpEvent deliveryPickedUp);

        /// <summary>
        /// Pubblica l'evento "Consegna completata da rider" su coda Order
        /// </summary>
        /// <param name="deliveryCompleted">Dati del messaggio</param>
        void DeliveryCompleted(DeliveryCompletedEvent deliveryCompleted);
    }
}
