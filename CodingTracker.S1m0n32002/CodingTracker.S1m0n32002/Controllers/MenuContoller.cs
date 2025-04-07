using CodingTracker.S1m0n32002.Models;
using Spectre.Console;

namespace CodingTracker.S1m0n32002.Controllers
{
    internal static class MenuContoller
    {
        enum MainMenuChoices
        {
            Begin,
            ShowReport,
            Exit
        }

        readonly static Dictionary<string, MainMenuChoices> mainMenuStrs = new()
        {
            { "Begin"           , MainMenuChoices.Begin},
            { "Show report"     , MainMenuChoices.ShowReport },
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
                            //EditHabits();
                            break;
                        case MainMenuChoices.ShowReport:
                            ShowReport();
                            break;
                        case MainMenuChoices.Exit:
                            return;
                    }
                }
            }
        }
/*
        /// <summary>
        /// Allows user to edit a habit
        /// </summary>
        Habit EditHabit(Habit habit)
        {
            Console.Clear();

            PrintRule("Edit Habit");

            var namePrompt = new TextPrompt<string>("Enter habit name:")
            {
                ShowDefaultValue = true,
                AllowEmpty = false,
            };
            namePrompt.DefaultValue(!string.IsNullOrWhiteSpace(habit.Name) ? habit.Name : "Habit");

            habit.Name = AnsiConsole.Prompt(namePrompt);

            var periodicityPrompt = new SelectionPrompt<Habit.Periodicities>()
            {
                Title = $"Select periodicity [green]({habit.Periodicity})[/]"
            };

            if (habit.Id < 0)
            {
                periodicityPrompt.AddChoices(Enum.GetValues<Habit.Periodicities>()
                                                        .Where(p => p != Habit.Periodicities.None));
            }
            else
            {
                periodicityPrompt.AddChoices(Enum.GetValues<Habit.Periodicities>()
                                                        .OrderBy(p => p == Habit.Periodicities.None ? -1 : 1));
            }

            var periodicity = AnsiConsole.Prompt(periodicityPrompt);

            if (periodicity != Habit.Periodicities.None)
                habit.Periodicity = periodicity;

            return habit;
        }
*/
        /// <summary>
        /// Show the report of all Sessions
        /// </summary>
        static void ShowReport()
        {

            var Sessions = DbController.LoadAllSessions();

            var grid = new Grid()
            .AddColumn().AddColumn().AddColumn().AddColumn().AddColumn();

            // Add header row 
            grid.AddRow([
                new Text(nameof(Session.Id), new Style(Color.Red, Color.Black)).Centered(),
                new Text(nameof(Session.Description), new Style(Color.Green, Color.Black)).Centered(),
                new Text(nameof(Session.Start), new Style(Color.Blue, Color.Black)).Centered(),
                new Text(nameof(Session.End), new Style(Color.Blue, Color.Black)).Centered(),
                new Text(nameof(Session.Duration), new Style(Color.Blue, Color.Black)).Centered()
            ]);

            foreach (Session session in Sessions)
            {
                grid.AddRow([
                    new Text(session.Id.ToString()).Centered(),
                    new Text(session.Description ?? "").Centered(),
                    new Text(session.Start.ToString()).Centered(),
                    new Text(session.End.ToString() ?? "").Centered(),
                    new Text(session.Duration.ToString()).Centered()
                    ]);
            }

            Console.Clear();
            AnsiConsole.Write(grid);
            AnsiConsole.Markup("Press [blue]ENTER[/] to exit");
            Console.ReadLine();
        }
        /*
        /// <summary>
        /// Allows user to edit a habit
        /// </summary>
        void EditSettings(Habit habit)
        {
            Console.Clear();

            PrintRule("Edit Settings");

            var namePrompt = new TextPrompt<string>("Enter habit name:")
            {
                ShowDefaultValue = true,
                AllowEmpty = false,
            };
            namePrompt.DefaultValue(!string.IsNullOrWhiteSpace(habit.Name) ? habit.Name : "Habit");

            habit.Name = AnsiConsole.Prompt(namePrompt);

            var periodicityPrompt = new SelectionPrompt<Habit.Periodicities>()
            {
                Title = $"Select periodicity [green]({habit.Periodicity})[/]"
            };

            if (habit.Id < 0)
            {
                periodicityPrompt.AddChoices(Enum.GetValues<Habit.Periodicities>()
                                                        .Where(p => p != Habit.Periodicities.None));
            }
            else
            {
                periodicityPrompt.AddChoices(Enum.GetValues<Habit.Periodicities>()
                                                        .OrderBy(p => p == Habit.Periodicities.None ? -1 : 1));
            }

            var periodicity = AnsiConsole.Prompt(periodicityPrompt);

            if (periodicity != Habit.Periodicities.None)
                habit.Periodicity = periodicity;

            return habit;
        }
        */
        /// <summary>
        /// Print a rule
        /// </summary>
        /// <param name="Title"> Title of the rule </param>
        static void PrintRule(string Title)
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
