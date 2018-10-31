using Noname.ComponentModel;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Data.Model;
using GameServer.Model;

namespace GameServer.ViewModel
{
    public class ClientViewModel : ModelBase
    {
        public ClientViewModel(ClientModel clientModel) => Model = clientModel;
        public ClientModel Model { get; }
    }
}
