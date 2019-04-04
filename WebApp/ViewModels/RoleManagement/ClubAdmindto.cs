using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels.RoleManagement
{
    public class ClubAdmindto
    {
        public int ClubAdminId { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string TeamName { get; set; }
    }
}
