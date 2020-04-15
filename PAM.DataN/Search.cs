using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAM.Data
{
    public class Search
    {
        [Key]
        public int SearchId { get; set; }
        public string Index { get; set; }
        public string Stock { get; set; }
        public DateTimeOffset DateOfSearch { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
    }
}
