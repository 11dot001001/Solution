using UnityEngine;

namespace GameCore.Model
{
    public abstract class VirusBase
    {
        protected int _id;
        protected Vector2 _position;
        protected int _value;

        public abstract int Id { get; set; }
        public abstract Vector2 Position { get; set; }
        public abstract int Value { get; set; }
    }
}
