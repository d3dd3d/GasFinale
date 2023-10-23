using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

static class DBManager
{
    private const string fileName = "dbase.bytes";
    public static string DBPath;
    private static SqliteConnection connection;
    private static SqliteCommand command;
    static DBManager()
{
    DBPath = GetDatabasePath();
}

private static string GetDatabasePath()
{
    return Path.Combine(Application.streamingAssetsPath, fileName);
}

private static void UnpackDatabase(string toPath)
{
    string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

    WWW reader = new WWW(fromPath);
    while (!reader.isDone) { }

    File.WriteAllBytes(toPath, reader.bytes);
}
private static void OpenConnection()
{
    connection = new SqliteConnection("Data Source=" + DBPath);
    command = new SqliteCommand(connection);
    connection.Open();
}

public static void CloseConnection()
{
    connection.Close();
    command.Dispose();
}

public static void ExecuteQueryWithoutAnswer(string query)
{
    OpenConnection();
    command.CommandText = query;
    command.ExecuteNonQuery();
    CloseConnection();
}
public static string ExecuteQueryWithAnswer(string query)
{
    OpenConnection();
    command.CommandText = query;
    var answer = command.ExecuteScalar();
    CloseConnection();

    if (answer != null) return answer.ToString();
    else return null;
}
public static DataTable GetTable(string query)
{
    OpenConnection();

    SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

    DataSet DS = new DataSet();
    adapter.Fill(DS);
    adapter.Dispose();

    CloseConnection();

    return DS.Tables[0];
}
}
