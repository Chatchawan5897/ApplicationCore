using System;
using System.Collections.Generic;

namespace ApplicationCore.Models.db
{
    public  class Book
    {
        public int Isbn { get; set; }
        public string TiTle { get; set; }
        public string Description { get; set; }
        public int? Price { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
