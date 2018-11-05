using GameCore.Enums;
using GameCore.Model;
using ILibrary.Maths.Geometry2D;
using Noname.BitConversion;
using System;
using UnityEngine;

namespace ServerModel.GameMechanics
{
    public class Bacterium : BacteriumBase
    {
        //private readonly int _growthValue = 1;

        public Bacterium() : base() { }
        public Bacterium(int id, int roadsCount, Vector2 areaPosition, float maxBacteriumRadius, float minBacteriumRadius): base(roadsCount, new BacteriumData(id, OwnerType.None, new GameCore.Model.Transform(maxBacteriumRadius, minBacteriumRadius, new Circle(areaPosition, maxBacteriumRadius + 0.3f)), 10)) { }

        public void GetViruses(Client clientVirus, int count)
        {
            //if (Owner == null)
            //{
            //    Owner = clientVirus;
            //    data.virusCount += count;
            //    return;
            //}

            //if (Owner.Equals(clientVirus))
            //    data.virusCount += count;
            //else
            //{
            //    if (count > data.virusCount)
            //    {
            //        Owner = clientVirus;
            //        data.virusCount = count - data.virusCount;
            //    }
            //    else
            //        data.virusCount -= count;
            //}
        }

        public void Growth(object sender, EventArgs e)
        {
            //VirusCount += growthValue;
            //transportRadius++;
        }
    }
}