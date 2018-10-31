using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    /// <summary>
    /// Specifies codes of sigh up results.
    /// </summary>
    public enum SignUpResultCode : byte
    {
        /// <summary>
        /// Specifies code of error occurring when email exists.
        /// </summary>
        SignUpEmailExists,
        /// <summary>
        /// Specifies code of error occurring when nickname exists.
        /// </summary>
        SignUpNicknameExists,
        /// <summary>
        /// Specifies code of message occurring when sign up successfully completed.
        /// </summary>
        SignUpSuccessfully,
    }
}
