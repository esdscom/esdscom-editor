


namespace eSDSCom.Editor.Tests;

public class DatabaseFixture : BaseTestData, IDisposable
{
    public DatabaseFixture() { }

    public void Dispose()
    {
        //NpgsqlConnection conn = new(TestConnectionString);
        //conn.Open();
        //NpgsqlCommand cmd = conn.CreateCommand();
        //cmd.CommandText = "truncate table USERS";
        //cmd.ExecuteNonQuery();

        //cmd.CommandText = "truncate table ORGANIZATIONS";
        //cmd.ExecuteNonQuery();

        //cmd.CommandText = "truncate table DATASHEETS";
        //cmd.ExecuteNonQuery();

        //cmd.CommandText = "truncate table DATASHEETFEEDITEMS";
        //cmd.ExecuteNonQuery();

        //cmd.CommandText = "truncate table DATASHEETFEEDS";
        //cmd.ExecuteNonQuery();

        //conn.Close();
        //conn.Dispose();
    }

    // public SqlConnection Db { get; private set; }
}

