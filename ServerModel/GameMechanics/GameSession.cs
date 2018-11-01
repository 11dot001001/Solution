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

        private readonly Map _map;
        private readonly GameMode _gameMode;
        private readonly Timer _bacteriumGrowthTimer;
        private readonly Dictionary<Client, Player> _players;
        private readonly List<VirusGroup> _virusGroups;


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
            _map = MapManager.GetMap(this, gameMode, clients.Count());
            SendSettings();
        }

        public void BacteriumPositionChanged(object sender, EventArgs e)
        {
            Bacterium bacterium = (Bacterium)sender;
        }
        public void BacteriumRadiusChanged(object sender, EventArgs e)
        {
            Bacterium bacterium = (Bacterium)sender;
        }
        private void SendSettings()  => Network.SendGameSettings(_players.Values, _map);
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

        public void RequestSendViruses(IEnumerable<int> bacteriumsFrom, int bacteriumTo)
        {
            List<int> bacteriumFrom = bacteriumsFrom.ToList();
            bacteriumFrom.Remove(bacteriumTo);
            //Network.SendVirus(_players.Keys, bacteriumFrom, bacteriumTo, 100);

            foreach (int bacteriumId in bacteriumFrom)
            {
                Road road = _map.Bacteriums[bacteriumId].Roads[bacteriumTo].First();
                Network.SendVirusGroup(_players.Keys, new VirusGroup(_map.Bacteriums[bacteriumId], _map.Bacteriums[bacteriumTo], 100, road, 2f));
            }
        }
    }
}