using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Database.Data.Model;
using Noname.ComponentModel;
using ServerModel;

namespace GameServer.Model
{
    public class RootModel : ModelBase
    {
        public ObservableCollection<ClientModel> AuthorizedClients { get; set; } = new ObservableCollection<ClientModel>();

        public RootModel() 
        {
            Network.Initialize(25000, @"Data Source=.\Sqlexpress;Initial Catalog=GameDatabase;Integrated Security=True");
            Network.Start();
        }

        public void GetOnlineAccounts()
        {
            AuthorizedClients.Clear();

            foreach (Account account in Network.AuthorizedClients.Select(x => x.AccountInfo))
                AuthorizedClients.Add(new ClientModel(account));
        }

        public void Stop() => Network.Stop();
    }
}
