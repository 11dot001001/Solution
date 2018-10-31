using Database.Data;
using System;

namespace Database
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (DataContext database = new DataContext(@"Data Source=.\Sqlexpress;Initial Catalog=GameDatabase;Integrated Security=True"))
            {
                //Account a = database.GetAccount("account2@mail.ru", "account2");

                //byte[] buffer = Account.OwnBitConverter.GetBytes(a);
                //Account ac = Account.OwnBitConverter.GetInstance(buffer);

                //database.CreateAccount("account2@mail.ru", "account2", "1232", out Account a);
                //database.CreateAccount("account22@mail.ru", "account2", "123422");
                //Account a = database.GetAccount("account2@mail.ru", "account2");
                //Account a2 = database.GetAccount("account22@mail.ru", "account2");
                //database.CreateAccount("1", "1", "1", out Account a3);
                //database.CreateAccount("2", "2", "2", out Account a4);
                //Clan clan = database.GetClanByName("clan");
                database.SaveChanges();

                //byte[] bytes = new byte[OwnAccountBitConverter.Instance.GetByteCount(a)];
                //OwnAccountBitConverter.Instance.GetBytes(a, bytes, 0);
                //Account cla2n = OwnAccountBitConverter.Instance.GetInstance(bytes, 0, 0);
            }
            Console.ReadKey();
        }
    }
}