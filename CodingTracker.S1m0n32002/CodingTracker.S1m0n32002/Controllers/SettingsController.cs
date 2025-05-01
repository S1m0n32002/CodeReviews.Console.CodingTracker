using Spectre.Console;
using static CodingTracker.S1m0n32002.Controllers.MenuContoller;

namespace CodingTracker.S1m0n32002.Controllers
{
    internal class SettingsController
    {
        enum Settings
        {
            Db,
            DbTemplate,
        }

        /// <summary>
        /// Show all sessions
        /// </summary>
        public static void EditSettings()
        {
            while (true)
            {
                var setting = ChooseSetting();

                switch (setting)
                {
                    case Settings.Db:
                        var fileDb = Spectre.Explorer.Explore(Models.Settings.Current.Db);
                        var pathDb = fileDb.FullName;

                        if (pathDb != null)
                        {
                            Models.Settings.Current.Db = pathDb;
                            Models.Settings.Save();
                        }

                        break;
                    case Settings.DbTemplate:
                        var fileTemplate = Spectre.Explorer.Explore(Models.Settings.Current.Template);
                        var pathTemplate = fileTemplate.FullName;

                        if (pathTemplate != null)
                        {
                            Models.Settings.Current.Template = pathTemplate;
                            Models.Settings.Save();
                        }

                        break;
                    default:
                        return;
                }
            }
        }

        /// <summary>
        /// Prompt user to choose a setting to change
        /// </summary>
        /// <returns> The chosen session or null if user chose to exit</returns>
        static Settings? ChooseSetting()
        {
            AnsiConsole.Clear();

            PrintRule("Choose setting");

            Dictionary<string, Settings?> menuStrs = [];

            int c = 0;
            menuStrs.Add($"{c++}.\t Path to db:" + Environment.NewLine + $"\t\t\"{Models.Settings.Current.Db}\"", Settings.Db);
            menuStrs.Add($"{c++}.\t Path to template:" + Environment.NewLine + $"\t\t\"{Models.Settings.Current.Template}\"", Settings.DbTemplate);
            menuStrs.Add("[yellow]Exit[/]", null);

            var prompt = new SelectionPrompt<string>()
                .AddChoices(menuStrs.Keys).PageSize(Console.BufferHeight - 4);

            if (menuStrs.TryGetValue(AnsiConsole.Prompt(prompt), out Settings? chosenSetting))
                return chosenSetting;

            return null;
        }
    }
}
