using Microsoft.AspNetCore.Identity;

namespace WebApp.ViewModels
{
    public class MostWicketsdto
    {        
        public int TotalMatch { get; set; }
        public long MostWickets { get; set; }
        public string PlayerName { get; set; }
        public string Image { get; set; }
    }
}
