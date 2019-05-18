using Microsoft.AspNetCore.Identity;

namespace WebApp.ViewModels
{
    public class MostFoursdto
    {        
        public int TotalMatch { get; set; }
        public int TotalInnings { get; set; }
        public long MostFours { get; set; }
        public string PlayerName { get; set; }
        public string Image { get; set; }
    }
}
