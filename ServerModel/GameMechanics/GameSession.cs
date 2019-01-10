using GameCore.Enums;
using GameCore.Model;
using GameCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace ServerModel.GameMechanics
{
    public sealed class GameSession
    {
        private const float _bacteriumGrowthPeriod = 1000f;
        private const float _virusSpeed = 1f;

        private readonly Map _map;
        private readonly GameMode _gameMode;
        private readonly Timer _bacteriumGrowthTimer;
        private readonly Dictionary<Client, Player> _players;
        private readonly List<VirusGroup> _virusGroups;
        private readonly Random _random = new Random();

        public GameSession(IEnumerable<Client> clients, GameMode gameMode)
        {
            _bacteriumGrowthTimer = new Timer(_bacteriumGrowthPeriod);
            _players = new Dictionary<Client, Player>();
            _virusGroups = new List<VirusGroup>();
            _gameMode = gameMode;
            foreach (Client client in clients)
            {
                client.CurrentGame = this;
                _players.Add(client, new Player(client));
            }
            foreach (Player player in _players.Values)
                foreach (Player otherPlayer in _players.Values)
                {
                    if (player == otherPlayer)
                        continue;
                    player.PlayerRelations.Add(otherPlayer, OwnerType.Enemy);
                }
            _map = MapManager.GetMap(this, gameMode, clients.Count());
            SendSettings();
        }

        private void SendSettings() => Network.SendGameSettings(_players.Values, _map);
        private void TryToStartGame()
        {
            if (_players.Values.Count(x => x.State == State.Ready) != _players.Count)
                return;
            Network.StartGame(_players.Keys);
            _bacteriumGrowthTimer.Start();
        }
        private void InitializePlayerBacteriums()
        {
            int bacteriumNumber = 0;
            foreach (Player player in _players.Values)
                player.Bacteriums.Add(_map.Bacteriums[bacteriumNumber++]);
        }

        public void RequestSendViruses(Client client, IEnumerable<int> bacteriumsFrom, int bacteriumTo)
        {
            List<int> bacteriumsFromId = bacteriumsFrom.ToList();
            bacteriumsFromId.Remove(bacteriumTo);
            List<Bacterium> bacteriumFrom = new List<Bacterium>(bacteriumsFromId.Count);
            for (int i = 0; i < bacteriumsFromId.Count; i++)
                bacteriumFrom.Add(_map.Bacteriums[bacteriumsFromId[i]]);

            //int removeCount = bacteriumFrom.RemoveAll(x => x.Owner != client);
            //if (removeCount != 0)
            //    MessageBox.Show("Some problem. " + client.IPAddress + ":" + client.Port);

            _players.TryGetValue(client, out Player player);
            foreach (Bacterium bacterium in bacteriumFrom)
            {
                Path path = bacterium.Roads.First(x => x.Key == bacteriumTo).Value;
                int roadNumber = _random.Next(path.Roads.Count);
                Road road = path.Roads[roadNumber];
                VirusGroup virusGroup;
                _virusGroups.Add(virusGroup = new VirusGroup(_virusSpeed, road, roadNumber, player));
                SendVirusGroup(virusGroup);
            }
        }
        public void SendVirusGroup(VirusGroup virusGroup)
        {
            foreach (Player player in _players.Values)
            {
                player.PlayerRelations.TryGetValue(player, out OwnerType ownerType);
                Network.SendVirusGroup(player.Client, new VirusGroupData(virusGroup.VirusGroupData, ownerType));
            }
        }
    }
}