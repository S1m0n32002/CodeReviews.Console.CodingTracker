using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CodingTracker.S1m0n32002.Controllers.DbController;

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
        public TimeSpan Duration => (End ?? DateTime.Now) - Start;

        /// <summary>
        /// Returns <see langword="true"/> if the session is ongoing
        /// </summary>
        [NotMapped]
        public bool IsOngoing => (End == null);

        /// <summary>
        /// Creates a new session in the database
        /// </summary>
        /// <param name="description"> Description of the session </param>
        /// <param name="start"> Starting time of the session </param>
        /// <param name="end"> Ending time of the session </param>
        /// <returns> The session created </returns>
        public Session(string description, DateTime start, DateTime? end = null)
        {
            Description = description;
            Start = start; 
            End = end;
        }

        public Session() { }

    }
}
