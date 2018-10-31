using Noname.BitConversion;
using Noname.BitConversion.System;
using System;

namespace GameCore.Model
{
    public abstract class ClanBase
    {
        static protected readonly InheritableVariableLengthBitConverter<ClanBase> BaseBitConverter;

        static ClanBase()
        {
            AbstractVariableLengthBitConverterBuilder<ClanBase> builder = new AbstractVariableLengthBitConverterBuilder<ClanBase>();
            builder.AddField(a => a._id, (a, id) => a._id = id, Int32BitConverter.Instance);
            builder.AddField(a => a._name, (a, name) => a._name = name, StringBitConverter.UnicodeReliableInstance);
            BaseBitConverter = builder.Finalize();
        }

        protected private int _id;
        protected private string _name;

        public ClanBase() { }
        public ClanBase(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
    }
}
