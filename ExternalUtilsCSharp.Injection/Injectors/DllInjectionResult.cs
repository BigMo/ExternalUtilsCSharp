using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.Injection.Injectors
{
    /// <summary>
    /// Holds data about an attempt to inject a DLL
    /// </summary>
    public struct DllInjectionResult
    {
        #region VARIABLES
        /// <summary>
        /// Whether the attempt to inject was successful
        /// </summary>
        public bool Success;
        /// <summary>
        /// Message stating what kind of error occured
        /// </summary>
        public string ErrorMessage;
        #endregion

        #region CONSTRUCTOR
        public DllInjectionResult(bool success = true)
        {
            Success = success;
            ErrorMessage = "";
        }
        public DllInjectionResult(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }
        public DllInjectionResult(string errorMessage, Exception ex)
        {
            Success = false;
            ErrorMessage = string.Format("{0}: [{1}] {2}", errorMessage, ex.GetType().Name, ex.Message);
        }
        public DllInjectionResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
        #endregion
    }
}
