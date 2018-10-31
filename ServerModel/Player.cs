using ServerModel.GameMechanics;
using System;
using System.Collections.Generic;

namespace ServerModel
{
    public class Player
    {
        public Client Client { get; private set; }
        public List<Bacterium> Bacteriums { get; private set; }
        public State State { get; set; }

        public Player(Client client) 
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Bacteriums = new List<Bacterium>();
            State = State.Waiting;
        }
    }
    public enum State { Waiting, Ready, Playing }
}