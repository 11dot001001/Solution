using Noname.BitConversion;
using Noname.BitConversion.System;
using Noname.BitConversion.System.Collections.Generic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Model
{
    public abstract class BacteriumBase
    {
        private readonly Dictionary<int, Path> _roads;
        private BacteriumData _data;

        protected BacteriumBase() { }
        protected BacteriumBase(int bacteriumsCount, BacteriumData bacteriumData)
        {
            _roads = new Dictionary<int, Path>(bacteriumsCount);
            _data = bacteriumData;
        }

        public int Id => _data.Id;
        public Transform Transform => _data._transform;
        public Dictionary<int, Path> Roads => _roads;
        public BacteriumData Data => _data;
    }
}