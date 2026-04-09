using System;
using System.Collections.Generic;
using System.Text;
using Android.Net.Wifi.Aware;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace RestaurantApp
{
    [Table("order")]
    public class OrderData : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("userid")]
        public int userid { get; set; }

        [Column("date")]
        public DateTime date { get; set; }

        [Column("eatlocation")]
        public string eatlocation { get; set; }

        [Column("subtotal")]
        public float subtotal { get; set; }

        [Column("deliveryfee")]
        public float deliveryfee { get; set; }

        [Column("iva")]
        public float iva { get; set; }

        [Column("totalprice")]
        public float totalprice { get; set; }

        [Column("status")]
        public string status { get; set; }
    }
}
