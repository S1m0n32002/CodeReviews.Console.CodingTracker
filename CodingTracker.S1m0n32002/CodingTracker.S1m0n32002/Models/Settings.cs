using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.S1m0n32002.Models
{
    class Settings
    {
        /// <summary>
        /// The name with extension of the database file.
        /// </summary>
        public string? DbName { get; set; }
        /// <summary>
        /// Path to the Database file.
        /// </summary>
        public string? DbPath { get; set; }
    }
}
