using GameCore.Tools;
using System;
using UnityEngine;

namespace ServerModel.GameMechanics
{
    public class Map
    {
        private Bacterium[] _bacteriums;

        public Map(float[] bacteriumsData) => Conversion(bacteriumsData ?? throw new ArgumentNullException(nameof(bacteriumsData)));
        public Map(Map map, EventHandler positionChanged, EventHandler radiusChanged)
        {
            _bacteriums = map.Bacteriums ?? throw new ArgumentNullException(nameof(map));
            foreach (Bacterium item in _bacteriums)
            {
                item.PositionChanged += positionChanged;
                item.RadiusChanged += radiusChanged;
            }
        }

        public Bacterium[] Bacteriums => _bacteriums;

        private void Conversion(float[] bacteriumsData)
        {
            int bacteriumCount = bacteriumsData.Length / 4;
            _bacteriums = new Bacterium[bacteriumCount];

            int offset = 0;
            for (int i = 0; i < bacteriumCount; i++)
            {
                Bacterium bacterium = new Bacterium(bacteriumCount, new Vector2(bacteriumsData[offset++], bacteriumsData[offset++]), bacteriumsData[offset++], bacteriumsData[offset++]);
                _bacteriums[i] = bacterium;
            }

            for (int i = 0; i < _bacteriums.Length; i++)
                for (int j = 0; j < _bacteriums.Length; j++)
                    if (j != i)
                        _bacteriums[i].Roads[j] = new RoadManager(_bacteriums[i], _bacteriums[j], _bacteriums);
        }
    }
}