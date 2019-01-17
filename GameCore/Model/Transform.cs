using Devdeb.Maths.Geometry2D;
using Noname.BitConversion;
using Noname.BitConversion.System;
using UnityEngine;

namespace GameCore.Model
{
    public struct Transform
    {
        public sealed class BitConverter : ConstantLengthBitConverter<Transform>
        {
            static public readonly BitConverter Instance;

            static BitConverter() => Instance = new BitConverter();
            public BitConverter() : base(SingleBitConverter.Instance.ByteCount + SingleBitConverter.Instance.ByteCount + CircleBitConverter.Instance.ByteCount) { }

            public override void GetBytes(Transform value, byte[] bytes, int offset)
            {
                SingleBitConverter.Instance.GetBytes(value.MaxBacteriumRadius, bytes, ref offset);
                SingleBitConverter.Instance.GetBytes(value.MinBacteriumRadius, bytes, ref offset);
                CircleBitConverter.Instance.GetBytes(value.Circle, bytes, ref offset);
            }
            public override Transform GetInstance(byte[] bytes, int startIndex) => new Transform(SingleBitConverter.Instance.GetInstance(bytes, ref startIndex), SingleBitConverter.Instance.GetInstance(bytes, ref startIndex), CircleBitConverter.Instance.GetInstance(bytes, ref startIndex));
        }

        private const float _transportFactor = 0.3f;
        private readonly float _maxBacteriumRadius;
        private readonly float _minBacteriumRadius;
        private Circle _circle;

        public Transform(float maxBacteriumRadius, float minBacteriumRadius, Circle circle)
        {
            _maxBacteriumRadius = maxBacteriumRadius;
            _minBacteriumRadius = minBacteriumRadius;
            _circle = circle;
        }

        public Circle Circle { get => _circle; set => _circle = value; }
        public Vector2 Position { get => _circle.Position; set => _circle.Position = value; }
        public float TransportRadius => _circle.Radius + _transportFactor;
        public float BacteriumRadius => _circle.Radius;
        public float MaxBacteriumRadius => _maxBacteriumRadius;
        public float MinBacteriumRadius => _minBacteriumRadius;
    }
}
