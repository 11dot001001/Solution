using Noname.BitConversion;
using Noname.BitConversion.System;

namespace GameCore.Model
{
    public class ProductPlace
    {
        static public readonly InheritableVariableLengthBitConverter<ProductPlace> BitConverter;

        static ProductPlace()
        {
            VariableLengthBitConverterBuilder<ProductPlace> builder = new VariableLengthBitConverterBuilder<ProductPlace>();
            //builder.AddField(a => (byte)a._place_1, (a, place_1) => a._place_1 = (ConsumerProductType)place_1, ByteBitConverter.Instance);
            // builder.AddField(a => a._characteristic_B, (a, characteristic_B) => a._characteristic_B = characteristic_B, Int32BitConverter.Instance);
            BitConverter = builder.Finalize();
        }

        private ConsumerProductType? _place_1;

        public ProductPlace() { }

        public ConsumerProductType? Place_1 { get => _place_1; set => _place_1 = value; }
        public ConsumerProductType? Place_2 { get; set; }
        public ConsumerProductType? Place_3 { get; set; }
        public ConsumerProductType? Place_4 { get; set; }
        public ConsumerProductType? Place_5 { get; set; }

    }
}
