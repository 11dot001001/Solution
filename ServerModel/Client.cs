using Database.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noname.Net.RPC;
using ServerModel.GameMechanics;

namespace ServerModel
{
    public class Client : RPCClientConnectionProvider
    {
        public Account AccountInfo { get; set; }
        public GameSession CurrentGame { get; set; }

        public Client() { }
    }
}
