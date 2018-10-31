using GameCore.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Database.Data.Model
{
    public class InventoryConfiguration : ComplexTypeConfiguration<Inventory>
    {
        public InventoryConfiguration()
        {
            Property(x => x.FreezeBottle).IsRequired();
            Property(x => x.RunBottle).IsRequired();
        }
    }
}
