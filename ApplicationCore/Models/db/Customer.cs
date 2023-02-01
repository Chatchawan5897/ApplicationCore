using System;
using System.Collections.Generic;

namespace ApplicationCore.Models.db
{
    public  class Customer
    {
        public Customer()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int CusId { get; set; }
        public string CusName { get; set; }
        public string CusAddress { get; set; }
        public string CusEmail { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
