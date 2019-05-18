using Microsoft.AspNetCore.Identity;

namespace WebApp.ViewModels
{
    public class MostRunsdto
    {        
        public int TotalMatch { get; set; }
        public int TotalInnings { get; set; }
        public long TotalBatRuns { get; set; }
        public string PlayerName { get; set; }
        public string Image { get; set; }
    }
}
