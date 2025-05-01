using CodingTracker.S1m0n32002.Models;
using Spectre.Console;

namespace CodingTracker.S1m0n32002.Controllers
{
    internal class ReportController
    {
        /// <summary>
        /// Show the report of all Sessions
        /// </summary>
        public static void ShowReport()
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

            TimeSpan avarageDuration = new();

            foreach (Session session in Sessions)
            {
                var duration = session.Duration;

                avarageDuration += duration;

                grid.AddRow([
                    new Text(session.Id.ToString()).Centered(),
                    new Text(session.Description ?? "").Centered(),
                    new Text(session.Start.ToString("G")).Centered(),
                    new Text(session.End?.ToString("G") ?? "Ongoing").Centered(),
                    new Text($"{Math.Truncate(duration.TotalHours):00}:{duration.Minutes:00}:{duration.Seconds:00}").Centered()
                    ]);
            }

            AnsiConsole.Clear();

            if (Sessions.Any())
            {
                AnsiConsole.Write(grid);
                avarageDuration /= Sessions.Count();
                AnsiConsole.MarkupLine($"Your coding sessions have an avarage duration of [green]{Math.Truncate(avarageDuration.TotalHours):00}:{avarageDuration.Minutes:00}:{avarageDuration.Seconds:00}[/].");
            }
            else
            {
                AnsiConsole.MarkupLine("You don't have any sessions yet. [red]GO DO SOMETHING YOU LAZY BITCH![/]");
            }

            AnsiConsole.MarkupLine("Press [blue]ENTER[/] to exit");
            Console.ReadLine();
        }
    }
}
