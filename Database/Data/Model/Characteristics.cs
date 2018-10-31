using GameCore.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data.Model
{
    public class CharacteristicsConfiguration : ComplexTypeConfiguration<Characteristics>
    {
        public CharacteristicsConfiguration()
        {
            Property(x => x.Characteristic_A).IsRequired();
            Property(x => x.Characteristic_B).IsRequired();
        }
    }
}
