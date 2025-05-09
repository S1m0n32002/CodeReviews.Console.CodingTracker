using CodingTracker.S1m0n32002.Models;
using Spectre.Console;
using static CodingTracker.S1m0n32002.Controllers.MenuContoller;
using System.Globalization;

namespace CodingTracker.S1m0n32002.Controllers
{
    internal class GoalController
    {
        /// <summary>
        /// Show all sessions
        /// </summary>
        public static void EditGoals()
        {
            while (true)
            {
                var goal= ChooseGoal();

                if (goal == null)
                    return;

                if (goal.Id < 0)
                {
                    goal = EditGoal(goal);

                    DbController.SaveGoal(goal);

                    continue;
                }

                switch (ChooseAction())
                {
                    case Actions.Edit:
                        DbController.SaveGoal(EditSession(goal));
                        break;
                    case Actions.Delete:
                        DeleteGoal(goal);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Prompt user to choose a goal
        /// </summary>
        /// <returns> The chosen goal or null if user chose to exit </returns>
        static Goal? ChooseGoal()
        {
            AnsiConsole.Clear();

            PrintRule("Choose goal");

            Dictionary<string, Goal?> menuStrs = [];

            int c = 0;
            foreach (Goal goal in DbController.LoadAllGoals())
            {
                menuStrs.Add($"{c++}.\t{goal.Description}", goal);
            }

            menuStrs.Add("[green]Add[/]", new Goal());
            menuStrs.Add("[yellow]Exit[/]", null);

            var prompt = new SelectionPrompt<string>()
                .AddChoices(menuStrs.Keys).PageSize(Console.BufferHeight - 4);

            var chosenGoalName = AnsiConsole.Prompt(prompt);

            if (menuStrs.TryGetValue(chosenGoalName, out Goal? chosenGoal))
                return chosenGoal;

            return null;
        }

        /// <summary>
        /// Allows user to edit a goal
        /// </summary>
        static Goal EditGoal(Goal goal)
        {
            AnsiConsole.Clear();

            PrintRule("Edit goal");

            var descriptionPrompt = new TextPrompt<string>("Enter goal description:")
            {
                ShowDefaultValue = true,
                AllowEmpty = false,
            };
            descriptionPrompt.DefaultValue(!string.IsNullOrWhiteSpace(goal.Description) ? goal.Description : "I'll be doing this and that");

            goal.Description = AnsiConsole.Prompt(descriptionPrompt);

            goal.Duration = ChooseTimeSpan() ?? TimeSpan.Zero;

            goal.Span = ChooseSpan();

            return goal;

            static TimeSpan? ChooseTimeSpan()
            {
                var prompt = new TextPrompt<TimeSpan>($"Enter date [yellow](Format: {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern} {CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern})[/]")
                {
                    ShowDefaultValue = true,
                    Converter = (x) => x.ToString(),
                    Culture = CultureInfo.CurrentCulture,
                };

                prompt.DefaultValue(TimeSpan.Zero);

                return AnsiConsole.Prompt(prompt);
            }

        }

        /// <summary>
        /// Allows user to delete a goal
        /// </summary>
        static Goal DeleteGoal(Goal goal)
        {
            AnsiConsole.Clear();

            PrintRule("Edit goal");

            var prompt = new SelectionPrompt<MyConfirmation>()
            {
                Title = $"Are you sure you want to delete \"{goal.Description}\"?"
            }
            .AddChoices(Enum.GetValues<MyConfirmation>());

            var answer = AnsiConsole.Prompt(prompt);

            if (answer == MyConfirmation.Yes)
            {
                DbController.DeleteGoal(goal);
            }

            return goal;
        }
    }
}
