namespace DMS.Domain.Settings;

public class HangfireSettings
{
    public static string Key = "Hangfire";

    public string Route { get; set; }

    public string ServerName { get; set; }

    public DatabaseSettings Storage { get; set; }

    public Dashboard Dashboard { get; set; }
}

public class DatabaseSettings
{
    public string DBProvider { get; set; }

    public string ConnectionString { get; set; }
}

public class Dashboard
{
    public string AppPath { get; set; }

    public int StatsPollingInterval { get; set; }

    public string DashboardTitle { get; set; }
}

