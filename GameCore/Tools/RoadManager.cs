using GameCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Tools
{
    public class RoadManager
    {
        private readonly Random _random = new Random();
        private readonly Dictionary<BacteriumModel, Dictionary<BacteriumModel, List<Road>>> _roads;

        public RoadManager(IEnumerable<BacteriumModel> bacteriumModels)
        {
            BacteriumModel[] bacteriums = bacteriumModels.ToArray();
            _roads = new Dictionary<BacteriumModel, Dictionary<BacteriumModel, List<Road>>>(bacteriums.Length);

            for (int i = 0; i < bacteriums.Length; i++)
            {
                Dictionary<BacteriumModel, List<Road>> targetRoads = new Dictionary<BacteriumModel, List<Road>>();
                List<Road> roads = new List<Road>();
                for (int j = 0; j < bacteriums.Length; j++)
                    if (j != i)
                        targetRoads.Add(bacteriums[j], new RoadCreator(bacteriums[i], bacteriums[j], bacteriums).Roads);
                _roads.Add(bacteriums[i], targetRoads);
            }
        }

        public Road GetRoad(BacteriumModel start, BacteriumModel end)
        {
            _roads.TryGetValue(start, out Dictionary<BacteriumModel, List<Road>> targetBacteriums);
            targetBacteriums.TryGetValue(end, out List<Road> targetRoads);
            return targetRoads[_random.Next(targetRoads.Count)];
        }
        public Road GetRoad(BacteriumModel start, BacteriumModel end, int index)
        {
            _roads.TryGetValue(start, out Dictionary<BacteriumModel, List<Road>> targetBacteriums);
            targetBacteriums.TryGetValue(end, out List<Road> targetRoads);
            return targetRoads[index];
        }
    }
}
