using System.Collections.Generic;
using Noname.BitConversion;
using Noname.BitConversion.System;

namespace GameCore.Model
{
    public abstract class AccountBase<TClan> where TClan : ClanBase
    {
        static private readonly Dictionary<InheritableVariableLengthBitConverter<TClan>, InheritableVariableLengthBitConverter<AccountBase<TClan>>> _bitConverters;

        static AccountBase() => _bitConverters = new Dictionary<InheritableVariableLengthBitConverter<TClan>, InheritableVariableLengthBitConverter<AccountBase<TClan>>>();

        protected static InheritableVariableLengthBitConverter<AccountBase<TClan>> GetBaseBitConverter(InheritableVariableLengthBitConverter<TClan> clanBitConverter)
        {
            if (_bitConverters.TryGetValue(clanBitConverter, out InheritableVariableLengthBitConverter<AccountBase<TClan>> accountBitConverter))
                return accountBitConverter;
            AbstractVariableLengthBitConverterBuilder<AccountBase<TClan>> builder = new AbstractVariableLengthBitConverterBuilder<AccountBase<TClan>>();
            builder.AddField(a => a._id, (a, id) => a._id = id, Int32BitConverter.Instance);
            builder.AddField(a => a._nickname, (a, nickname) => a._nickname = nickname, StringBitConverter.UnicodeReliableInstance);
            builder.AddField(a => a._level, (a, level) => a._level = level, Int32BitConverter.Instance);
            builder.AddField(a => a._victories, (a, victories) => a._victories = victories, Int32BitConverter.Instance);
            builder.AddField(a => a._losses, (a, losses) => a._losses = losses, Int32BitConverter.Instance);
            builder.AddField(a => a._clan, (a, clan) => a._clan = clan, ReliableBitConverter.GetInstance(NullableBitConverter.GetInstance(clanBitConverter)));
            _bitConverters.Add(clanBitConverter, accountBitConverter = builder.Finalize());
            return accountBitConverter;
        }

        private int _id;
        private string _nickname;
        private int _level;
        private int _victories;
        private int _losses;
        private TClan _clan;

        public AccountBase() { }
        public AccountBase(string nickname, int level, int victories, int losses)
        {
            Nickname = nickname;
            Level = level;
            Victories = victories;
            Losses = losses;
        }

        public int Id { get => _id; set => _id = value; }
        public string Nickname { get => _nickname; set => _nickname = value; }
        public int Level { get => _level; set => _level = value; }
        public int Victories { get => _victories; set => _victories = value; }
        public int Losses { get => _losses; set => _losses = value; }
        public TClan Clan { get => _clan; set => _clan = value; }
        public bool IsInClan => Clan != null;
    }
}
