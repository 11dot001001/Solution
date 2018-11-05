using Database.Data;
using Database.Data.Model;
using GameCore;
using GameCore.Model;
using Noname.BitConversion;
using Noname.BitConversion.System;
using Noname.BitConversion.System.Collections.Generic;
using Noname.Net.RPC;
using ServerModel.GameMechanics;
using System.Collections.Generic;
using System.Linq;

namespace ServerModel
{
    public class Server : RPCServer<Client>
    {
        private readonly DataContext _database;

        public RemoteProcedure<bool> EmailExistsResponse { get; set; }
        public RemoteProcedure<bool> NicknameExistsResponse { get; set; }
        public RemoteProcedure<byte, Account> SignUpResponse { get; set; }
        public RemoteProcedure<Account> LogInResponse { get; set; }
        public RemoteProcedure<byte> InvokeTheError { get; set; }
        public RemoteProcedure<Account> ReceiveOtherAccount { get; set; }
        public RemoteProcedure<GameSettings> SendGameSettings { get; set; }
        public RemoteProcedure StartGame { get; set; }
        public RemoteProcedure<IEnumerable<int>, int, int> SendViruses { get; private set; }
        public RemoteProcedure<VirusGroup> SendVirusGroup { get; private set; }

        public List<Client> AuthorizedClients { get; set; } = new List<Client>();
        public Dictionary<int, Client> FindGameClients { get; set; } = new Dictionary<int, Client>();


        public Server(int port, string nameOrConnectionString, string mapManagerDirectory) : base(port)
        {
            _database = new DataContext(nameOrConnectionString);
            MapManager.Initialize(mapManagerDirectory);
        }

        private void TryToCreateGame()
        {
            if (FindGameClients.Count < 2)
                return;
            Client client1 = FindGameClients.First().Value;
            FindGameClients.Remove(client1.AccountInfo.Id);
            Client client2 = FindGameClients.First().Value;
            FindGameClients.Remove(client2.AccountInfo.Id);
            GameSession game = new GameSession(new Client[] { client1, client2 }, GameMode.OneByOne);
        }

        protected override void InitializeLocalProcedures()
        {
            ReliableBitConverter<IEnumerable<int>> iEnumerableBacteriumId = ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(Int32BitConverter.Instance));

            DefineLocalProcedure(true, SignUp, StringBitConverter.UnicodeReliableInstance, StringBitConverter.UnicodeReliableInstance, StringBitConverter.UnicodeReliableInstance);
            DefineLocalProcedure(true, LogIn, StringBitConverter.UnicodeReliableInstance, StringBitConverter.UnicodeReliableInstance);
            DefineLocalProcedure(true, IsEmailExists, StringBitConverter.UnicodeReliableInstance);
            DefineLocalProcedure(true, IsNicknameExists, StringBitConverter.UnicodeReliableInstance);
            DefineLocalProcedure(true, GetOtherAccount, Int32BitConverter.Instance);
            DefineLocalProcedure(true, FindGame);
            DefineLocalProcedure(true, RequestSendViruses, iEnumerableBacteriumId, Int32BitConverter.Instance);
        }
        protected override void InitializeRemoteProcedures()
        {
            ReliableBitConverter<Account> accountNullableConverter = ReliableBitConverter.GetInstance(NullableBitConverter.GetInstance(Account.OwnBitConverter));
            ReliableBitConverter<Account> otherAccountNullableConverter = ReliableBitConverter.GetInstance(NullableBitConverter.GetInstance(Account.BitConverter));
            ReliableBitConverter<IEnumerable<BacteriumData>> iEnumerableBacteriumConverter = ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(BacteriumData.BitConverter.Instance));
            ReliableBitConverter<IEnumerable<int>> iEnumerableBacteriumId = ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(Int32BitConverter.Instance));

            EmailExistsResponse = DefineRemoteProcedure(BooleanBitConverter.Instance);
            NicknameExistsResponse = DefineRemoteProcedure(BooleanBitConverter.Instance);
            SignUpResponse = DefineRemoteProcedure(ByteBitConverter.Instance, accountNullableConverter);
            LogInResponse = DefineRemoteProcedure(accountNullableConverter);
            InvokeTheError = DefineRemoteProcedure(ByteBitConverter.Instance);
            ReceiveOtherAccount = DefineRemoteProcedure(otherAccountNullableConverter);
            SendGameSettings = DefineRemoteProcedure(ReliableBitConverter.GetInstance(GameSettings.BitConverter));
            StartGame = DefineRemoteProcedure();
            SendViruses = DefineRemoteProcedure(iEnumerableBacteriumId, Int32BitConverter.Instance, Int32BitConverter.Instance);
            SendVirusGroup = DefineRemoteProcedure(ReliableBitConverter.GetInstance(VirusGroup.NetworkBitConverter));
        }

        #region Network
        private void SignUp(Client client, string email, string password, string nickname)
        {
            SignUpResultCode result = _database.CreateAccount(email, password, nickname, out Account account);
            if (result == SignUpResultCode.SignUpSuccessfully)
            {
                client.AccountInfo = account;
                AuthorizedClients.Add(client);
            }
            Network.SignUpResponse(result, account, client);
        }
        private void LogIn(Client client, string email, string password)
        {
            Account account = _database.GetAccount(email, password);
            if (account == null)
            {
                Network.InvokeTheError((byte)MessageCode.LogInError, client);
                return;
            }
            client.AccountInfo = account;
            Network.LogInResponse(account, client);
            AuthorizedClients.Add(client);
        }
        private void IsEmailExists(Client client, string email) => Network.EmailExistsResponse(_database.IsEmailExist(email), client);
        private void IsNicknameExists(Client client, string nickname) => Network.NicknameExistsResponse(_database.IsNicknameExist(nickname), client);
        private void GetOtherAccount(Client client, int id) => Network.ReceiveOtherAccount(_database.GetAccountById(id), client);
        private void FindGame(Client client)
        {
            FindGameClients.Add(client.AccountInfo.Id, client);
            TryToCreateGame();
        }
        private void RequestSendViruses(Client client, IEnumerable<int> bacteriumsFrom, int bacteriumTo) => client.CurrentGame.RequestSendViruses(bacteriumsFrom, bacteriumTo);
        #endregion 
    }
}
