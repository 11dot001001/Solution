using System;
using System.Linq;
using GameCore.Tools;
using Noname.BitConversion;
using Noname.BitConversion.System;

namespace GameCore.Model
{
    public class VirusGroup
    {
        static public readonly InheritableVariableLengthBitConverter<VirusGroup> NetworkBitConverter;

        static VirusGroup()
        {
            VariableLengthBitConverterBuilder<VirusGroup> builder = new VariableLengthBitConverterBuilder<VirusGroup>();
            builder.AddField(x => x._start.Id, (x, start) => { }, Int32BitConverter.Instance);
            builder.AddField(x => x._end.Id, (x, end) => { }, Int32BitConverter.Instance);
            builder.AddField(a => a._start.Roads.First(x=> x.Key == a._end.Id).Value.Roads.FindIndex(x => x == a._road), (a, road) => { }, Int32BitConverter.Instance);
            builder.AddField(a => a._virusCount, (a, virusCount) => { }, Int32BitConverter.Instance);
            builder.AddField(a => a._speed, (a, speed) => { }, SingleBitConverter.Instance);
            NetworkBitConverter = builder.Finalize();
        }
         
        private BacteriumBase _start;
        private BacteriumBase _end;
        private Road _road;
        private int _virusCount;
        private float _speed;

        public VirusGroup() { }
        public VirusGroup(BacteriumBase start, BacteriumBase end, int virusCount, Road road, float speed)
        {
            _start = start;
            _end = end;
            _virusCount = virusCount;
            _road = road ?? throw new ArgumentNullException(nameof(road));
            _speed = speed;
        }

        public int VirusCount => _virusCount;
        public Road Road => _road;
        public float Speed => _speed;
    }
}