using System.ComponentModel.DataAnnotations.Schema;

namespace Tabemory.Models
{
    public class Record
    {
        public int RecordId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? Url { get; set; }
        public string? Station_name { get; set; }
        public string? Genre_catch { get; set; }
        public string? Genre_name { get; set; }
        public string? Open { get; set; }
        public string? Close { get; set; }
        public string? Photo {  get; set; }
        public ICollection<Review> Reviews { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }


    }
}
