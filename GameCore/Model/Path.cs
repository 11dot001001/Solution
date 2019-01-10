using GameCore.Tools;
using System;
using System.Collections.Generic;

namespace GameCore.Model
{
    public class Path
    {
        public BacteriumBase BacteriumBase;
        public List<Road> Roads;

        public Path(BacteriumBase bacteriumBase, List<Road> roads)
        {
            BacteriumBase = bacteriumBase ?? throw new ArgumentNullException(nameof(bacteriumBase));
            Roads = roads ?? throw new ArgumentNullException(nameof(roads));
        }
    }
}