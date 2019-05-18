using Microsoft.AspNetCore.Identity;

namespace WebApp.ViewModels
{
    public class MostCenturiesdto
    {        
        public int TotalMatch { get; set; }
        public int TotalInnings { get; set; }
        public long NumberOf100s { get; set; }
        public string PlayerName { get; set; }
        public string Image { get; set; }
    }
}
