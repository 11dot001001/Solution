using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Database.Data.Model;
using GameServer.Model;
using Noname.ComponentModel;
using Noname.Windows.MVVM;
using ServerModel;

namespace GameServer.ViewModel
{
    public class RootViewModel : ModelBase
    {
        public ObservableCollection<ClientViewModel> ClientCollection { get; } = new ObservableCollection<ClientViewModel>();
        public RootModel Model { get; }
        public Command UpdateClientsCommmand { get; }

        public RootViewModel()
        {
            Model = new RootModel();
            UpdateClientsCommmand = new Command(OnUpdateClients);
            Model.AuthorizedClients.CollectionChanged += AuthorizedClients_CollectionChanged;
        }

        private void AuthorizedClients_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                ClientCollection.Clear();

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (System.ComponentModel.INotifyPropertyChanged obj in e.NewItems)
                    if (obj is ClientModel client)
                        ClientCollection.Add(new ClientViewModel(client));
        }

        private void OnUpdateClients() => Model.GetOnlineAccounts();
    }
}
