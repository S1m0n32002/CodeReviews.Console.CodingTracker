using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingTracker.S1m0n32002.Models
{
    [Table(TabName)]
    public class Session
    {
        /// <summary>
        /// Name of the database table
        /// </summary>
        public const string TabName = "sessions";

        /// <summary>
        /// Id of Session
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; } = -1;

        /// <summary>
        /// Description of the session
        /// </summary>
        public string? Description { get; set; } = "";

        /// <summary>
        /// Starting time of session
        /// </summary>
        [Required]
        public DateTime Start { get; set; }

        /// <summary>
        /// Ending time of session
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// Returns the duration of the session
        /// </summary>
        /// <remarks> If <see cref="End"/> is <see langword="null"/> it is assumed to be ongoing and return the time from start to now</remarks>
        [NotMapped]
        public TimeSpan Duration
        {
            get
            {
                  return  (End ?? DateTime.Now) - Start;
            }
        }
    
    }
}
