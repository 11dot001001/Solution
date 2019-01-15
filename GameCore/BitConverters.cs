using GameCore.Tools;
using ILibrary.Maths.Geometry2D;
using Noname.BitConversion;
using Noname.BitConversion.System;
using UnityEngine;

namespace GameCore
{
    public sealed class Vector2BitConverter : ConstantLengthBitConverter<Vector2>
    {
        static public readonly Vector2BitConverter Instance;

        static Vector2BitConverter() => Instance = new Vector2BitConverter();
        public Vector2BitConverter() : base(SingleBitConverter.Instance.ByteCount + SingleBitConverter.Instance.ByteCount) { }

        public override void GetBytes(Vector2 value, byte[] bytes, int offset)
        {
            SingleBitConverter.Instance.GetBytes(value.x, bytes, ref offset);
            SingleBitConverter.Instance.GetBytes(value.y, bytes, ref offset);
        }
        public override Vector2 GetInstance(byte[] bytes, int startIndex) => new Vector2(SingleBitConverter.Instance.GetInstance(bytes, ref startIndex), SingleBitConverter.Instance.GetInstance(bytes, ref startIndex));
    }

    public sealed class CircleBitConverter : ConstantLengthBitConverter<Circle>
    {
        static public readonly CircleBitConverter Instance;

        static CircleBitConverter() => Instance = new CircleBitConverter();
        public CircleBitConverter() : base(Vector2BitConverter.Instance.ByteCount + SingleBitConverter.Instance.ByteCount) { }

        public override void GetBytes(Circle value, byte[] bytes, int offset)
        {
            Vector2BitConverter.Instance.GetBytes(value.Position, bytes, ref offset);
            SingleBitConverter.Instance.GetBytes(value.Radius, bytes, ref offset);
        }
        public override Circle GetInstance(byte[] bytes, int startIndex) => new Circle(Vector2BitConverter.Instance.GetInstance(bytes, ref startIndex), SingleBitConverter.Instance.GetInstance(bytes, ref startIndex));
    }
}