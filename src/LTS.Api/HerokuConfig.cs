using MySqlConnector;

namespace LTS.Api;

/// <summary>
/// Heroku: PORT binding and MySQL URLs from JawsDB / ClearDB (mysql://…).
/// </summary>
internal static class HerokuConfig
{
    internal static void UseHerokuPort(WebApplicationBuilder builder)
    {
        var port = Environment.GetEnvironmentVariable("PORT");
        if (!string.IsNullOrEmpty(port))
            builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
    }

    internal static string? TryMysqlConnectionFromAddonUrl()
    {
        foreach (var key in new[] { "JAWSDB_MARIA_URL", "JAWSDB_URL", "CLEARDB_DATABASE_URL", "DATABASE_URL" })
        {
            var raw = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrWhiteSpace(raw)) continue;

            var normalized = raw.Trim();
            if (normalized.StartsWith("mysql2://", StringComparison.OrdinalIgnoreCase))
                normalized = "mysql://" + normalized["mysql2://".Length..];

            if (!normalized.StartsWith("mysql://", StringComparison.OrdinalIgnoreCase))
                continue;

            try
            {
                return MysqlUrlToConnectionString(normalized);
            }
            catch
            {
                /* try next env key */
            }
        }

        return null;
    }

    internal static bool IsHerokuDyno => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DYNO"));

    private static string MysqlUrlToConnectionString(string mysqlUrl)
    {
        var rest = mysqlUrl["mysql://".Length..];
        var at = rest.LastIndexOf('@');
        if (at < 0) throw new FormatException("mysql URL: missing @");

        var userInfo = rest[..at];
        var hostAndDb = rest[(at + 1)..];

        string user, password;
        var colon = userInfo.IndexOf(':');
        if (colon >= 0)
        {
            user = Uri.UnescapeDataString(userInfo[..colon]);
            password = Uri.UnescapeDataString(userInfo[(colon + 1)..]);
        }
        else
        {
            user = Uri.UnescapeDataString(userInfo);
            password = "";
        }

        var slash = hostAndDb.IndexOf('/');
        var hostPort = slash >= 0 ? hostAndDb[..slash] : hostAndDb;
        var database = slash >= 0 ? hostAndDb[(slash + 1)..] : "";
        database = database.Split('?', '#')[0];

        string host;
        var port = 3306;
        if (hostPort.Contains(':'))
        {
            var lc = hostPort.LastIndexOf(':');
            host = hostPort[..lc];
            int.TryParse(hostPort[(lc + 1)..], out port);
        }
        else
            host = hostPort;

        var b = new MySqlConnectionStringBuilder
        {
            Server = host,
            Port = (uint)port,
            UserID = user,
            Password = password,
            Database = database,
            SslMode = MySqlSslMode.Required,
        };

        return b.ConnectionString;
    }
}
