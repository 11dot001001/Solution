using GameCore.Model;
using ServerModel.GameMechanics;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace ServerModel.Managers
{
    public class GameManager
    {
        public GameSettings GetGameSettings(GameSession gameSession) => new GameSettings();

        private const float _virusGroupTimerInterval = 100f;
        private const float _bacteriumGrowthTimerInterval = 1000f;
        private readonly Timer _virusGroupTimer;
        private readonly Timer _bacteriumGrowthTimer;
        private readonly List<GameSession> _gameSessions;
        private readonly Dictionary<int, Client> _findGameClients;

        public GameManager()
        {
            _gameSessions = new List<GameSession>();
            _findGameClients = new Dictionary<int, Client>();

            _virusGroupTimer = new Timer(_virusGroupTimerInterval);
            _virusGroupTimer.Elapsed += _virusGroupTimer_Elapsed;
            _virusGroupTimer.Start();

            _bacteriumGrowthTimer = new Timer(_bacteriumGrowthTimerInterval);
            _bacteriumGrowthTimer.Elapsed += _bacteriumGrowthTimer_Elapsed; ;
            _bacteriumGrowthTimer.Start();
        }

        private void TryToCreateGame()
        {
            if (_findGameClients.Count < 2)
                return;
            Client client1 = _findGameClients.First().Value;
            _findGameClients.Remove(client1.AccountInfo.Id);
            Client client2 = _findGameClients.First().Value;
            _findGameClients.Remove(client2.AccountInfo.Id);
            _gameSessions.Add(new GameSession(new Client[] { client1, client2 }, GameMode.OneByOne));
        }
        private void _virusGroupTimer_Elapsed(object sender, ElapsedEventArgs e) => _gameSessions.ForEach(x => x.UpdateVirusGroup());
        private void _bacteriumGrowthTimer_Elapsed(object sender, ElapsedEventArgs e) => _gameSessions.ForEach(x => x.UpdateBacterium());

        public void ClientReadyToFindGame(Client client)
        {
            _findGameClients.Add(client.AccountInfo.Id, client);
            TryToCreateGame();
        }
    }
}
