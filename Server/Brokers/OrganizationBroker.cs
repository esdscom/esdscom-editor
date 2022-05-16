namespace eSDSCom.Editor.Server.Brokers;

public interface IOrganizationBroker
{
    Task<Organization> Get(Guid orgId);
    Task<Organization> Add(Organization org);
    Task<Organization> Update(Organization org);
    Task<bool> Delete(Guid orgId);
}

public class OrganizationBroker : IOrganizationBroker
{
    private readonly string ConnectionString;

    public OrganizationBroker(string connString)
    {
        ConnectionString = connString;
    }

    public async Task<Organization> Get(Guid orgId)
    {
        Organization org = new();

        try
        {
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = "SELECT * FROM ORGANIZATIONS WHERE ID = $1 ";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = orgId }
                }
            };           

            dbConn.Open();
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                org = DBUtils.GetOrgFromReader(reader);
            }
            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return org;
    }

    public async Task<Organization> Add(Organization org)
    {
        try
        {
            if (org.Id == default)
            {
                org.Id = Guid.NewGuid();
            }

            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @"INSERT INTO ORGANIZATIONS
                                (ID, ORGANIZATIONTYPE, NAME, ADDRESS, INFORMATIONFROMEXPORTINGSYSTEM)
                            VALUES
                                ($1, $2, $3, $4, $5)";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = org.Id },
                    new() { Value = org.OrganizationType },
                    new() { Value = org.Name },
                    new() { Value = org.Address },
                    new() { Value = org.InfoExSysString },
                }
            };


            dbConn.Open();
            int result = await cmd.ExecuteNonQueryAsync();
            await dbConn.CloseAsync();

            if (result > 0)
            {
                return org;
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

    public async Task<Organization> Update(Organization org)
    {
        try
        {
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @"UPDATE ORGANIZATIONS
                            SET ORGANIZATIONTYPE =$1,
                                NAME = $2,
                                ADDRESS = $3,                                
                                INFORMATIONFROMEXPORTINGSYSTEM = $4
                            WHERE ID = $5";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {                    
                    new() { Value = org.OrganizationType },
                    new() { Value = org.Name },
                    new() { Value = org.Address },
                    new() { Value = org.InfoExSysString },
                    new() { Value = org.Id }
                }
            };

            dbConn.Open();
            int result = await cmd.ExecuteNonQueryAsync();
            await dbConn.CloseAsync();  

            if (result > 0)
            {
                return org;
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

    public async Task<bool> Delete(Guid orgId)
    {
        int result = 0;
        try
        {
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = "DELETE FROM ORGANIZATIONS WHERE ID = $1 ";
            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = orgId }
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

