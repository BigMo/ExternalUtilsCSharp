using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.Injection.Injectors
{
    public class RemoteThreadResult
    {
         #region VARIABLES
        /// <summary>
        /// Whether the attempt to inject was successful
        /// </summary>
        public bool Success;
        /// <summary>
        /// Returnvalue of the thread that was executed
        /// </summary>
        public long ReturnValue;
        /// <summary>
        /// Message stating what kind of error occured
        /// </summary>
        public string ErrorMessage;
        #endregion

        #region CONSTRUCTOR
        public RemoteThreadResult(bool success)
        {
            Success = success;
            ErrorMessage = "";
            ReturnValue = 0;
        }
        public RemoteThreadResult(long returnValue)
        {
            Success = true;
            ErrorMessage = "";
            ReturnValue = returnValue;
        }
        public RemoteThreadResult(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
            ReturnValue = 0;
        }
        public RemoteThreadResult(string errorMessage, Exception ex)
        {
            Success = false;
            ErrorMessage = string.Format("{0}: [{1}] {2}", errorMessage, ex.GetType().Name, ex.Message);
            ReturnValue = 0;
        }
        public RemoteThreadResult(bool success, string errorMessage, long returnValue)
        {
            Success = success;
            ErrorMessage = errorMessage;
            ReturnValue = 0;
        }
        #endregion
    }
}
