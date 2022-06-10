namespace eSDSCom.Editor.Server.Brokers;

public interface ISubstanceBroker
{
    Task<Substance> Get(Guid id);

    Task<Substance> GetById(string substanceId);

    Task<Substance> GetByECNumber(string ecNumber);

    Task<Substance> GetByCASNumber(string casNumber);

    Task<List<Substance>> GetList(List<string> substanceIds);

    Task<List<Substance>> GetAll();
}

public class SubstanceBroker : ISubstanceBroker
{
    private readonly IMemoryCache cache;
    private static string ConnectionString;
   
    public SubstanceBroker(IMemoryCache _cache, string connString)
    {
        cache = _cache;
        ConnectionString = connString;
    }

    public async Task<Substance> Get(Guid id)
    {
        Substance subs = new();

        try
        {
            if (!cache.TryGetValue("AllSubstances", out List<Substance> allSubstances))
            {
                allSubstances = await GetAllSubstances();
            }

            subs = allSubstances.FirstOrDefault(p => p.Id == id);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return subs;
    }

    public async Task<Substance> GetById(string substanceId)
    {
        Substance subs = new();

        try
        {
            if (!cache.TryGetValue("AllSubstances", out List<Substance> allSubstances))
            {
                allSubstances = await GetAllSubstances();
            }

            subs = allSubstances.FirstOrDefault(p => p.SubstanceId == substanceId);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return subs;
    }

    public async Task<Substance> GetByECNumber(string ecNumber)
    {
        Substance subs = new();

        try
        {
            if (!cache.TryGetValue("AllSubstances", out List<Substance> allSubstances))
            {
                allSubstances = await GetAllSubstances();
            }

            subs = allSubstances.FirstOrDefault(p => p.ECNumber == ecNumber);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return subs;
    }

    public async Task<Substance> GetByCASNumber(string casNumber)
    {
        Substance subs = new();

        try
        {
            if (!cache.TryGetValue("AllSubstances", out List<Substance> allSubstances))
            {
                allSubstances = await GetAllSubstances();
            }

            subs = allSubstances.FirstOrDefault(p => p.CASNumber == casNumber);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return subs;
    }

    public async Task<List<Substance>> GetList(List<string> substanceIds)
    {
        List<Substance> phraseList = new();

        try
        {
            if (!cache.TryGetValue("AllSubstances", out List<Substance> allPhrases))
            {
                allPhrases = await GetAllSubstances();
            }

            phraseList = (from a in allPhrases
                         join s in substanceIds on a.SubstanceId equals s
                         select a).ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return phraseList;
    }

    public async Task<List<Substance>> GetAll()
    {
        List<Substance> phraseList;

        try
        {
            if (!cache.TryGetValue("AllSubstances", out List<Substance> allPhrases))
            {
                allPhrases = await GetAllSubstances();
            }

            phraseList = allPhrases;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return phraseList;
    }

    private async Task<List<Substance>> GetAllSubstances()
    {
        List<Substance> subsList = new();

        try
        {
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = $"SELECT * FROM SUBSTANCES";

            using NpgsqlCommand cmd = new(sql, dbConn);
            dbConn.Open();
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                subsList.Add(DBUtils.GetSubstanceFromReader(reader));
            }
            await reader.CloseAsync();
            await dbConn.CloseAsync();

            cache.Set("AllSubstances", subsList);
        }

        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return subsList;
    }
}

