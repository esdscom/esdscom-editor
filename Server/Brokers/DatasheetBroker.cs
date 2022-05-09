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

public class DatasheetBroker : IDatasheetBroker
{
    //  Throughout these Broker classes the Postgres standard positional parameters are used
    //  instead of the more ADO-friendly syntax such as .AddParameter or .AddParameterWithValue
    //  this is to reduce the work NpgSQL has to do around parsing the query commands

    private static string ConnectionString;

    public DatasheetBroker() 
    {
        ConnectionString = DBUtils.GetConnectionString();
    }

    public DatasheetBroker(string testConnectionString)
    {
        ConnectionString = testConnectionString;
    }

    public async Task<Datasheet> Get(Guid organizationId, Guid dsId)
    {
        Datasheet ds = new();

        try
        {
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @"SELECT ds.*, u.NAME as USERNAME 
                            FROM DATASHEETS ds , USERS u
                            WHERE ds.UserId = u.ID
                            AND ds.ORGANIZATIONID = $1
                            AND ds.ID = $2 ";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters = 
                { 
                    new() { Value = organizationId },
                    new() { Value = dsId }
                }

            };          

            dbConn.Open();
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                ds = DBUtils.GetDatasheetFromReader(reader);
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
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @"SELECT   ds.Id, ds.ORGANIZATIONID, ds.USERID, ds.NAME, 
                                    ds.CREATEDDATE, ds.UPDATEDDATE, ds.STATUS, ds.COMMENTS, '' as DatasheetDoc, ds.REGIONSSTRING,
                                    u.NAME as USERNAME 
                            FROM DATASHEETS ds , USERS u
                            WHERE ds.USERID = u.ID
                            AND ds.ID = $1 ";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = dsId }
                }
            };

            dbConn.Open();
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                ds = DBUtils.GetDatasheetFromReader(reader);
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
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @"SELECT ds.*, u.NAME as USERNAME 
                            FROM DATASHEETS ds , USERS u
                            WHERE ds.UserId = u.ID 
                            AND ds.ORGANIZATIONID = $1 ";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = organizationId }
                }
            };

            dbConn.Open();
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                dsList.Add(DBUtils.GetDatasheetFromReader(reader));
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
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @"INSERT INTO Datasheets
                                (ID, ORGANIZATIONID, USERID, NAME, STATUS, DATASHEETDOC, COMMENTS, REGIONSSTRING)
                            VALUES
                                ($1, $2, $3, $4, $5, $6, $7, $8) ";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = ds.Id },
                    new() { Value = ds.OrganizationId },
                    new() { Value = ds.UserId },
                    new() { Value = ds.Name },
                    new() { Value = ds.Status },
                    new() { Value = ds.DatasheetString},
                    new() { Value = ds.Comments },
                    new() { Value = ds.RegionsString }
                }
            };

            dbConn.Open();
            int result = await cmd.ExecuteNonQueryAsync();
            await dbConn.CloseAsync();

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
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @"UPDATE Datasheets
                            SET ORGANIZATIONID = $1, 
                                USERID = $2,                                
                                NAME = $3, 
                                STATUS = $4,
                                DATASHEETDOC = $5,
                                COMMENTS = $6,
                                REGIONSSTRING = $7
                            WHERE ID = $8 ";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = ds.OrganizationId },
                    new() { Value = ds.UserId },
                    new() { Value = ds.Name },
                    new() { Value = ds.Status },
                    new() { Value = ds.DatasheetString },
                    new() { Value = ds.Comments },
                    new() { Value = ds.RegionsString },
                    new() { Value = ds.Id },

                }
            };

            dbConn.Open();
            int result = await cmd.ExecuteNonQueryAsync();
            await dbConn.CloseAsync();

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
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = "DELETE FROM Datasheets WHERE ID = $1 ";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = dsId }
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

