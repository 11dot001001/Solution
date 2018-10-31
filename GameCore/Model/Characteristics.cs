using Noname.BitConversion;
using Noname.BitConversion.System;

namespace GameCore.Model
{
    public class Characteristics
    {
        static public readonly InheritableVariableLengthBitConverter<Characteristics> BitConverter;

        static Characteristics()
        {
            VariableLengthBitConverterBuilder<Characteristics> builder = new VariableLengthBitConverterBuilder<Characteristics>();
            builder.AddField(a => a._characteristic_A, (a, characteristic_A) => a._characteristic_A = characteristic_A, Int32BitConverter.Instance);
            builder.AddField(a => a._characteristic_B, (a, characteristic_B) => a._characteristic_B = characteristic_B, Int32BitConverter.Instance);
            BitConverter = builder.Finalize();
        }

        private int _characteristic_A;
        private int _characteristic_B;

        public Characteristics() { }
        public Characteristics(int characteristic_A, int characteristic_B)
        {
            Characteristic_A = characteristic_A;
            Characteristic_B = characteristic_B;
        }

        public int Characteristic_A { get => _characteristic_A; set => _characteristic_A = value; }
        public int Characteristic_B { get => _characteristic_B; set => _characteristic_B = value; }
    }
}
