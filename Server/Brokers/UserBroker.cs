namespace eSDSCom.Editor.Server.Brokers;

public interface IUserBroker
{
    Task<User> Get(Guid id);
    Task<List<User>> GetForOrganization(Guid organizationId, bool activeOnly);
    Task<User> Add(User user);
    Task<User> Update(User user);
    Task<bool> Delete(Guid id);
}

public class UserBroker : BaseBroker, IUserBroker
{
    public UserBroker() {}

    public UserBroker(string testConnectionString)
    {
        ConnectionString = testConnectionString;
    }

    public async Task<User> Get(Guid id)
    {
        User user = new();

        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = "SELECT * FROM USERS WHERE ID = @ID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@ID"].Value = id;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                user = GetUserFromReader(reader);
            }
            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return user;
    }

    public async Task<List<User>> GetForOrganization(Guid organizationId, bool activeOnly)
    {
        List<User> userList = new();

        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = "SELECT * FROM USERS WHERE ORGANIZATIONID = @ID";
            if (activeOnly)
            {
                sql = $"{sql} AND ISACTIVE = 1 ";
            }

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters["@ID"].Value = organizationId;

            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                userList.Add(GetUserFromReader(reader));
            }
            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return userList;
    }



    public async Task<User> Add(User user)
    {
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @" INSERT INTO USERS 
                                (ID, ORGANIZATIONID,EMAIL,NAME,ROLE,ISACTIVE,CREATEDDATE, UPDATEDDATE)
                            VALUES 
                                (@ID,@ORGID,@EMAIL,@NAME,@ROLE,@ISACTIVE,getdate(), getdate())";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@ORGID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@EMAIL", SqlDbType.NVarChar,250));
            cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar, 250));
            cmd.Parameters.Add(new SqlParameter("@ROLE", SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@ISACTIVE", SqlDbType.Bit));

            cmd.Parameters["@ID"].Value = user.Id;
            cmd.Parameters["@ORGID"].Value = user.OrganizationId;
            cmd.Parameters["@EMAIL"].Value = user.Email;
            cmd.Parameters["@NAME"].Value = user.Name;
            cmd.Parameters["@ROLE"].Value = user.Role;
            cmd.Parameters["@ISACTIVE"].Value = user.IsActive;

            connection.Open();
            int result = await cmd.ExecuteNonQueryAsync();

            if (result > 0)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        return null;
    }

    public async Task<User> Update(User user)
    {
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = @" UPDATE USERS 
                                SET ORGANIZATIONID= @ORGID,
                                    EMAIL = @EMAIL,
                                    NAME = @NAME,
                                    ROLE = @ROLE,
                                    ISACTIVE = @ISACTIVE,
                                    UPDATEDDATE = getdate()
                            WHERE ID = @ID";

            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@ORGID", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@EMAIL", SqlDbType.NVarChar, 250));
            cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar, 250));
            cmd.Parameters.Add(new SqlParameter("@ROLE", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@ISACTIVE", SqlDbType.Bit));

            cmd.Parameters["@ID"].Value = user.Id;
            cmd.Parameters["@ORGID"].Value = user.OrganizationId;
            cmd.Parameters["@EMAIL"].Value = user.Email;
            cmd.Parameters["@NAME"].Value = user.Name;
            cmd.Parameters["@ISACTIVE"].Value = user.IsActive;
            cmd.Parameters["@UPDATEDDATE"].Value = DateTime.Now;

            connection.Open();
            int result = await cmd.ExecuteNonQueryAsync();

            if (result == 0)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
        return user;
    }


    public async Task<bool> Delete(Guid id)
    {
        int result = 0;
        try
        {
            using SqlConnection connection = new(ConnectionString);
            string sql = "DELETE FROM USERS WHERE ID = @ID";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier)); 
            cmd.Parameters["@ID"].Value = id;

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

