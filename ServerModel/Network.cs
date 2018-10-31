using Database.Data.Model;
using GameCore;
using GameCore.Enums;
using GameCore.Model;
using ServerModel.GameMechanics;
using System.Collections.Generic;

namespace ServerModel
{
    public static class Network
    {
        private static Server _server;

        public static List<Client> AuthorizedClients => _server.AuthorizedClients;

        public static void EmailExistsResponse(bool response, Client client) => _server.TCPCall(_server.EmailExistsResponse, response, client);
        public static void NicknameExistsResponse(bool response, Client client) => _server.TCPCall(_server.NicknameExistsResponse, response, client);
        public static void SignUpResponse(SignUpResultCode result, Account account, Client client) => _server.TCPCall(_server.SignUpResponse, (byte)result, account, client);
        public static void LogInResponse(Account account, Client client) => _server.TCPCall(_server.LogInResponse, account, client);
        public static void InvokeTheError(MessageCode message, Client client) => _server.TCPCall(_server.InvokeTheError, (byte)message, client);
        public static void ReceiveOtherAccount(Account account, Client client) => _server.TCPCall(_server.ReceiveOtherAccount, account, client);
        public static void SendGameSettings(IEnumerable<Player> players, Map map)
        {
            foreach (Player player in players)
            {
                List<Bacterium> data = new List<Bacterium>(map.Bacteriums);
                BacteriumData[] buffer = new BacteriumData[data.Count];
                int offset = 0;
                foreach (Bacterium item in data)
                    buffer[offset++] = player.Bacteriums.Contains(item) ? new BacteriumData(OwnerType.My, item.Transform, item.VirusCount) : new BacteriumData(OwnerType.None, item.Transform, item.VirusCount);
                _server.TCPCall(_server.SendGameSettings, new GameSettings(buffer), player.Client);
            }
        }
        public static void StartGame(IEnumerable<Client> clients) => _server.TCPCall(_server.StartGame, clients);
        public static void SendVirus(IEnumerable<Client> clients, IEnumerable<int> bacteriums, int target, int count) => _server.TCPCall(_server.SendViruses, bacteriums, target, count, clients);
        public static void SendVirusPosition(Virus virus, IEnumerable<Client> clients)
        {
            //    foreach (Client client in clients)
            //    {
            //        OwnerType owner = virus.Owner == null ? OwnerType.None : virus.Owner.Equals(client) ? OwnerType.My : OwnerType.Enemy;
            //        _server.TCPCall(_server.SendVirusPosition, virus, (byte)owner, client);
            //    }
        }
        public static void Initialize(int port, string nameOrConnectionString) => _server = new Server(port, nameOrConnectionString, "");
        public static void Start() => _server.Start();
        public static void Stop() => _server.Stop();
    }
}