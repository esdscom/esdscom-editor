namespace eSDSCom.Editor.Server.Brokers;

public interface IDatasheetFeedBroker
{
    Task<DatasheetFeed> Get(Guid organizationId, Guid dsfId);
    Task<List<DatasheetFeed>> GetForOrganization(Guid organizationId);
    Task<List<DatasheetFeed>> GetForUser(Guid organizationId, Guid userId);
    Task<DatasheetFeed> Add(DatasheetFeed sheet);
    Task<DatasheetFeed> Update(DatasheetFeed sheet);
    Task<bool> Delete(Guid dsfId);
}

public class DatasheetFeedBroker : BaseBroker, IDatasheetFeedBroker
{  
    public DatasheetFeedBroker() {}
    public DatasheetFeedBroker(string testConnectionString)
    {
        ConnectionString = testConnectionString;
    }

    public async Task<DatasheetFeed> Get(Guid organizationId, Guid dsfId)
    {
        DatasheetFeed dsf = new();

        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"SELECT dsf.*, u.NAME as USERNAME 
                            FROM DATASHEETFEEDS dsf , USERS u
                            WHERE dsf.UserId = u.ID
                            AND dsf.ID = @ID
                            AND dsf.ORGANIZATIONID = @ORGID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@ORGID", SqlDbType.UniqueIdentifier));
            
            cmd.Parameters["@ID"].Value = dsfId;
            cmd.Parameters["@ORGID"].Value = organizationId;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                dsf = GetDatasheetFeedFromReader(reader);
            }

            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return dsf;
    }  


    public async Task<List<DatasheetFeed>> GetForOrganization(Guid organizationId)
    {
        List<DatasheetFeed> dsfList = new();

        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"SELECT dsf.*, u.NAME as USERNAME
                            FROM DATASHEETFEEDS dsf , USERS u
                            WHERE dsf.UserId = u.ID
                            AND dsf.ORGANIZATIONID = @ORGID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ORGID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@ORGID"].Value = organizationId;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                dsfList.Add(GetDatasheetFeedFromReader(reader));
            }
            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return dsfList;
    }

    public async Task<List<DatasheetFeed>> GetForUser(Guid organizationId, Guid userId)
    {
        List<DatasheetFeed> dsfList = new();

        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"SELECT dsf.*, u.NAME as USERNAME
                            FROM DATASHEETFEEDS dsf , USERS u
                            WHERE dsf.UserId = u.ID
                            AND dsf.ORGANIZATIONID = @ORGID
                            AND dsf.USERID = @UID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ORGID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@UID", SqlDbType.UniqueIdentifier));

            cmd.Parameters["@ORGID"].Value = organizationId;
            cmd.Parameters["@UID"].Value = userId;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                dsfList.Add(GetDatasheetFeedFromReader(reader));
            }
            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return dsfList;
    }

    public async Task<DatasheetFeed> Add(DatasheetFeed dsf)
    {
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"INSERT INTO DatasheetFeeds
                                (ID, ORGANIZATIONID, USERID, NAME, CREATEDDATE, UPDATEDDATE, DATASHEETFEEDDOC, COMMENTS, STATUS)
                            VALUES
                                (@ID, @ORGID, @USERID, @NAME, getdate(), getdate(), @DOC, @COMMENTS, @STATUS)";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@ORGID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar, 250));
            cmd.Parameters.Add(new SqlParameter("@DOC", SqlDbType.NVarChar, -1));
            cmd.Parameters.Add(new SqlParameter("@COMMENTS", SqlDbType.NVarChar, -1));
            cmd.Parameters.Add(new SqlParameter("@STATUS", SqlDbType.Int));


            cmd.Parameters["@ID"].Value = dsf.Id;
            cmd.Parameters["@ORGID"].Value = dsf.OrganizationId;
            cmd.Parameters["@USERID"].Value = dsf.UserId;
            cmd.Parameters["@NAME"].Value = dsf.Name;
            cmd.Parameters["@DOC"].Value = dsf.DatasheetFeedString;
            cmd.Parameters["@COMMENTS"].Value = dsf.Comments;
            cmd.Parameters["@STATUS"].Value = dsf.Status;

            connection.Open();
            int result = await cmd.ExecuteNonQueryAsync();

            if (result > 0)
            {
                return dsf;
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

    public async Task<DatasheetFeed> Update(DatasheetFeed dsf)
    {
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"UPDATE DatasheetFeeds
                            SET ORGANIZATIONID = @ORGID, 
                                USERID = @USERID, 
                                NAME = @NAME, 
                                UPDATEDDATE = getdate(), 
                                DATASHEETFEEDDOC = @DOC,
                                COMMENTS = @COMMENTS
                            WHERE ID = @ID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ORGID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar, 250));
            cmd.Parameters.Add(new SqlParameter("@DOC", SqlDbType.NVarChar, -1));
            cmd.Parameters.Add(new SqlParameter("@COMMENTS", SqlDbType.NVarChar, -1));
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));

            cmd.Parameters["@ORGID"].Value = dsf.OrganizationId;
            cmd.Parameters["@USERID"].Value = dsf.UserId;
            cmd.Parameters["@NAME"].Value = dsf.Name;
            cmd.Parameters["@DOC"].Value = dsf.DatasheetFeedString;
            cmd.Parameters["@COMMENTS"].Value = dsf.Comments;
            cmd.Parameters["@ID"].Value = dsf.Id;

            connection.Open();
            int result = await cmd.ExecuteNonQueryAsync();

            if (result > 0)
            {
                return dsf;
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

    public async Task<bool> Delete(Guid dsfId)
    {
        int result = 0;
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = "DELETE FROM DatasheetFeeds WHERE ID = @ID";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@ID"].Value = dsfId;

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

