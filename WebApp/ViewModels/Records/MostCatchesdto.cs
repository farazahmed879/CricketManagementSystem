using Microsoft.AspNetCore.Identity;

namespace WebApp.ViewModels
{
    public class MostCatchesdto
    {        
        public int TotalMatch { get; set; }
        public long MostCatches { get; set; }
        public string PlayerName { get; set; }
       
    }
}
