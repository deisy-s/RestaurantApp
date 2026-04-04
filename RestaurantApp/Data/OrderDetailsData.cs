using System;
using System.Collections.Generic;
using System.Text;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RestaurantApp
{
    [Table("orderdetails")]
    public class OrderDetailsData : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("orderid")]
        public int orderid { get; set; }

        [Column("menuid")]
        public int menuid { get; set; }

        [Column("comments")]
        public string? comments { get; set; }

        [Column("quantity")]
        public int quantity { get; set; }

        [Column("name")]
        public string name { get; set; }
    }
}
