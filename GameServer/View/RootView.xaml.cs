using GameServer.Model;
using GameServer.ViewModel;
using Noname.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameServer.View
{
    /// <summary>
    /// Interaction logic for RootView.xaml
    /// </summary>
    public partial class RootView : Window
    {
        public ObservableCollection<ClientView> ClientCollection { get; } = new ObservableCollection<ClientView>();
        public Command UpdateClientsCommmand { get; }

        public RootView()
        {
            InitializeComponent();
            DataContext = new RootViewModel();
        }

        protected override void OnClosed(EventArgs e)
        {
            ((RootViewModel)DataContext).Model.Stop();
        }
    }
}