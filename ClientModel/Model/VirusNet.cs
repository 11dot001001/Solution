using GameCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClientModel.Data
{
    public class VirusNet : VirusBase
    {
        public override int Id { get => _id; set => _id = value; }
        public override Vector2 Position { get => _position; set => _position = value; }
        public override int Value { get => _value; set => _value = value; }
    }
}
