using GameCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data
{
    public class Product
    {
        public ConsumerProductType ProductID { get; set; }
        public int Price { get; set; }

        public Product(ConsumerProductType productType, int price)
        {
            ProductID = productType;
            Price = price;
        }
    }
}
