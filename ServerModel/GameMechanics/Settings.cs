using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.GameMechanics
{
    public abstract class SettingsBase
    {
        public abstract int StartVirusesCountByOwner { get;}
        public abstract int StartVirusesCountByNeutral { get;}
    }

    public class OneByOneSettings : SettingsBase
    {
        public override int StartVirusesCountByOwner { get; } = 100;
        public override int StartVirusesCountByNeutral { get; } = 0;
    }
}
