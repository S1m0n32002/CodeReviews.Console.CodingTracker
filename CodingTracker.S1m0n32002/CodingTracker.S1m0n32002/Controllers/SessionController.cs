using CodingTracker.S1m0n32002.Models;
using Spectre.Console;
using System.Globalization;
using static CodingTracker.S1m0n32002.Controllers.MenuContoller;

namespace CodingTracker.S1m0n32002.Controllers
{
    internal class SessionController
    {
        /// <summary>
        /// Show all sessions
        /// </summary>
        public static void EditSessions()
        {
            while (true)
            {
                var session = ChooseSession();

                if (session == null)
                    return;

                if (session.Id < 0)
                {
                    session = EditSession(session);

                    DbController.SaveSession(session);

                    continue;
                }

                switch (ChooseAction())
                {
                    case Actions.Edit:
                        DbController.SaveSession(EditSession(session));
                        break;
                    case Actions.Delete:
                        DeleteSession(session);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Prompt user to choose a session
        /// </summary>
        /// <returns> The chosen session or null if user chose to exit</returns>
        static Session? ChooseSession()
        {
            AnsiConsole.Clear();

            PrintRule("Choose session"); // idk why but this doesn't remain on screen the first time it's called

            Dictionary<string, Session?> menuStrs = [];

            int c = 0;
            foreach (Session session in DbController.LoadAllSessions())
            {
                menuStrs.Add($"{c++}.\t{session.Description}", session);
            }

            menuStrs.Add("[green]Add[/]", new Session());
            menuStrs.Add("[yellow]Exit[/]", null);

            var prompt = new SelectionPrompt<string>()
                .AddChoices(menuStrs.Keys)
                .PageSize(Console.BufferHeight - 4);

            var chosenSessionName = AnsiConsole.Prompt(prompt);

            if (menuStrs.TryGetValue(chosenSessionName, out Session? chosenSession))
                return chosenSession;

            return null;
        }

        /// <summary>
        /// Allows user to edit a session
        /// </summary>
        static Session EditSession(Session session)
        {
            AnsiConsole.Clear();

            PrintRule("Edit session");

            var namePrompt = new TextPrompt<string>("Enter session description:")
            {
                ShowDefaultValue = true,
                AllowEmpty = false,
            };
            namePrompt.DefaultValue(!string.IsNullOrWhiteSpace(session.Description) ? session.Description : "I've done this, that and also that...");

            session.Description = AnsiConsole.Prompt(namePrompt);

            AnsiConsole.WriteLine("Choose start date:");
            session.Start = ChooseTime(session.Start) ?? DateTime.Now;

            AnsiConsole.WriteLine("Choose end date or leave empty if still ongoing:");
            session.End = ChooseTime(session.End, session.Start);

            return session;

            static DateTime? ChooseTime(DateTime? defaultTime, DateTime? startTime = null)
            {
                bool allowEmpty = defaultTime == null;

                var prompt = new TextPrompt<DateTime>($"Enter date [yellow](Format: {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern} {CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern})[/]")
                {
                    ShowDefaultValue = true,
                    AllowEmpty = allowEmpty,
                    Converter = (x) => x.ToString(),
                    Culture = CultureInfo.CurrentCulture,
                };

                if (defaultTime == new DateTime())
                    defaultTime = DateTime.Now;

                if (defaultTime != null)
                    prompt.DefaultValue((DateTime)defaultTime);

                DateTime? time;

                while (true)
                {
                    time = AnsiConsole.Prompt(prompt);

                    if (time != new DateTime())
                    {
                        if (startTime != null)
                        {
                            if (time > startTime) break;
                            AnsiConsole.MarkupInterpolated($"This time must be before {startTime:G}{Environment.NewLine}");
                        }
                        else
                            break;
                    }
                    else
                    {
                        time = null;
                        break;
                    }
                }

                return time;
            }

        }

        /// <summary>
        /// Allows user to delete a session
        /// </summary>
        static Session DeleteSession(Session session)
        {
            AnsiConsole.Clear();

            PrintRule("Edit Session");

            var prompt = new SelectionPrompt<MyConfirmation>()
            {
                Title = $"Are you sure you want to delete \"{session.Description}\"?"
            }
            .AddChoices(Enum.GetValues<MyConfirmation>());

            var answer = AnsiConsole.Prompt(prompt);

            if (answer == MyConfirmation.Yes)
            {
                DbController.DeleteSession(session);
            }

            return session;
        }
    }
}
