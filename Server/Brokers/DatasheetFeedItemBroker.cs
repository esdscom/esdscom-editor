namespace eSDSCom.Editor.Server.Brokers;

public interface IDatasheetFeedItemBroker
{
    Task<DatasheetFeedItem> Get(Guid datasheetId, Guid datasheetFeedId);
    Task<List<DatasheetFeedItem>> GetForDatasheetFeed(Guid datasheetFeedId);
    Task<DatasheetFeedItem> Add(DatasheetFeedItem dsfi);
    Task<bool> Delete(Guid datasheetId, Guid datasheetFeedId);
}

public class DatasheetFeedItemBroker : BaseBroker, IDatasheetFeedItemBroker
{
    public DatasheetFeedItemBroker() {}

    public DatasheetFeedItemBroker(string testConnectionString)
    {
        ConnectionString = testConnectionString;
    }

    public async Task<DatasheetFeedItem> Get(Guid datasheetId, Guid datasheetFeedId)
    {
        DatasheetFeedItem dsfi = new();
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = "SELECT * FROM DatasheetFeedItems WHERE DatasheetId = @DSID AND DatasheetFeedId = @DSFID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@DSID", SqlDbType.UniqueIdentifier));            
            cmd.Parameters.Add(new SqlParameter("@DSFID", SqlDbType.UniqueIdentifier));

            cmd.Parameters["@DSID"].Value = datasheetId;
            cmd.Parameters["@DSFID"].Value = datasheetFeedId;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                dsfi = GetDatasheetFeedItemFromReader(reader);
            }
            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return dsfi;
    }

    public async Task<List<DatasheetFeedItem>> GetForDatasheetFeed(Guid datasheetFeedId)
    {
        List<DatasheetFeedItem> dsfiList = new();
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = "SELECT * FROM DatasheetFeedItems WHERE DatasheetFeedId = @DSFID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@DSFID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@DSFID"].Value = datasheetFeedId;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                dsfiList.Add(GetDatasheetFeedItemFromReader(reader));
            }
            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return dsfiList;
    }

    public async Task<DatasheetFeedItem> Add(DatasheetFeedItem dsfi)
    {
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"INSERT INTO DatasheetFeedItems
                                (DatasheetFeedId, DatasheetId, USERID, CREATEDDATE, UPDATEDDATE)
                            VALUES
                                (@DSFID, @DSID, @USERID, getdate(), getdate())";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@DSFID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@DSID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.UniqueIdentifier));

            cmd.Parameters["@DSFID"].Value = dsfi.DatasheetFeedId;
            cmd.Parameters["@DSID"].Value = dsfi.DatasheetId;
            cmd.Parameters["@USERID"].Value = dsfi.UserId;

            connection.Open();
            int result = await cmd.ExecuteNonQueryAsync();

            if (result > 0)
            {
                return dsfi;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }
    }

    public async Task<bool> Delete(Guid datasheetFeedId, Guid datasheetId)
    {
        int result = 0;
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = "DELETE FROM DatasheetFeedItems WHERE DatasheetId = @DSID AND DatasheetFeedId = @DSFID";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@DSID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@DSFID", SqlDbType.UniqueIdentifier));

            cmd.Parameters["@DSID"].Value = datasheetId;
            cmd.Parameters["@DSFID"].Value = datasheetFeedId;

            connection.Open();
            result = await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
        return result > 0;
    }
}

