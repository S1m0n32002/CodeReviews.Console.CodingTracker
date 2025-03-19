namespace HabitTracker.S1m0n32002.Models
{
    public class Session
    {
        /// <summary>
        /// Name of the database table
        /// </summary>
        public const string TabName = "sessions";

        /// <summary>
        /// Id of Session
        /// </summary>
        public int Id { get; set; } = -1;
        
        /// <summary>
        /// Description of the session
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Starting time of session
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Ending time of session
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Returns the duration of the session
        /// </summary>
        public TimeSpan Duration => End - Start;
    }
}
