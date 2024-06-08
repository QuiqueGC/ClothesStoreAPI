using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClothesStoreAPI.Utils
{
    public static class ErrorMessageManager
    {
        /// <summary>
        /// Extracts error message from the relevant exception
        /// </summary>
        /// <param name="ex">the exception that occurred</param>
        /// <returns>the exception message</returns>
        public static String GetErrorMessage(SqlException ex)
        {
            String msg;

            switch (ex.Number)
            {
                case 2:
                    msg = "Server not operational";
                    break;
                case 53:
                    msg = "No connection to database server";
                    break;
                case 547:
                    msg = "Record has related data";
                    break;
                case 2601:
                    msg = "Duplicate record";
                    break;
                case 2627:
                    msg = "Duplicate record";
                    break;
                case 4060:
                    msg = "Unable to open database";
                    break;
                case 18456:
                    msg = "Login failed";
                    break;
                default:
                    msg = ex.Number + " : " + ex.Message;
                    break;
            }
            return msg;
        }
    }
}