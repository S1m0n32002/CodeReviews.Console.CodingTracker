using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingTracker.S1m0n32002.Models
{
    [Table(TabName)]
    public class Goal
    {
        /// <summary>
        /// Name of the database table
        /// </summary>
        public const string TabName = "goals";

        /// <summary>
        /// The spans of the goal
        /// </summary>
        public enum Spans
        {
            Day,
            Week,
            Month,
            Year,
        }

        /// <summary>
        /// Id of goal
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; } = -1;

        /// <summary>
        /// Description of the goal
        /// </summary>
        public string? Description { get; set; } = "";

        /// <summary>
        /// Duration of the goal
        /// </summary>
        [Required]
        public TimeSpan Duration;

        /// <summary>
        /// Span of the goal
        /// </summary>
        [Required]
        public Spans Span;

        /// <summary>
        /// Flags the goal as completed 
        /// </summary>
        public bool IsCompleted { get; set; } = false;

    }
}
