using Noname.BitConversion;
using Noname.BitConversion.System;

namespace ClientModel.Model
{
    public class VirusGroupNet
    {
        static public readonly InheritableVariableLengthBitConverter<VirusGroupNet> NetworkBitConverter;

        static VirusGroupNet()
        {
            VariableLengthBitConverterBuilder<VirusGroupNet> builder = new VariableLengthBitConverterBuilder<VirusGroupNet>();
            builder.AddField(x => { return int.MaxValue; }, (x, startId) => x._startId = startId, Int32BitConverter.Instance);
            builder.AddField(x => { return int.MaxValue; }, (x, endId) => x._endId = endId, Int32BitConverter.Instance);
            builder.AddField(x => { return int.MaxValue; }, (x, roadId) => x._roadId = roadId, Int32BitConverter.Instance);
            builder.AddField(x => { return int.MaxValue; }, (x, virusCount) => x._virusCount = virusCount, Int32BitConverter.Instance);
            builder.AddField(x => { return int.MaxValue; }, (x, speed) => x._speed = speed, SingleBitConverter.Instance);
            NetworkBitConverter = builder.Finalize();
        }

        private int _startId;
        private int _endId;
        private int _roadId;
        private int _virusCount;
        private float _speed;

        public int StartId => _startId;
        public int EndId => _endId;
        public int RoadId => _roadId;
        public int VirusCount => _virusCount;
        public float Speed => _speed;
    }
}
