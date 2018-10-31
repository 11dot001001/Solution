using GameServer.Model;
using GameServer.ViewModel;
using Noname.Windows.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.View
{
    public class ClientView : ItemView<ClientViewModel>
    {
        public ClientView(ClientViewModel clientViewModel) : base(clientViewModel) {  }
    }
}
