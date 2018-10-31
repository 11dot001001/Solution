using System;
using GameCore.Tools;

namespace GameCore.Model
{
    public class VirusGroup
    {
        private readonly BacteriumBase _start;
        private readonly BacteriumBase _target;
        private readonly int _virusCount;
        private readonly Road _road;
        private readonly float _speed;

        public VirusGroup(BacteriumBase start, BacteriumBase target, int virusCount, Road road, float speed)
        {
            _start = start ?? throw new ArgumentNullException(nameof(start));
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _virusCount = virusCount;
            _road = road ?? throw new ArgumentNullException(nameof(road));
            _speed = speed;
        }
    }
}