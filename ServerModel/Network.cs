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
                BacteriumData[] buffer = new BacteriumData[map.Bacteriums.Length];
                int offset = 0;
                foreach (Bacterium item in map.Bacteriums)
                    buffer[offset++] = player.Bacteriums.Contains(item) ? new BacteriumData(OwnerType.My, item.Transform, item.VirusCount) : new BacteriumData(OwnerType.None, item.Transform, item.VirusCount);
                _server.TCPCall(_server.SendGameSettings, new GameSettings(buffer), player.Client);
            }
        }
        public static void StartGame(IEnumerable<Client> clients) => _server.TCPCall(_server.StartGame, clients);
        public static void SendVirus(IEnumerable<Client> clients, IEnumerable<int> bacteriums, int target, int count) => _server.TCPCall(_server.SendViruses, bacteriums, target, count, clients);
        public static void SendVirusGroup(IEnumerable<Client> clients, VirusGroup virusGroup) => _server.TCPCall(_server.SendVirusGroup, virusGroup, clients);
        public static void Initialize(int port, string nameOrConnectionString) => _server = new Server(port, nameOrConnectionString, "");
        public static void Start() => _server.Start();
        public static void Stop() => _server.Stop();
    }
}