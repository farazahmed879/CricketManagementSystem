using System.ComponentModel.DataAnnotations;

namespace CricketApp.Domain
{
    public class Ground
    {
        [Key]
        public int GroundId { get; set; }
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(25)]
        public string Location  { get; set; }

    }
}
