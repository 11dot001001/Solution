using System;
using System.Linq;
using GameCore.Enums;
using GameCore.Tools;
using Noname.BitConversion;
using Noname.BitConversion.System;

namespace GameCore.Model
{
    public struct VirusGroupData
    {
        public sealed class BitConverter : ConstantLengthBitConverter<VirusGroupData>
        {
            static public readonly BitConverter Instance;

            static BitConverter() => Instance = new BitConverter();
            public BitConverter() : base(Int32BitConverter.Instance.ByteCount + Int32BitConverter.Instance.ByteCount + Int32BitConverter.Instance.ByteCount + Int32BitConverter.Instance.ByteCount + ByteBitConverter.Instance.ByteCount) { }

            public override sealed void GetBytes(VirusGroupData value, byte[] bytes, int offset)
            {
                Int32BitConverter.Instance.GetBytes(value._roadId, bytes, ref offset);
                Int32BitConverter.Instance.GetBytes(value._startBacteriumId, bytes, ref offset);
                Int32BitConverter.Instance.GetBytes(value._startBacteriumVirusCount, bytes, ref offset);
                Int32BitConverter.Instance.GetBytes(value._endBacteriumId, bytes, ref offset);
                ByteBitConverter.Instance.GetBytes((byte)value._owner, bytes, ref offset);
            }
            public override sealed VirusGroupData GetInstance(byte[] bytes, int startIndex) => new VirusGroupData(Int32BitConverter.Instance.GetInstance(bytes, ref startIndex), Int32BitConverter.Instance.GetInstance(bytes, ref startIndex), Int32BitConverter.Instance.GetInstance(bytes, ref startIndex), Int32BitConverter.Instance.GetInstance(bytes, ref startIndex), (OwnerType)ByteBitConverter.Instance.GetInstance(bytes, ref startIndex));
        }

        private int _roadId;
        private int _startBacteriumId;
        private int _startBacteriumVirusCount;
        private int _endBacteriumId;
        private OwnerType _owner;

        public VirusGroupData(VirusGroupData virusGroupData, OwnerType owner) : this(virusGroupData._roadId, virusGroupData._startBacteriumId, virusGroupData._startBacteriumVirusCount, virusGroupData._endBacteriumId, virusGroupData._owner) { }
        public VirusGroupData(int roadId, int startBacteriumId, int startBacteriumVirusCount, int endBacteriumId) : this(roadId, startBacteriumId, startBacteriumVirusCount, endBacteriumId, OwnerType.None) { }
        public VirusGroupData(int roadId, int startBacteriumId, int startBacteriumVirusCount, int endBacteriumId, OwnerType owner)
        {
            _roadId = roadId;
            _startBacteriumId = startBacteriumId;
            _startBacteriumVirusCount = startBacteriumVirusCount;
            _endBacteriumId = endBacteriumId;
            _owner = owner;
        }

        public int RoadId => _roadId;
        public int StartBacteriumId => _startBacteriumId;
        public int StartBacteriumVirusCount => _startBacteriumVirusCount;
        public int EndBacteriumId => _endBacteriumId;
        public OwnerType Owner => _owner;
    }
}