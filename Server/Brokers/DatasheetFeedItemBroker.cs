namespace eSDSCom.Editor.Server.Brokers;

public interface IDatasheetFeedItemBroker
{
    Task<DatasheetFeedItem> Get(Guid datasheetId, Guid datasheetFeedId);
    Task<List<DatasheetFeedItem>> GetForDatasheetFeed(Guid datasheetFeedId);
    Task<DatasheetFeedItem> Add(DatasheetFeedItem dsfi);
    Task<bool> Delete(Guid datasheetId, Guid datasheetFeedId);
}

public class DatasheetFeedItemBroker : IDatasheetFeedItemBroker
{
    private static string ConnectionString;
    public DatasheetFeedItemBroker() 
    {
        ConnectionString = DBUtils.GetConnectionString();    
    }

    public DatasheetFeedItemBroker(string testConnectionString)
    {
        ConnectionString = testConnectionString;
    }

    public async Task<DatasheetFeedItem> Get(Guid datasheetFeedId, Guid datasheetId)
    {
        DatasheetFeedItem dsfi = new();
        try
        {
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @"SELECT * FROM DATASHEETFEEDITEMS 
                            WHERE DATASHEETFEEDID = $1 
                            AND DATASHEETID = $2";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = datasheetFeedId },
                    new() { Value = datasheetId }
                }
            };

            dbConn.Open();
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                dsfi = DBUtils.GetDatasheetFeedItemFromReader(reader);
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
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = "SELECT * FROM DATASHEETFEEDITEMS WHERE DATASHEETFEEDID = $1";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = datasheetFeedId }
                }
            };

            dbConn.Open();
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                dsfiList.Add(DBUtils.GetDatasheetFeedItemFromReader(reader));
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
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @"INSERT INTO DATASHEETFEEDITEMS
                                (DATASHEETFEEDID, DATASHEETID, USERID)
                            VALUES
                                ($1, $2, $3) ";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = dsfi.DatasheetFeedId },
                    new() { Value = dsfi.DatasheetId },
                    new() { Value = dsfi.UserId },
                }
            };


            dbConn.Open();
            int result = await cmd.ExecuteNonQueryAsync();
            await dbConn.CloseAsync();

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
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @"DELETE FROM DATASHEETFEEDITEMS 
                            WHERE DATASHEETFEEDID = $1 
                            AND DATASHEETID = $2 ";
            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = datasheetFeedId },
                    new() { Value = datasheetId },
                }
            };

            dbConn.Open();
            result = await cmd.ExecuteNonQueryAsync();
            await dbConn.CloseAsync();

        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
        return result > 0;
    }
}

