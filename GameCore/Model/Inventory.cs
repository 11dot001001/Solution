using Noname.BitConversion;
using Noname.BitConversion.System;

namespace GameCore.Model
{
    public class Inventory
    {
        static public readonly InheritableVariableLengthBitConverter<Inventory> BitConverter;

        static Inventory()
        {
            VariableLengthBitConverterBuilder<Inventory> builder = new VariableLengthBitConverterBuilder<Inventory>();
            builder.AddField(a => a._freezeBottle, (a, freezeBottle) => a._freezeBottle = freezeBottle, Int32BitConverter.Instance);
            builder.AddField(a => a._runBottle, (a, runBottle) => a._runBottle = runBottle, Int32BitConverter.Instance);
            BitConverter = builder.Finalize();
        }


        private int _freezeBottle;
        private int _runBottle;

        public Inventory() { }

        public int FreezeBottle { get => _freezeBottle; set => _freezeBottle = value; }
        public int RunBottle { get => _runBottle; set => _runBottle = value; }

        public void AddProduct(ConsumerProductType productType, int count)
        {
            switch (productType)
            {
                case ConsumerProductType.FreezeBottle: FreezeBottle += count; break;
                case ConsumerProductType.RunBottle: RunBottle += count; break;
            }
        }
    }
}
