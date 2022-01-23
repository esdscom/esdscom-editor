namespace eSDSCom.Editor.Server.Brokers;

public interface IOrganizationBroker
{
    Task<Organization> Get(Guid orgId);
    Task<Organization> Add(Organization org);
    Task<Organization> Update(Organization org);
    Task<bool> Delete(Guid orgId);
}

public class OrganizationBroker : BaseBroker, IOrganizationBroker
{
    public OrganizationBroker() {}

    public OrganizationBroker(string testConnectionString)
    {
        ConnectionString = testConnectionString;
    }

    public async Task<Organization> Get(Guid orgId)
    {
        Organization org = new();

        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = "SELECT * FROM ORGANIZATIONS WHERE ID = @ID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@ID"].Value = orgId;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                org = GetOrgFromReader(reader);
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
            using SqlConnection connection = new(ConnectionString);
            string sql = @"INSERT INTO ORGANIZATIONS
                                (ID, ORGANIZATIONTYPE, NAME, ADDRESS, CREATEDDATE, UPDATEDDATE, INFORMATIONFROMEXPORTINGSYSTEM)
                            VALUES
                                (@ID, @ORGTYPE, @NAME,@ADDRESS, getdate(), getdate(), @INFO)";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@ORGTYPE", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar, 250));
            cmd.Parameters.Add(new SqlParameter("@ADDRESS", SqlDbType.NVarChar, 1000));
            cmd.Parameters.Add(new SqlParameter("@INFO", SqlDbType.NVarChar,-1));

            cmd.Parameters["@ID"].Value = org.Id;
            cmd.Parameters["@ORGTYPE"].Value = org.OrganizationType;
            cmd.Parameters["@NAME"].Value = org.Name;
            cmd.Parameters["@ADDRESS"].Value = org.Address;
            cmd.Parameters["@INFO"].Value = org.InfoExSys;

           connection.Open();
           int result = await cmd.ExecuteNonQueryAsync();

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
            using SqlConnection connection = new(ConnectionString);
            string sql = @"UPDATE ORGANIZATIONS
                            SET ORGANIZATIONTYPE = @ORGTYPE,
                                NAME = @NAME,
                                ADDRESS = @ADDRESS,
                                UPDATEDDATE = getdate(),
                                INFORMATIONFROMEXPORTINGSYSTEM = @INFO
                            WHERE ID = @ID";

            using SqlCommand cmd = new(sql, connection);         
            cmd.Parameters.Add(new SqlParameter("@ORGTYPE", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar, 250));
            cmd.Parameters.Add(new SqlParameter("@ADDRESS", SqlDbType.NVarChar, 1000));
            cmd.Parameters.Add(new SqlParameter("@INFO", SqlDbType.NVarChar,-1));
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));

            cmd.Parameters["@ORGTYPE"].Value = org.OrganizationType;
            cmd.Parameters["@NAME"].Value = org.Name;
            cmd.Parameters["@ADDRESS"].Value = org.Address;
            cmd.Parameters["@INFO"].Value = org.InfoExSys;
            cmd.Parameters["@ID"].Value = org.Id;

            connection.Open();
            int result = await cmd.ExecuteNonQueryAsync();

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
            using SqlConnection connection = new(ConnectionString);
            string sql = "DELETE FROM ORGANIZATIONS WHERE ID = @ID";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@ID"].Value = orgId;

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

