using GameCore.Enums;
using GameCore.Model;
using GameCore.Tools;
using ILibrary.Maths.Geometry2D;
using System;
using System.Collections.Generic;
using UnityEngine;
using Transform = GameCore.Model.Transform;

namespace ServerModel.GameMechanics
{
    public class Map : ICloneable
    {
        public Map(float[] bacteriumsData) => Conversion(bacteriumsData ?? throw new ArgumentNullException(nameof(bacteriumsData)));
        public Map(BacteriumModel[] bacteriums, RoadManager roadManager, IEnumerable<float> data)
        {
            Bacteriums = bacteriums ?? throw new ArgumentNullException(nameof(bacteriums));
            RoadManager = roadManager ?? throw new ArgumentNullException(nameof(roadManager));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public BacteriumModel[] Bacteriums { get; private set; }
        public RoadManager RoadManager { get; private set; }
        public IEnumerable<float> Data { get; private set; }

        private void Conversion(float[] bacteriumsData)
        {
            Data = bacteriumsData;
            int bacteriumCount = bacteriumsData.Length / 4;
            Bacteriums = new BacteriumModel[bacteriumCount];

            int offset = 0;
            for (int i = 0; i < bacteriumCount; i++)
            {
                int id = i;
                Vector2 areaPosition = new Vector2(bacteriumsData[offset++], bacteriumsData[offset++]);
                float maxBacteriumRadius = bacteriumsData[offset++];
                float minBacteriumRadius = bacteriumsData[offset++];
                Bacteriums[i] = new BacteriumModel(id, OwnerType.None, new Transform(maxBacteriumRadius, minBacteriumRadius, new Circle(areaPosition, maxBacteriumRadius + 0.3f)), 10);
            }
            RoadManager = new RoadManager(Bacteriums);
        }

        public object Clone() => new Map(Bacteriums, RoadManager, Data);
    }
}