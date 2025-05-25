using EventTicketing.Data.Entities.Events;

namespace EventTicketing.Data.Entities.TicketSales
{
    public class TicketSale
    {
        public virtual Guid Id { get; set; }

        public required virtual Event Event { get; set; }
        public required virtual Guid EventId { get; set; }

        public required virtual Guid UserId { get; set; }

        public virtual DateTime PurchaseDate { get; set; }

        public virtual decimal Price
        {
            get => PriceInCents / 100m;
            set => PriceInCents = (int)(value * 100);
        }

        public virtual int PriceInCents { get; set; }
    }
}
