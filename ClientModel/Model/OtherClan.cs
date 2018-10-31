using GameCore.Model;
using Noname.BitConversion;

namespace ClientModel.Data
{
    public class OtherClan : ClanBase
    {
        static public readonly InheritableVariableLengthBitConverter<OtherClan> BitConverter;

        static OtherClan()
        {
            VariableLengthBitConverterBuilder<OtherClan, ClanBase> builder = new VariableLengthBitConverterBuilder<OtherClan, ClanBase>();
            builder.SetBaseBitConverter(BaseBitConverter);
            BitConverter = builder.Finalize();
        }
    }
}