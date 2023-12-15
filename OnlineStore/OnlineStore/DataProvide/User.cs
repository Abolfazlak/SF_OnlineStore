using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.DataProvide
{
    public class User
    {
        public User()
        {
            OrderList = new List<Order>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<Order> OrderList { get; set; }
    }
}

