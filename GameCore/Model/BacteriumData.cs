using GameCore.Enums;
using Noname.BitConversion;
using Noname.BitConversion.System;

namespace GameCore.Model
{
    public struct BacteriumData
    {
        public sealed class BitConverter : ConstantLengthBitConverter<BacteriumData>
        {
            static private readonly int _initializeInstanceByteCount;
            static private readonly int _dataInstanceByteCount;
            static public readonly BitConverter InitializeInstance;
            static public readonly BitConverter DataInstance;

            static BitConverter()
            {
                _initializeInstanceByteCount = Int32BitConverter.Instance.ByteCount + ByteBitConverter.Instance.ByteCount + Transform.BitConverter.Instance.ByteCount + Int32BitConverter.Instance.ByteCount;
                _dataInstanceByteCount = Int32BitConverter.Instance.ByteCount + ByteBitConverter.Instance.ByteCount + Int32BitConverter.Instance.ByteCount;
                InitializeInstance = new BitConverter(_initializeInstanceByteCount);
                DataInstance = new BitConverter(_dataInstanceByteCount);
            }
            public BitConverter(int byteCount) : base(byteCount) { }

            public override sealed void GetBytes(BacteriumData value, byte[] bytes, int offset)
            {
                if (value.Owner == OwnerType.None)
                    throw new System.Exception();
                if (ByteCount == _initializeInstanceByteCount)
                {
                    Int32BitConverter.Instance.GetBytes(value._id, bytes, ref offset);
                    ByteBitConverter.Instance.GetBytes((byte)value._owner, bytes, ref offset);
                    Transform.BitConverter.Instance.GetBytes(value._transform, bytes, ref offset);
                    Int32BitConverter.Instance.GetBytes(value._virusCount, bytes, ref offset);
                }
                else
                {
                    Int32BitConverter.Instance.GetBytes(value._id, bytes, ref offset);
                    ByteBitConverter.Instance.GetBytes((byte)value._owner, bytes, ref offset);
                    Int32BitConverter.Instance.GetBytes(value._virusCount, bytes, ref offset);
                }
            }

            public override sealed BacteriumData GetInstance(byte[] bytes, int startIndex) => ByteCount == _initializeInstanceByteCount
                    ? new BacteriumData(Int32BitConverter.Instance.GetInstance(bytes, ref startIndex), (OwnerType)ByteBitConverter.Instance.GetInstance(bytes, ref startIndex), Transform.BitConverter.Instance.GetInstance(bytes, ref startIndex), Int32BitConverter.Instance.GetInstance(bytes, ref startIndex))
                    : new BacteriumData(Int32BitConverter.Instance.GetInstance(bytes, ref startIndex), (OwnerType)ByteBitConverter.Instance.GetInstance(bytes, ref startIndex), Int32BitConverter.Instance.GetInstance(bytes, ref startIndex));
        }

        private int _id;
        private OwnerType _owner;
        private Transform _transform;
        private int _virusCount;

        public BacteriumData(int id, OwnerType owner, int virusCount) : this(id, owner, new Transform(), virusCount) { }
        public BacteriumData(int id, OwnerType owner, Transform transform, int virusCount)
        {
            _id = id;
            _owner = owner;
            _transform = transform;
            _virusCount = virusCount;
        }

        public int Id => _id;
        public Transform Transform => _transform;
        public int VirusCount => _virusCount;
        public OwnerType Owner => _owner;
    }
}
