using Noname.ComponentModel;
using ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Data.Model;

namespace GameServer.Model
{
    public class ClientModel : ModelBase
    {
        public NotifyingProperty<Account> AccountProperty { get; } = new NotifyingProperty<Account>();

        public ClientModel() { }

        public ClientModel(Account accountInfo) => AccountProperty.Value = accountInfo;
    }
}
