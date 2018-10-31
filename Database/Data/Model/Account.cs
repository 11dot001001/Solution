using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Database.Data.Model.Tools;
using GameCore;
using GameCore.Model;
using Noname.BitConversion;

namespace Database.Data.Model
{
    public class Account : OwnAccountBase<Clan, Account, Clan>
    {
        private static InheritableVariableLengthBitConverter<Account> _bitConverter;
        private static InheritableVariableLengthBitConverter<Account> _ownBitConverter;

        static public InheritableVariableLengthBitConverter<Account> BitConverter
        {
            get
            {
                if (_bitConverter == null)
                {
                    VariableLengthBitConverterBuilder<Account, AccountBase<Clan>> builder = new VariableLengthBitConverterBuilder<Account, AccountBase<Clan>>();
                    builder.SetBaseBitConverter(GetBaseBitConverter(Clan.BitConverter));
                    _bitConverter = builder.Finalize();
                }
                return _bitConverter;
            }
        }

        static public InheritableVariableLengthBitConverter<Account> OwnBitConverter
        {
            get
            {
                if (_ownBitConverter == null)
                {
                    VariableLengthBitConverterBuilder<Account, OwnAccountBase<Clan, Account, Clan>> builder = new VariableLengthBitConverterBuilder<Account, OwnAccountBase<Clan, Account, Clan>>();
                    builder.SetBaseBitConverter(GetOwnBaseBitConverter(Clan.OwnBitConverter, BitConverter, Clan.BitConverter));
                    _ownBitConverter = builder.Finalize();
                }
                return _ownBitConverter;
            }
        }

        private string _email;
        private byte[] _password;
        private DateTime _dateOfRegistration;

        public Account() : base() => Friends = new HashSet<Account>();
        public Account(string email, byte[] password, string nickname) : base(nickname, 0, 0, 0, Enumerable.Empty<Account>(), AccountStatus.Normal, 0, 0, 0, new Characteristics(), 0, new Inventory(), new ProductPlace(), DateTime.MinValue, 0)
        {
            Friends = new HashSet<Account>();
            Email = email;
            Password = password;
            DateOfRegistration = DateTime.Now;
        }

        public string Email { get => _email; set => _email = value; }
        public byte[] Password { get => _password; set => _password = value; }
        public DateTime DateOfRegistration { get => _dateOfRegistration; set => _dateOfRegistration = value; }

        public bool AddFriend(Account account)
        {
            if (Friends.Count() >= AccountSetting.maxFriendsCount)
                return false;
            Friends.Add(account);
            return true;
        }
        public void RemoveFriend(Account account) => Friends.Remove(account);
        public void ResetClanInfo()
        {
            Clan = null;
            ClanPoints = 0;
            DateOfMembership = DateTime.MaxValue;
        }
    }

    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountConfiguration()
        {
            ToTable(nameof(Account));
            HasKey(x => x.Id);
            Property(x => x.Email).HasColumnType("varchar").HasMaxLength(AccountSetting.maxEmailLength).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute { IsUnique = true })).IsRequired();
            Property(x => x.Password).HasColumnType("binary").HasMaxLength(AccountTools.GetPasswordLength()).IsRequired();
            Property(x => x.DateOfRegistration).HasColumnType("date").IsRequired();
            Property(x => x.Nickname).HasColumnType("varchar").HasMaxLength(AccountSetting.maxNicknameLength).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute { IsUnique = true })).IsRequired();
            Property(x => x.AccountStatus).IsRequired();
            Property(x => x.Level).IsRequired();
            Property(x => x.LevelPoints).IsRequired();
            Property(x => x.SeasonalPoints).IsRequired();
            Property(x => x.Victories).IsRequired();
            Property(x => x.Losses).IsRequired();
            Property(x => x.DateOfMembership).HasColumnType("date").IsRequired();
            Property(x => x.ClanPoints).IsRequired();
            Property(x => x.CharacteristicsPoints).IsRequired();
            HasMany(x => x.Friends).WithMany();
        }
    }
}
