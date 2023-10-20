using System.ComponentModel.DataAnnotations;

namespace Tabemory.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MinLength(5)]
        [MaxLength(280)]
        public string? Comment { get; set; }

        public byte[]? Image { get; set; }

        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}
