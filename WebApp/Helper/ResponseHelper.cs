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
                return "Data has been Inserted";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

    }
}
