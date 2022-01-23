namespace eSDSCom.Editor.Server.Brokers;
public interface IDatasheetBroker
{
    Task<Datasheet> Get(Guid organizationId, Guid dsId);
    Task<Datasheet> Add(Datasheet sheet);
    Task<Datasheet> Update(Datasheet sheet);
    Task<bool> Delete(Guid dsId);
    Task<List<Datasheet>> GetForOrganization(Guid organizationId);
    Task<Datasheet> GetMetadataOnly(Guid dsId);
}

public class DatasheetBroker : BaseBroker, IDatasheetBroker
{
   
    public DatasheetBroker() { }

    public DatasheetBroker(string testConnectionString)
    {
        ConnectionString = testConnectionString;
    }

    public async Task<Datasheet> Get(Guid organizationId, Guid dsId)
    {
        Datasheet ds = new();

        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"SELECT ds.*, u.NAME as USERNAME 
                            FROM DATASHEETS ds , USERS u
                            WHERE ds.UserId = u.ID
                            AND ds.ORGANIZATIONID = @ORGID
                            AND ds.ID = @ID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@ORGID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@ID"].Value = dsId;
            cmd.Parameters["@ORGID"].Value = organizationId;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                ds = GetDatasheetFromReader(reader);
            }
            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return ds;
    }

    public async Task<Datasheet> GetMetadataOnly(Guid dsId)
    {
        //dont get the DatasheetDoc field

        Datasheet ds = new();

        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"SELECT   ds.Id, ds.OrganizationId, ds.UserId, ds.Name, 
                                    ds.CreatedDate, ds.UpdatedDate, ds.Status, ds.Comments, '' as DatasheetDoc,
                                    u.NAME as USERNAME 
                            FROM DATASHEETS ds , USERS u
                            WHERE ds.UserId = u.ID
                            AND ds.ID = @ID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@ID"].Value = dsId;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                ds = GetDatasheetFromReader(reader);
            }
            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return ds;
    }

    public async Task<List<Datasheet>> GetForOrganization(Guid organizationId)
    {
        List<Datasheet> dsList = new();

        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"SELECT ds.*, u.NAME as USERNAME 
                            FROM DATASHEETS ds , USERS u
                            WHERE ds.UserId = u.ID 
                            AND ds.ORGANIZATIONID = @ID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@ID"].Value = organizationId;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                dsList.Add(GetDatasheetFromReader(reader));
            }

            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return dsList;
    }

    public async Task<Datasheet> Add(Datasheet ds)
    {
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"INSERT INTO Datasheets
                                (ID, ORGANIZATIONID, USERID, NAME, CREATEDDATE, UPDATEDDATE, STATUS, DATASHEETDOC, COMMENTS)
                            VALUES
                                (@ID, @ORGID, @USERID, @NAME, getdate(), getdate(), @STATUS, @DOC, @COMMENTS)";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@ORGID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar, 250));
            cmd.Parameters.Add(new SqlParameter("@STATUS", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@DOC", SqlDbType.NVarChar,-1));
            cmd.Parameters.Add(new SqlParameter("@COMMENTS", SqlDbType.NVarChar, -1));

            cmd.Parameters["@ID"].Value = ds.Id;
            cmd.Parameters["@ORGID"].Value = ds.OrganizationId;
            cmd.Parameters["@USERID"].Value = ds.UserId;
            cmd.Parameters["@NAME"].Value = ds.Name;
            cmd.Parameters["@STATUS"].Value = ds.Status;
            cmd.Parameters["@DOC"].Value = ds.DatasheetString;
            cmd.Parameters["@COMMENTS"].Value = ds.Comments;

            connection.Open();
            int result = await cmd.ExecuteNonQueryAsync();

            if (result > 0)
            {
                return ds;
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

    public async Task<Datasheet> Update(Datasheet ds)
    {
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @"UPDATE Datasheets
                            SET ORGANIZATIONID = @ORGID, 
                                USERID = @USERID,                                
                                NAME = @NAME, 
                                UPDATEDDATE = getdate(), 
                                STATUS = @STATUS,
                                DATASHEETDOC = @DOC,
                                COMMENTS = @COMMENTS
                            WHERE ID = @ID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ORGID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@USERID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar, 250));
            cmd.Parameters.Add(new SqlParameter("@STATUS", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@DOC", SqlDbType.NVarChar, -1));
            cmd.Parameters.Add(new SqlParameter("@COMMENTS", SqlDbType.NVarChar, -1));            
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));

            cmd.Parameters["@ORGID"].Value = ds.OrganizationId;
            cmd.Parameters["@USERID"].Value = ds.UserId;
            cmd.Parameters["@NAME"].Value = ds.Name;
            cmd.Parameters["@STATUS"].Value = ds.Status;
            cmd.Parameters["@DOC"].Value = ds.DatasheetString;
            cmd.Parameters["@COMMENTS"].Value = ds.Comments;
            cmd.Parameters["@ID"].Value = ds.Id;

            connection.Open();
            int result = await cmd.ExecuteNonQueryAsync();

            if (result > 0)
            {
                return ds;
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

    public async Task<bool> Delete(Guid dsId)
    {
        int result = 0;
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = "DELETE FROM Datasheets WHERE ID = @ID";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@ID"].Value = dsId;

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

