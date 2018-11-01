using GameCore.Tools;
using Noname.BitConversion;
using Noname.BitConversion.System.Collections.Generic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Model
{
    public abstract class BacteriumBase
    {
        static protected readonly InheritableVariableLengthBitConverter<BacteriumBase> BaseBitConverter;

        static BacteriumBase()
        {
            AbstractVariableLengthBitConverterBuilder<BacteriumBase> builder = new AbstractVariableLengthBitConverterBuilder<BacteriumBase>();
            builder.AddField(a => a._roads, (a, roads) => a._roads = roads, ArrayReliableBitConverter.GetInstance(ListReliableBitConverter.GetInstance(ReliableBitConverter.GetInstance(Road.BitConverter))));
            builder.AddField(a => a._bacteriumData, (a, bacteriumData) => a._bacteriumData = bacteriumData, BacteriumData.BitConverter.Instance);
            BaseBitConverter = builder.Finalize();
        }

        private List<Road>[] _roads;
        private BacteriumData _bacteriumData;

        protected BacteriumBase() { }
        protected BacteriumBase(int bacteriumsCount, BacteriumData bacteriumData)
        {
            _roads = new List<Road>[bacteriumsCount];
            _bacteriumData = bacteriumData;
        }

        public Vector2 Position { get => _bacteriumData._transform.Position; set { _bacteriumData._transform.Position = value; PositionChanged?.Invoke(this, EventArgs.Empty); } }
        public int VirusCount { get => _bacteriumData._virusCount; set { _bacteriumData._virusCount = value; RadiusChanged?.Invoke(this, EventArgs.Empty); } }
        public float TransportRadius => _bacteriumData._transform.TransportRadius;
        public float BacteriumRadius => _bacteriumData._transform.BacteriumRadius;
        public Transform Transform => _bacteriumData._transform;
        public List<Road>[] Roads => _roads;
        public BacteriumData BacteriumData => _bacteriumData;

        public event EventHandler PositionChanged;
        public event EventHandler RadiusChanged;
    }
}