using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Database.Data.Model;
using Database.Data.Model.Tools;
using GameCore;
using GameCore.Model;

namespace Database.Data
{
    public class DataContext : DbContext
    {
        private readonly DbSet<Account> _accounts = null;
        private readonly DbSet<Clan> _clans = null;
        private List<Product> _products = null;

        public DataContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            ProductInitialization();
            _accounts = Set<Account>();
            _clans = Set<Clan>();
        }

        protected override void OnModelCreating(DbModelBuilder modelGetInstanceer)
        {
            modelGetInstanceer.Configurations.Add(new AccountConfiguration());
            modelGetInstanceer.Configurations.Add(new ClanConfiguration());
            modelGetInstanceer.Configurations.Add(new CharacteristicsConfiguration());
            modelGetInstanceer.Configurations.Add(new InventoryConfiguration());
            modelGetInstanceer.Configurations.Add(new ProductPlaceConfiguration());
            modelGetInstanceer.Ignore<ClanBase>();
            modelGetInstanceer.Ignore<OwnClanBase<Account, Clan>>();
            modelGetInstanceer.Ignore<AccountBase<Clan>>();
            modelGetInstanceer.Ignore<OwnAccountBase<Clan, Account, Clan>>();
        }

        private void OnClanDestroy(object sender, EventArgs e) => _clans.Remove((Clan)sender);
        private void ProductInitialization() => _products = new List<Product>
            {
                new Product(ConsumerProductType.FreezeBottle, 500),
                new Product(ConsumerProductType.RunBottle, 400)
            };

        public SignUpResultCode CreateAccount(string email, string password, string nickname, out Account account)
        {
            account = null;
            if (!Validation.IsEmailValid(email) || !Validation.IsPasswordValid(password) || !Validation.IsNicknameValid(nickname))
                throw new InvalidOperationException();
            if (GetAccountByEmail(email) != null)
                return SignUpResultCode.SignUpEmailExists;
            if (GetAccountByNickname(nickname) != null)
                return SignUpResultCode.SignUpNicknameExists;

            _accounts.Add(account = new Account(email, AccountTools.GetDataPassword(password), nickname));
            SaveChanges();
            return SignUpResultCode.SignUpSuccessfully;
        }
        public Account GetAccountByEmail(string email) => _accounts.AsNoTracking().FirstOrDefault(x => x.Email == email);
        public Account GetAccountById(int id) => _accounts.AsNoTracking().FirstOrDefault(x => x.Id == id);
        public Account GetAccountByNickname(string nickname) => _accounts.AsNoTracking().FirstOrDefault(x => x.Nickname == nickname);
        public Account GetAccount(string email, string password)
        {
            Account account = _accounts.SingleOrDefault(x => x.Email.Equals(email));
            return account == null || !AccountTools.IsPasswordVerification(account, password) ? null : account;
        }

        public Clan CreateClan(string clanName, Account leader)
        {
            Clan clan = new Clan(clanName, leader);
            clan.OnClanDestroy += OnClanDestroy;
            return _clans.Add(clan);
        }
        public Clan GetClanByName(string clanName) => _clans.FirstOrDefault(x => x.Name == clanName);

        public bool IsEmailExist(string email) => _accounts.Count(x => x.Email == email) == 0 ? false : true;
        public bool IsNicknameExist(string nickname) => _accounts.Count(x => x.Nickname == nickname) == 0 ? false : true;
    }
}