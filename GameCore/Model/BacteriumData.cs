﻿using GameCore.Enums;
using Noname.BitConversion;
using Noname.BitConversion.System;

namespace GameCore.Model
{
    public struct BacteriumData
    {
        public sealed class BitConverter : ConstantLengthBitConverter<BacteriumData>
        {
            static public readonly BitConverter Instance;

            static BitConverter() => Instance = new BitConverter();
            public BitConverter() : base(Int32BitConverter.Instance.ByteCount + ByteBitConverter.Instance.ByteCount + Transform.BitConverter.Instance.ByteCount + Int32BitConverter.Instance.ByteCount) { }

            public override sealed void GetBytes(BacteriumData value, byte[] bytes, int offset)
            {
                if (value.Owner == OwnerType.None)
                    throw new System.Exception();
                Int32BitConverter.Instance.GetBytes(value._id, bytes, ref offset);
                ByteBitConverter.Instance.GetBytes((byte)value._owner, bytes, ref offset);
                Transform.BitConverter.Instance.GetBytes(value._transform, bytes, ref offset);
                Int32BitConverter.Instance.GetBytes(value._virusCount, bytes, ref offset);
            }
            public override sealed BacteriumData GetInstance(byte[] bytes, int startIndex) => new BacteriumData(Int32BitConverter.Instance.GetInstance(bytes, ref startIndex), (OwnerType)ByteBitConverter.Instance.GetInstance(bytes, ref startIndex), Transform.BitConverter.Instance.GetInstance(bytes, ref startIndex), Int32BitConverter.Instance.GetInstance(bytes, ref startIndex));
        }

        internal int _id;
        internal OwnerType _owner;
        internal Transform _transform;
        internal int _virusCount;

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
