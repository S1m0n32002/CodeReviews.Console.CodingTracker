using Newtonsoft.Json;

namespace CodingTracker.S1m0n32002.Models
{
    /// <summary>
    /// Manages the settings of the application.
    /// </summary>
    internal class Settings
    {
        public string? Db { set; get; }
        public string? Template { set; get; }

        /// <summary>
        /// Returns the current application settings.
        /// </summary>
        public static Settings Current => _Current;

        /// <summary>
        /// Memory to make Current readonly from the outside.
        /// </summary>
        static Settings _Current = new();

        /// <summary>
        /// SaveSession the settings to the file.
        /// </summary>
        /// <param name="filename"> The name of the settings file to save. </param>
        public static void Save(string filename = "AppSettings")
        {
            var output = JsonConvert.SerializeObject(Settings._Current, Formatting.Indented);
            File.WriteAllText(Path.Combine("Settings", $"{filename}.json"), output);
        }

        /// <summary>
        /// Load the settings from the file.
        /// </summary>
        /// <param name="filename"> The name of the settings file to load. </param>
        /// <exception cref="InvalidOperationException"> Thrown when a property is missing from the settings file. </exception>
        public static void Load(string filename = "AppSettings")
        {
            var input = File.ReadAllText(Path.Combine("Settings", $"{filename}.json"));
            _Current = JsonConvert.DeserializeObject<Settings>(input, new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                NullValueHandling = NullValueHandling.Include,
            }) ?? new();

            // Checks if all properties are set inside the file.
            var missingProperties = _Current.GetType().GetProperties().Where(x => x.GetValue(Settings._Current) == null);

            if (missingProperties.Any())
            {
                throw new InvalidOperationException($"The following properties are missing from the settings file:\n" +
                                           $"- {string.Join(", \n- ", missingProperties.Select(x => x.Name))}");
            }

        }
    }
}
