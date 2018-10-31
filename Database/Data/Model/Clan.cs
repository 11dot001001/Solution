using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using GameCore.Model;
using Noname.BitConversion;

namespace Database.Data.Model
{
    public class Clan : OwnClanBase<Account, Clan>
    {
        private static InheritableVariableLengthBitConverter<Clan> _bitConverter;
        private static InheritableVariableLengthBitConverter<Clan> _ownBitConverter;

        static public InheritableVariableLengthBitConverter<Clan> BitConverter
        {
            get
            {
                if (_bitConverter == null)
                {
                    VariableLengthBitConverterBuilder<Clan, ClanBase> builder = new VariableLengthBitConverterBuilder<Clan, ClanBase>();
                    builder.SetBaseBitConverter(BaseBitConverter);
                    _bitConverter = builder.Finalize();
                }
                return _bitConverter;
            }
        }

        static public InheritableVariableLengthBitConverter<Clan> OwnBitConverter
        {
            get
            {
                if (_ownBitConverter == null)
                {
                    VariableLengthBitConverterBuilder<Clan, OwnClanBase<Account, Clan>> builder = new VariableLengthBitConverterBuilder<Clan, OwnClanBase<Account, Clan>>();
                    builder.SetBaseBitConverter(GetOwnBaseBitConverter(Account.BitConverter));
                    _ownBitConverter = builder.Finalize();
                }
                return _ownBitConverter;
            }
        }

        private const int _maxParticipantsCount = 30;
        public const int MaxNameLength = 30;

        public event EventHandler OnClanDestroy;

        public Clan() => Participants = new HashSet<Account>();
        public Clan(string name, Account leader) : this(name, DateTime.Now, 0, leader) { }
        public Clan(string name, DateTime dateOfCreation, int seasonalPoints, Account leader)
        {
            Participants = new HashSet<Account>();
            Name = name;
            DateOfCreation = dateOfCreation;
            SeasonalPoints = seasonalPoints;
            Leader = leader;
            AddParticipant(leader);
        }

        public bool AddParticipant(Account user)
        {
            if (Participants.Count() >= _maxParticipantsCount)
                return false;
            if (user.Clan != null)
                return false;
            user.ClanPoints = 0;
            user.DateOfMembership = DateTime.Now;
            Participants.Add(user);
            return true;
        }
        public void RemoveParticipant(Account user)
        {
            if (Participants.Count() == 1)
            {
                OnClanDestroy.Invoke(this, new EventArgs());
                return;
            }
            Participants.Remove(user);
            user.ResetClanInfo();

            if (Leader != user)
                return;

            ChangeLeader(Participants.First(x => x.ClanPoints == Participants.Max(y => y.ClanPoints)));
        }
        public bool ChangeLeader(Account newLeader)
        {
            if (Participants.Contains(newLeader))
            {
                Leader = newLeader;
                return true;
            }
            return false;
        }
    }

    public class ClanConfiguration : EntityTypeConfiguration<Clan>
    {
        public ClanConfiguration()
        {
            ToTable(nameof(Clan));
            HasKey(x => x.Id);
            Property(x => x.Name).HasColumnType("varchar").HasMaxLength(Clan.MaxNameLength).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute { IsUnique = true })).IsRequired();
            HasRequired(x => x.Leader);
            Property(x => x.DateOfCreation).IsRequired();
            HasMany(x => x.Participants).WithOptional(x => x.Clan);
        }
    }
}
