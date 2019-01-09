using GameCore.Tools;
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
        private BacteriumData _bacteriumData;

        protected BacteriumBase() { }
        protected BacteriumBase(int bacteriumsCount, BacteriumData bacteriumData)
        {
            _roads = new Dictionary<int, Path>(bacteriumsCount);
            _bacteriumData = bacteriumData;
        }

        public int Id => _bacteriumData.Id;
        public int VirusCount { get => _bacteriumData._virusCount; set { _bacteriumData._virusCount = value; RadiusChanged?.Invoke(this, EventArgs.Empty); } }
        public float TransportRadius => _bacteriumData._transform.TransportRadius;
        public float BacteriumRadius => _bacteriumData._transform.BacteriumRadius;
        public Transform Transform => _bacteriumData._transform;
        public Dictionary<int, Path> Roads => _roads;
        public BacteriumData BacteriumData => _bacteriumData;

        public event EventHandler PositionChanged;
        public event EventHandler RadiusChanged;
    }
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