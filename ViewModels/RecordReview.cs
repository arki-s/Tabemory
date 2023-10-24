using Tabemory.Models;

namespace Tabemory.ViewModels
{
    public class RecordReview
    {
        public Record Record { get; set; }
        public List<Review> ReviewList { get; set; }
        public Review Review { get; set; }
    }
}
