namespace eSDSCom.Editor.Server.Brokers;

public interface IUserBroker
{
    Task<User> Get(Guid id);
    Task<List<User>> GetForOrganization(Guid organizationId, bool activeOnly);
    Task<User> Add(User user);
    Task<User> Update(User user);
    Task<bool> Delete(Guid id);
}

public class UserBroker : IUserBroker
{
    private static string ConnectionString;
    public UserBroker() 
    {
       ConnectionString = DBUtils.GetConnectionString();
    }

    public UserBroker(string testConnectionString)
    {
        ConnectionString = testConnectionString;
    }

    public async Task<User> Get(Guid id)
    {
        User user = new();

        try
        {
            using NpgsqlConnection connection = new(ConnectionString);
            string sql = "SELECT * FROM USERS WHERE ID = $1";

            using NpgsqlCommand cmd = new(sql, connection)
            {
                Parameters =
                {
                    new() { Value = id }
                }
            };

            connection.Open();
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                user = DBUtils.GetUserFromReader(reader);
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
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = "SELECT * FROM USERS WHERE ORGANIZATIONID = $1 ";
            if (activeOnly)
            {
                sql = $"{sql} AND ISACTIVE = True ";
            }

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
                userList.Add(DBUtils.GetUserFromReader(reader));
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
            if (user.Id == default)
            {
                user.Id = Guid.NewGuid();
            }

            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @" INSERT INTO USERS 
                                (ID,ORGANIZATIONID,EMAIL,NAME,ROLE,ISACTIVE)
                            VALUES 
                                ($1,$2,$3,$4,$5,$6)";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = user.Id },
                    new() { Value = user.OrganizationId },
                    new() { Value = user.Email },
                    new() { Value = user.Name },
                    new() { Value = user.Role },
                    new() { Value = user.IsActive }
                }
            };


            dbConn.Open();
            int result = await cmd.ExecuteNonQueryAsync();
            await dbConn.CloseAsync();

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
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = @" UPDATE USERS 
                                SET ORGANIZATIONID= $1,
                                    EMAIL = $2,
                                    NAME = $3,
                                    ROLE = $4,
                                    ISACTIVE = $5
                            WHERE ID = $6";

            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {                    
                    new() { Value = user.OrganizationId },
                    new() { Value = user.Email },
                    new() { Value = user.Name },
                    new() { Value = user.Role },
                    new() { Value = user.IsActive },
                    new() { Value = user.Id }
                }
            };

            dbConn.Open();
            int result = await cmd.ExecuteNonQueryAsync();
            await dbConn.CloseAsync();

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
        return user;
    }


    public async Task<bool> Delete(Guid id)
    {
        int result = 0;
        try
        {
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = "DELETE FROM USERS WHERE ID = $1";
            using NpgsqlCommand cmd = new(sql, dbConn)
            {
                Parameters =
                {
                    new() { Value = id }
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

