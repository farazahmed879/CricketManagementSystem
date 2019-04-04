using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels.RoleManagement
{
    public class Userdto
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public ClubAdmindto ClubAdmin { get; set; }
    }
}
