using GameCore.Model;
using Noname.BitConversion;

namespace ClientModel.Data
{
    public class OtherAccount : AccountBase<OtherClan>
    {
        static public readonly InheritableVariableLengthBitConverter<OtherAccount> BitConverter;

        static OtherAccount()
        {
            VariableLengthBitConverterBuilder<OtherAccount, AccountBase<OtherClan>> builder = new VariableLengthBitConverterBuilder<OtherAccount, AccountBase<OtherClan>>();
            builder.SetBaseBitConverter(GetBaseBitConverter(OtherClan.BitConverter));
            BitConverter = builder.Finalize();
        }
    }
}
