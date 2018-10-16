using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Helper
{
    public static class ResponseHelper
    {
        public static string Success()
        {
            try
            {
                return "Record has been inserted";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
        public static string UnSuccess()
        {
            try
            {
                return "Record could not be Inserted";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
        public static string UpdateSuccess()
        {
            try
            {
                return "Record has been updated";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }
        public static string UpdateUnSuccess()
        {
            try
            {
                return "Record could not be updated";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

    }
}
