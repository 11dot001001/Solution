using GameCore.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data.Model
{
    public class ProductPlaceConfiguration : ComplexTypeConfiguration<ProductPlace>
    {
        public ProductPlaceConfiguration()
        {
            Property(x => x.Place_1).IsOptional();
            Property(x => x.Place_2).IsOptional();
            Property(x => x.Place_3).IsOptional();
            Property(x => x.Place_4).IsOptional();
            Property(x => x.Place_5).IsOptional();
        }
    }
}
