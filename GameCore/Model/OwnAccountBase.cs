using System;
using System.Collections.Generic;
using Noname.BitConversion;
using Noname.BitConversion.System;
using Noname.BitConversion.System.Collections.Generic;

namespace GameCore.Model
{
    public abstract class OwnAccountBase<TOwnClan, TAccount, TClan> : AccountBase<TOwnClan> where TOwnClan : OwnClanBase<TAccount, TClan> where TAccount : AccountBase<TClan> where TClan : ClanBase
    {
        internal struct Info
        {
            internal readonly InheritableVariableLengthBitConverter<TOwnClan> _ownClanBitConverter;
            internal readonly InheritableVariableLengthBitConverter<TAccount> _otherAccountBitConverter;
            internal readonly InheritableVariableLengthBitConverter<TClan> _otherClanBitConverter;

            public Info(InheritableVariableLengthBitConverter<TOwnClan> ownClanBitConverter, InheritableVariableLengthBitConverter<TAccount> otherAccountBitConverter, InheritableVariableLengthBitConverter<TClan> otherClanBitConverter)
            {
                _ownClanBitConverter = ownClanBitConverter;
                _otherAccountBitConverter = otherAccountBitConverter;
                _otherClanBitConverter = otherClanBitConverter;
            }
        }

        static private readonly Dictionary<Info, InheritableVariableLengthBitConverter<OwnAccountBase<TOwnClan, TAccount, TClan>>> _bitConverters;

        static OwnAccountBase() => _bitConverters = new Dictionary<Info, InheritableVariableLengthBitConverter<OwnAccountBase<TOwnClan, TAccount, TClan>>>();

        static protected InheritableVariableLengthBitConverter<OwnAccountBase<TOwnClan, TAccount, TClan>> GetOwnBaseBitConverter(InheritableVariableLengthBitConverter<TOwnClan> ownClanBitConverter, InheritableVariableLengthBitConverter<TAccount> accountBitConverter, InheritableVariableLengthBitConverter<TClan> otherClanBitConverter)
        {
            Info info = new Info(ownClanBitConverter, accountBitConverter, otherClanBitConverter);
            if (_bitConverters.TryGetValue(info, out InheritableVariableLengthBitConverter<OwnAccountBase<TOwnClan, TAccount, TClan>> ownAccountBitConverter))
                return ownAccountBitConverter;
            AbstractVariableLengthBitConverterBuilder<OwnAccountBase<TOwnClan, TAccount, TClan>, AccountBase<TOwnClan>> builder = new AbstractVariableLengthBitConverterBuilder<OwnAccountBase<TOwnClan, TAccount, TClan>, AccountBase<TOwnClan>>();
            builder.SetBaseBitConverter(GetBaseBitConverter(ownClanBitConverter));
            builder.AddField(a => a._friends, (a, friends) => a._friends = friends, ICollectionReliableBitConverter.GetInstance(ReliableBitConverter.GetInstance(accountBitConverter)));
            builder.AddField(a => (byte)a._accountStatus, (a, accountStatus) => a._accountStatus = (AccountStatus)accountStatus, ByteBitConverter.Instance);
            builder.AddField(a => a._money, (a, money) => a._money = money, Int32BitConverter.Instance);
            builder.AddField(a => a._levelPoints, (a, levelPoints) => a._levelPoints = levelPoints, Int32BitConverter.Instance);
            builder.AddField(a => a._seasonalPoints, (a, seasonalPoints) => a._seasonalPoints = seasonalPoints, Int32BitConverter.Instance);
            builder.AddField(a => a._characteristics, (a, characteristics) => a._characteristics = characteristics, ReliableBitConverter.GetInstance(Characteristics.BitConverter));
            builder.AddField(a => a._characteristicsPoints, (a, characteristicsPoints) => a._characteristicsPoints = characteristicsPoints, Int32BitConverter.Instance);
            builder.AddField(a => a._inventory, (a, inventory) => a._inventory = inventory, ReliableBitConverter.GetInstance(Inventory.BitConverter));
            builder.AddField(a => a._productPlace, (a, productPlace) => a._productPlace = productPlace, ReliableBitConverter.GetInstance(ProductPlace.BitConverter));
            builder.AddField(a => a._dateOfMembership, (a, dateOfMembership) => a._dateOfMembership = dateOfMembership, DateTimeBitConverter.Instance);
            builder.AddField(a => a._clanPoints, (a, clanPoints) => a._clanPoints = clanPoints, Int32BitConverter.Instance);
            _bitConverters.Add(info, ownAccountBitConverter = builder.Finalize());
            return ownAccountBitConverter;
        }

        private ICollection<TAccount> _friends;
        private AccountStatus _accountStatus;
        private int _money;
        private int _levelPoints;
        private int _seasonalPoints;
        private Characteristics _characteristics;
        private int _characteristicsPoints;
        private Inventory _inventory;
        private ProductPlace _productPlace;
        private DateTime _dateOfMembership;
        private int _clanPoints;

        protected OwnAccountBase() : base() { }
        protected OwnAccountBase(string nickname, int level, int victories, int losses, IEnumerable<TAccount> friends, AccountStatus accountStatus, int money, int levelPoints, int seasonalPoints, Characteristics characteristics, int characteristicsPoints, Inventory inventory, ProductPlace productPlace, DateTime dateOfMembership, int clanPoints) : base(nickname, level, victories, losses)
        {
            Friends = new HashSet<TAccount>(friends ?? throw new ArgumentNullException(nameof(friends)));
            AccountStatus = accountStatus;
            Money = money;
            LevelPoints = levelPoints;
            SeasonalPoints = seasonalPoints;
            Characteristics = characteristics ?? throw new ArgumentNullException(nameof(characteristics));
            CharacteristicsPoints = characteristicsPoints;
            Inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            ProductPlace = productPlace ?? throw new ArgumentNullException(nameof(productPlace));
            DateOfMembership = dateOfMembership;
            ClanPoints = clanPoints;
        }

        public virtual ICollection<TAccount> Friends { get => _friends; set => _friends = value; }
        public AccountStatus AccountStatus { get => _accountStatus; set => _accountStatus = value; }
        public int Money { get => _money; set => _money = value; }
        public int LevelPoints { get => _levelPoints; set => _levelPoints = value; }
        public int SeasonalPoints { get => _seasonalPoints; set => _seasonalPoints = value; }
        public virtual Characteristics Characteristics { get => _characteristics; set => _characteristics = value; }
        public int CharacteristicsPoints { get => _characteristicsPoints; set => _characteristicsPoints = value; }
        public virtual Inventory Inventory { get => _inventory; set => _inventory = value; }
        public virtual ProductPlace ProductPlace { get => _productPlace; set => _productPlace = value; }
        public DateTime DateOfMembership { get => _dateOfMembership; set => _dateOfMembership = value; }
        public int ClanPoints { get => _clanPoints; set => _clanPoints = value; }
    }
}
