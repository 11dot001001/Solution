using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    /// <summary>
    /// Specifies codes of error.
    /// </summary>
    public enum MessageCode : byte
    {
        /// <summary>
        /// Specifies code of error occurring when email or password are not valid.
        /// </summary>
        LogInError,
    }
}
