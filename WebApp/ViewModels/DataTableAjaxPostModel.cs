using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class DataTableAjaxPostModel
    {
        public DataTableAjaxPostModel Init()
        {
            if (Length == 0)
                Length = 10;

            if (Start == 0)
                Start = 1;
            else
                Start = (Start / Length) + 1;

            return this;
        }

        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        //public Search Search { get; set; }
    }
}
