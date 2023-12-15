using System;

namespace OnlineStore.DataProvide
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }

    }
}

