using System;
using System.Collections.Generic;

namespace ApplicationCore.Models.db
{
    public  class Transaction
    {
        public int Isbn { get; set; }
        public int? CusId { get; set; }
        public int? Quatity { get; set; }
        public int? TotalPrice { get; set; }

        public virtual Customer Cus { get; set; }
        public virtual Book IsbnNavigation { get; set; }
    }
}
