using Microsoft.AspNetCore.Identity;

namespace WebApp.ViewModels
{
    public class MostFiftiesdto
    {        
        public int TotalMatch { get; set; }
        public int TotalInnings { get; set; }
        public long NumberOf50s { get; set; }
        public string PlayerName { get; set; }
       
    }
}
