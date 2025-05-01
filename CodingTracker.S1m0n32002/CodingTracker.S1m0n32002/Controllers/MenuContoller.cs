using Spectre.Console;
using static CodingTracker.S1m0n32002.Controllers.SessionController;
using static CodingTracker.S1m0n32002.Controllers.ReportController;
using static CodingTracker.S1m0n32002.Controllers.SettingsController;
using static CodingTracker.S1m0n32002.Controllers.GoalController;

namespace CodingTracker.S1m0n32002.Controllers
{
    internal static class MenuContoller
    {
        public enum MyConfirmation
        {
            No,
            Yes
        }

        enum MainMenuChoices
        {
            Begin,
            SetAGoal,
            ShowReport,
            Settings,
            Exit
        }

        static readonly Dictionary<string, MainMenuChoices> mainMenuStrs = new()
        {
            { "Begin"           , MainMenuChoices.Begin},
            { "Set a Goal"      , MainMenuChoices.SetAGoal},
            { "Show report"     , MainMenuChoices.ShowReport },
            { "Settings"        , MainMenuChoices.Settings },
            { "[yellow]Exit[/]" , MainMenuChoices.Exit }
        };

        public static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();

                PrintRule("Main menu");

                var prompt = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .AddChoices(mainMenuStrs.Keys)
                );

                if (mainMenuStrs.TryGetValue(prompt, out MainMenuChoices result))
                {
                    switch (result)
                    {
                        case MainMenuChoices.Begin:
                            EditSessions();
                            break;
                        case MainMenuChoices.SetAGoal:
                            EditGoals();
                            break;
                        case MainMenuChoices.ShowReport:
                            ShowReport();
                            break;
                        case MainMenuChoices.Settings:
                            EditSettings();
                            break;
                        case MainMenuChoices.Exit:
                            return;
                    }
                }
            }
        }

        public enum Actions
        {
            Edit,
            Delete,
            Exit
        }

        static readonly Dictionary<string, Actions> actionsMenuStrs = new()
        {
            { "Edit"              , Actions.Edit },
            { "Delete"            , Actions.Delete },
            { "[yellow]Exit[/]"   , Actions.Exit   }
        };

        /// <summary>
        /// Promt user to choose an action
        /// </summary>
        public static Actions? ChooseAction()
        {
            AnsiConsole.Clear();

            PrintRule("Choose Action");

            var prompt = new SelectionPrompt<string>();

            prompt.AddChoices(actionsMenuStrs.Keys);

            var chosenAction = AnsiConsole.Prompt(prompt);

            if (actionsMenuStrs.TryGetValue(chosenAction, out Actions action))
                return action;

            return null;
        }

        /// <summary>
        /// Print a rule
        /// </summary>
        /// <param name="Title"> Title of the rule </param>
        public static void PrintRule(string Title)
        {
            Rule rule = new()
            {
                Title = $"[white]{Title}[/]",
                Justification = Justify.Left,
                Style = Style.Parse("yellow"),
                Border = BoxBorder.Heavy
            };
            AnsiConsole.Write(rule);
            AnsiConsole.WriteLine();
        }
    }
}
