using System;
using GameCore.Tools;
using Noname.BitConversion;
using Noname.BitConversion.System;

namespace GameCore.Model
{
    public abstract class VirusGroupBase
    {
        static protected readonly InheritableVariableLengthBitConverter<VirusGroupBase> BaseBitConverter;

        static VirusGroupBase()
        {
            AbstractVariableLengthBitConverterBuilder<VirusGroupBase> builder = new AbstractVariableLengthBitConverterBuilder<VirusGroupBase>();
            builder.AddField(a => a._virusCount, (a, virusCount) => a._virusCount = virusCount, Int32BitConverter.Instance);
            builder.AddField(a => a._road, (a, roads) => a._road = roads, ReliableBitConverter.GetInstance(Road.BitConverter));
            builder.AddField(a => a._speed, (a, speed) => a._speed = speed, SingleBitConverter.Instance);
            BaseBitConverter = builder.Finalize();
        }

        protected int _virusCount;
        protected Road _road;
        protected float _speed;

        protected VirusGroupBase() { }
        protected VirusGroupBase(int virusCount, Road road, float speed)
        {
            _virusCount = virusCount;
            _road = road ?? throw new ArgumentNullException(nameof(road));
            _speed = speed;
        }

        public int VirusCount => _virusCount;
        public Road Road => _road;
        public float Speed => _speed;
    }
}