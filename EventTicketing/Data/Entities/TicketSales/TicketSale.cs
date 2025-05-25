using EventTicketing.Data.Entities.Events;

namespace EventTicketing.Data.Entities.TicketSales
{
    public class TicketSale
    {
        public virtual string Id { get; set; }

        public required virtual Event Event { get; set; }
        public required virtual string EventId { get; set; }

        public required virtual string UserId { get; set; }

        public virtual DateTime PurchaseDate { get; set; }

        public virtual decimal Price
        {
            get => PriceInCents / 100m;
            set => PriceInCents = (int)(value * 100);
        }

        public virtual int PriceInCents { get; set; }
    }
}
