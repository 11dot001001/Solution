using ServerModel.GameMechanics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerModel.Managers
{
    public static class MapManager
    {
        private static readonly Dictionary<GameMode, List<Map>> _maps;
        private static readonly Dictionary<GameMode, List<float[]>> _bacteriumData;
        private static string _directory;

        static MapManager()
        {
            _bacteriumData = new Dictionary<GameMode, List<float[]>>();
            _maps = new Dictionary<GameMode, List<Map>>();
        }

        public static void Initialize(string directory)
        {
            _directory = directory;
            //if (!File.Exists(directory))
            //throw new ArgumentException(nameof(directory));
            LoadResources();
        }

        public static void LoadResources()
        {
            float[] data = new float[] { -7.77f, -3.89f, 1.07f, 0.74f, 7.79f, 3.92f, 1.07f, 0.74f, -0.02f, 0f, 2f, 0.63f, -2.18f, -3.51f, 0.74f, 0.63f, -4.21f, 0.12f, 0.74f, 0.63f, -5.83f, -1.37f, 0.74f, 0.63f, 5.04f, 2.42f, 0.74f, 0.63f, -4.59f, -4.03f, 0.74f, 0.63f, 4.45f, 0.16f, 0.68f, 0.63f, 2.64f, 3.62f, 0.74f, 0.63f };
             //{ -7f,-3.4f,1.1f,0.7f,7.2f,3.4f,1.1f,0.7f,4.5f,1.5f,1.3f,0.4f,-3.9f,-1.3f,1.3f,0.6f,-1.3f,1.2f,1.3f,0.6f,1f,3.2f,0.9f,0.6f,1f,-1.9f,1.2f,0.6f,-7f,4.4f,1.4f,0.6f };
            _bacteriumData.Add(GameMode.OneByOne, new List<float[]>() { data });
            _maps.Add(GameMode.OneByOne, new List<Map> { new Map(data) });
        }

        public static Map GetMap(GameSession gameSession, GameMode gameMode, int playersCount)
        {
            if (!_maps.TryGetValue(gameMode, out List<Map> maps))
                throw new Exception();
            return (Map)maps.First().Clone();
        }
    }
}