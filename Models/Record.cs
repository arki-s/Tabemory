namespace Tabemory.Models
{
    public class Record
    {
        public int RecordId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public int Capacity { get; set; }
        public string? Station_name { get; set; }
        public string? Genre_catch { get; set; }
        public string? Genre_name { get; set; }
        public string? Open { get; set; }
        public string? Close { get; set; }
        public string? Photo {  get; set; }
        public ICollection<Review> Reviews { get; set; }
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }


    }
}
