using GameCore.Model;
using GameCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;

namespace ServerModel.GameMechanics
{
    public class Virus : VirusBase
    {
        private readonly List<WayPosition> _way;
        //private readonly WayPosition _target;
        private readonly Timer _moveTimer;
        private readonly float _speed;

        public override int Id { get => _id; set => _id = value; }
        public override Vector2 Position { get => _position; set { _position = value; PositionChanged?.Invoke(this, EventArgs.Empty); } }
        public override int Value { get => _value; set => _value = value; }
        public Client Owner { get; set; } = null;

        public event EventHandler PositionChanged;

        public Virus() { }
        public Virus(Client owner, IEnumerable<WayPosition> way, int value, float speed)
        {
            Owner = owner;
            _way = new List<WayPosition>(way);
            _value = value;
            _speed = speed;

            _moveTimer = new Timer(speed);
            _moveTimer.Elapsed += Move;
            _moveTimer.Start();
            _position = _way[0].Position;
        }
        private void Move(object sender, ElapsedEventArgs e)
        {
            Vector2 localTarget = _way.First().Position;
            Position = Vector2.MoveTowards(_position, localTarget, 0.3f);

            if(_position == localTarget)
            {
                _way.Remove(_way.First());
                if (_way.Count == 0)
                    Arrived();
            }
        }

        private void Arrived() => _moveTimer.Stop();
    }
}
