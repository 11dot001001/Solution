using GameCore.Enums;
using GameCore.Model;
using GameCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ServerModel.GameMechanics
{
    public sealed class GameSession
    {
        private const float _virusSpeed = 1f;
        private readonly Map _map;
        private readonly GameMode _gameMode;
        private readonly Dictionary<Client, Player> _players;
        private readonly Random _random = new Random();
        private readonly List<VirusGroup> _virusGroups;

        public GameSession(IEnumerable<Client> clients, GameMode gameMode)
        {
            _virusGroups = new List<VirusGroup>();
            _players = new Dictionary<Client, Player>();
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
        }
        private void InitializePlayerBacteriums()
        {
            int bacteriumNumber = 0;
            foreach (Player player in _players.Values)
                player.Bacteriums.Add(_map.Bacteriums[bacteriumNumber++]);
        }
        private void SendVirusGroup(VirusGroup virusGroup, int newVirusCount)
        {
            foreach (Player player in _players.Values)
            {
                player.PlayerRelations.TryGetValue(player, out OwnerType ownerType);
                Network.SendVirusGroup(player.Client, new VirusGroupData(virusGroup.VirusGroupData, ownerType), newVirusCount);
            }
        }
        private void VirusGroupArrived(VirusGroup virusGroup)
        {
            Bacterium bacterium = _map.Bacteriums[virusGroup.EndBacterium.Id];
            bacterium.VirusCount += virusGroup.VirusCount;
            foreach (Player player in _players.Values)
                Network.SendVirusGroupArrived(player.Client, bacterium.Id, bacterium.VirusCount);
        }

        public void UpdateVirusGroup()
        {
            lock (_virusGroups)
            {
                for (int i = _virusGroups.Count - 1; i >= 0; i--)
                {
                    VirusGroup virusGroup = _virusGroups[i];
                    if (DateTime.Now - virusGroup.Start < virusGroup.DrivingTime)
                        continue;
                    VirusGroupArrived(virusGroup);
                    _virusGroups.RemoveAt(i);
                }
            }
        }
        public void UpdateBacterium()
        {
            for (int i = 0; i < _map.Bacteriums.Length; i++)
                _map.Bacteriums[i].VirusCount++;
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
                bacterium.VirusCount /= 2;
                VirusGroup virusGroup;
                _virusGroups.Add(virusGroup = new VirusGroup(_virusSpeed, road, roadNumber, player, bacterium.VirusCount));
                SendVirusGroup(virusGroup, bacterium.VirusCount);
            }
        }
    }
}