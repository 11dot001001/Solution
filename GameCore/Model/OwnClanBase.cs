using System;
using System.Collections.Generic;
using Noname.BitConversion;
using Noname.BitConversion.System;
using Noname.BitConversion.System.Collections.Generic;

namespace GameCore.Model
{
    public abstract class OwnClanBase<TAccount, TСlan> : ClanBase where TAccount : AccountBase<TСlan> where TСlan : ClanBase
    {
        static private readonly Dictionary<InheritableVariableLengthBitConverter<TAccount>, InheritableVariableLengthBitConverter<OwnClanBase<TAccount, TСlan>>> _bitConverters;

        static OwnClanBase() => _bitConverters = new Dictionary<InheritableVariableLengthBitConverter<TAccount>, InheritableVariableLengthBitConverter<OwnClanBase<TAccount, TСlan>>>();

        static protected InheritableVariableLengthBitConverter<OwnClanBase<TAccount, TСlan>> GetOwnBaseBitConverter(InheritableVariableLengthBitConverter<TAccount> accountBitConverter)
        {
            if (_bitConverters.TryGetValue(accountBitConverter, out InheritableVariableLengthBitConverter<OwnClanBase<TAccount, TСlan>> clanBitConverter))
                return clanBitConverter;
            AbstractVariableLengthBitConverterBuilder<OwnClanBase<TAccount, TСlan>,ClanBase> builder = new AbstractVariableLengthBitConverterBuilder<OwnClanBase<TAccount, TСlan>, ClanBase>();
            builder.SetBaseBitConverter(BaseBitConverter);
            builder.AddField(a => a._dateOfCreation, (a, dateOfCreation) => a._dateOfCreation = dateOfCreation, DateTimeBitConverter.Instance);
            builder.AddField(a => a._seasonalPoints, (a, seasonalPoints) => a._seasonalPoints = seasonalPoints, Int32BitConverter.Instance);
            builder.AddField(a => a._leader, (a, leader) => a._leader = leader, ReliableBitConverter.GetInstance(accountBitConverter));
            builder.AddField(a => a._participants, (a, participants) => a._participants = participants, ICollectionReliableBitConverter.GetInstance(ReliableBitConverter.GetInstance(accountBitConverter)));

            _bitConverters.Add(accountBitConverter, clanBitConverter = builder.Finalize());
            return clanBitConverter;
        }

        private DateTime _dateOfCreation;
        private int _seasonalPoints;
        private TAccount _leader;
        private ICollection<TAccount> _participants;

        protected OwnClanBase() : base() { }
        protected OwnClanBase(int id, string name, DateTime dateOfCreation, int seasonalPoints, TAccount leader, ICollection<TAccount> participants) : base(id, name)
        {
            DateOfCreation = dateOfCreation;
            SeasonalPoints = seasonalPoints;
        }

        public DateTime DateOfCreation { get => _dateOfCreation; set => _dateOfCreation = value; }
        public int SeasonalPoints { get => _seasonalPoints; set => _seasonalPoints = value; }
        public TAccount Leader { get => _leader; set => _leader = value; }
        public ICollection<TAccount> Participants { get => _participants; set => _participants = value; }
    }
}
