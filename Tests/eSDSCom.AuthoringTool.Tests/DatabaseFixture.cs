using System.Data.SqlClient;

namespace eSDSCom.Editor.Tests;

public class DatabaseFixture : IDisposable
{
    string TestConnectionString = @"Server=DBSERVER\SQLSRVR;Database=AuthorDBTest;User ID=sa;Password=Gollum17";

    public DatabaseFixture() { }

    public void Dispose()
    {
        SqlConnection conn = new(TestConnectionString);
        conn.Open();
        SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = "truncate table USERS";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "truncate table ORGANIZATIONS";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "truncate table DATASHEETS";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "truncate table DATASHEETFEEDITEMS";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "truncate table DATASHEETFEEDS";
        cmd.ExecuteNonQuery();

        conn.Close();
        conn.Dispose();
    }

    // public SqlConnection Db { get; private set; }
}

