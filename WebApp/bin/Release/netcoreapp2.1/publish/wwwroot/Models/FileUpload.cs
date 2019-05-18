using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class FileUpload
    {
        [Required]
        [Display(Name = "Title")]
        [StringLength(60, MinimumLength = 3)]
        public string PlayerImage { get; set; }

        [Required]
        [Display(Name = "Public Schedule")]
        public IFormFile UploadPublicSchedule { get; set; }

        [Required]
        [Display(Name = "Private Schedule")]
        public IFormFile UploadPrivateSchedule { get; set; }
    }
}
