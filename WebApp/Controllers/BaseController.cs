using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public Guid GetUserId()
        {
            try
            {
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return new Guid(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetUserRole()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            return role;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<string> GetAllUserRole() =>
            User
                .FindAll(ClaimTypes.Role)
                .Select(i => i.Value)
                .ToList();

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetUserEmail() => User.FindFirstValue(ClaimTypes.Email);
    }
}