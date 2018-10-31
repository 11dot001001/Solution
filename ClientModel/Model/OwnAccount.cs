using GameCore.Model;
using Noname.BitConversion;


namespace ClientModel.Data
{
    public class OwnAccount : OwnAccountBase<OwnClan, OtherAccount, OtherClan>
    {
        static public readonly InheritableVariableLengthBitConverter<OwnAccount> BitConverter;

        static OwnAccount()
        {
            VariableLengthBitConverterBuilder<OwnAccount, OwnAccountBase<OwnClan, OtherAccount, OtherClan>> builder = new VariableLengthBitConverterBuilder<OwnAccount, OwnAccountBase<OwnClan, OtherAccount, OtherClan>>();
            builder.SetBaseBitConverter(GetOwnBaseBitConverter(OwnClan.OwnBitConverter, OtherAccount.BitConverter, OtherClan.BitConverter));
            BitConverter = builder.Finalize();
        }
    }
}
