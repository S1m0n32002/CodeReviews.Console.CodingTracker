using CodingTracker.S1m0n32002.Models;
using Dapper;
using Spectre.Console;
using System.Data.SQLite;

namespace CodingTracker.S1m0n32002.Controllers;

public static class DbController
{
    private static readonly Lock _lock = new();

    /// <summary>
    /// Delete a session from the database
    /// </summary>
    /// <param name="session"> session to delete </param>
    public static void DeleteSession(Session session)
    {
        CheckAndInitDB();

        string StrCmd = @$"DELETE FROM [{Session.TabName}] WHERE {nameof(Session.Id)} = @{nameof(Session.Id)}";

        using var connection = Connect();

        connection.Execute(StrCmd, new { session.Id });
    }

    /// <summary>
    /// Load all sessions from database
    /// </summary>
    public static IEnumerable<Session> LoadAllSessions()
    {
        CheckAndInitDB();

        string StrCmd = $"SELECT * FROM [{Session.TabName}]";
        using var connection = Connect();
        return connection.Query<Session>(StrCmd);
    }

    /// <summary>
    /// Saves the session in the database
    /// </summary>
    /// <param name="session"> Session to save </param>
    /// <returns> The session saved </returns>
    public static Session? SaveSession(this Session session)
    {
        CheckAndInitDB();

        string StrCmd;

        if (session.Id < 0)
            StrCmd = $"INSERT INTO {Session.TabName} ({nameof(Session.Id)}," +
                                                    $"{nameof(Session.Description)}," +
                                                    $"{nameof(Session.Start)}," +
                                                    $"{nameof(Session.End)})" +
                                            $"VALUES (" +
                                                    $"(SELECT IFNULL(MAX({nameof(Session.Id)}), -1) + 1 FROM [{Session.TabName}])," +
                                                    $"@{nameof(Session.Description)}," +
                                                    $"@{nameof(Session.Start)}," +
                                                    $"@{nameof(Session.End)})" +
                                            $"RETURNING *;";
        else
            StrCmd = $"UPDATE [{Session.TabName}] SET {nameof(Session.Description)} = @{nameof(Session.Description)}, " +
                                                    $"{nameof(Session.Start)} = @{nameof(Session.Start)}, " +
                                                    $"{nameof(Session.End)} = @{nameof(Session.End)} " +
                                                 $"WHERE {nameof(Session.Id)} = @{nameof(Session.Id)} " +
                                                 $"RETURNING *;";

        using var connection = Connect();

        return connection.QuerySingle<Session>(StrCmd, new
        {
            session.Id,
            session.Description,
            Start = $"{session.Start:o}",
            End = session.End != null ? $"{session.End:o}" : null
        });
    }

    /// <summary>
    /// Check and initialize database/>
    /// </summary>
    public static void CheckAndInitDB()
    {
        lock (_lock)
        {
            if (System.IO.File.Exists(Settings.Current.Db))
                return;

            AnsiConsole.Status().Start("Checking database...", ctx =>
            {
                AnsiConsole.WriteLine("Database not found!");

                ctx.Spinner(Spinner.Known.Circle); // not working :(
                ctx.Status("Initializing database...");
                AnsiConsole.WriteLine("Loading database template...");

                string Template;
                if (System.IO.File.Exists(Settings.Current.Template))
                    Template = System.IO.File.ReadAllText(Settings.Current.Template);
                else
                    throw new System.IO.FileNotFoundException($"Database initialization failed. Restore \"{Settings.Current.Template}\" to continue");

                AnsiConsole.WriteLine("Database template loaded!");
                ctx.Status("Creating database...");

                using var connection = Connect();

                connection.Execute(Template);

                AnsiConsole.WriteLine("Database created!");
                ctx.Status("Populating database...");

                var start = DateTime.Now;
                var rnd = new Random();

                for (int i = 0; i <= 50; i++)
                {
                    var added = SaveSession(new Session($"DEMO Session {i}", start.AddHours(i), start.AddHours(i + rnd.Next(0, 10))));

                    if (added != null)
                        AnsiConsole.WriteLine($"Added: {added.Description} | Start: {added.Start}");
                }

                SaveSession(new Session($"DEMO Session Occurring", DateTime.Now));

                AnsiConsole.WriteLine("Database ready!");
            });

            Console.Clear();
        }
    }

    /// <summary>
    /// Instantiate connection to database
    /// </summary>
    public static SQLiteConnection Connect()
    {
        return new SQLiteConnection(new SQLiteConnectionStringBuilder
        {
            DataSource = Settings.Current.Db
        }.ConnectionString);
    }
}
