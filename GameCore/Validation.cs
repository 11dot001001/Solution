using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameCore
{
    public static class Validation
    {
        public static bool IsEmailValid(string email)
        {
            //regular expression pattern for valid email
            //addresses, allows for the following domains:
            //com,edu,info,gov,int,mil,net,org,biz,name,museum,coop,aero,pro,tv
            string pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
    (com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";

            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            bool valid = false;

            if (string.IsNullOrEmpty(email))
                valid = false;
            else
                valid = check.IsMatch(email);

            return valid;
        }

        public static bool IsPasswordValid(string password)
        {
            if (password.Length < AccountSetting.minPasswordLength)
                return false;
            return true;
        }

        public static bool IsNicknameValid(string nickname)
        {//длина меньше значения = 8 байт
            return true;
        }
    }
    
}
