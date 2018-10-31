using GameCore.Model;
using Noname.BitConversion;


namespace ClientModel.Data
{
    public class OwnClan : OwnClanBase<OtherAccount, OtherClan>
    {
        static public readonly InheritableVariableLengthBitConverter<OwnClan> OwnBitConverter;
        
        static OwnClan()
        {
            VariableLengthBitConverterBuilder<OwnClan, OwnClanBase<OtherAccount, OtherClan>> builder = new VariableLengthBitConverterBuilder<OwnClan, OwnClanBase<OtherAccount, OtherClan>>();
            builder.SetBaseBitConverter(GetOwnBaseBitConverter(OtherAccount.BitConverter));
            OwnBitConverter = builder.Finalize();
        }
    }
}
