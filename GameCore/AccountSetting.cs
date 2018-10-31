using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public static class AccountSetting
    {
        public readonly static int maxEmailLength = 50;
        public readonly static int maxNicknameLength = 50;
        public readonly static int maxFriendsCount = 5;
        public readonly static int minPasswordLength = 6;

        public readonly static int Characteristic_A_Max = 10;
        public readonly static int Characteristic_B_Max = 10;
    }
}
