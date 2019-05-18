using Microsoft.AspNetCore.Identity;

namespace WebApp.ViewModels
{
    public class MostSixesdto
    {        
        public int TotalMatch { get; set; }
        public int TotalInnings { get; set; }
        public long MostSixes { get; set; }
        public string PlayerName { get; set; }
        public string Image { get; set; }
    }
}
