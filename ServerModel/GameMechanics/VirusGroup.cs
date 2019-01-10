using GameCore.Model;
using GameCore.Tools;
using System;

namespace ServerModel.GameMechanics
{
    public class VirusGroup
    {
        private readonly float _speed;
        private readonly Road _road;
        private readonly int _roadId;
        private readonly Player _owner;

        public VirusGroup() { }
        public VirusGroup(float speed, Road road, int roadId, Player owner)
        {
            _speed = speed;
            _road = road ?? throw new ArgumentNullException(nameof(road));
            _roadId = roadId;
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public float TimeToEnd { get; set; }
        public VirusGroupData VirusGroupData => new VirusGroupData(_roadId, _road.Start.Id, _road.Start.Data.VirusCount, _road.End.Id);
    }
}