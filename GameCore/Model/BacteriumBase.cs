using GameCore.Enums;
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

        protected BacteriumBase() { }
        protected BacteriumBase(int bacteriumsCount, int id, OwnerType owner, Transform transform, int virusCount)
        {
            _roads = new Dictionary<int, Path>(bacteriumsCount);
            Id = id;
            Owner = owner;
            Transform = transform;
            VirusCount = virusCount;
        }

        public int Id { get; set; }
        public OwnerType Owner { get; set; }
        public Transform Transform { get; set; }
        public int VirusCount { get; set; }
        public Dictionary<int, Path> Roads => _roads;
        public BacteriumData Data => new BacteriumData(Id, Owner, Transform, VirusCount);
    }
}