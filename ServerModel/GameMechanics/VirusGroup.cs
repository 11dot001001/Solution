using GameCore.Model;
using GameCore.Tools;
using Noname.BitConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.GameMechanics
{
    public class VirusGroup : VirusGroupBase
    {
        static public readonly InheritableVariableLengthBitConverter<VirusGroup> BitConverter;

        static VirusGroup()
        {
            VariableLengthBitConverterBuilder<VirusGroup, VirusGroupBase> builder = new VariableLengthBitConverterBuilder<VirusGroup, VirusGroupBase>();
            builder.SetBaseBitConverter(BaseBitConverter);
            builder.AddField(x => x._start, (x, start) => x._start = start, ReliableBitConverter.GetInstance(Bacterium.BitConverter));
            builder.AddField(x => x._end, (x, end) => x._end = end, ReliableBitConverter.GetInstance(Bacterium.BitConverter));
            BitConverter = builder.Finalize();
        }

        private Bacterium _start;
        private Bacterium _end;

        public VirusGroup() : base() { }
        public VirusGroup(Bacterium start, Bacterium end, int virusCount, Road road, float speed) : base(virusCount, road, speed)
        {
            _start = start;
            _end = end;
        }
    }
}
