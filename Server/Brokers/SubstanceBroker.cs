namespace eSDSCom.Editor.Server.Brokers;

public interface ISubstanceBroker
{
    Task<Substance> Get(string ID);

    Task<Substance> GetById(string substanceId);

    Task<Substance> GetByECNumber(string ecNumber);

    Task<Substance> GetByCASNumber(string casNumber);

    Task<List<Substance>> GetList(List<string> IDs);

    Task<List<Substance>> GetAll();
}

public class SubstanceBroker : BaseBroker, ISubstanceBroker
{
    private readonly IMemoryCache cache;

    public SubstanceBroker(IMemoryCache _cache)
    {
        cache = _cache;
    }

    public SubstanceBroker(IMemoryCache _cache, string testConnectionString)
    {
        cache = _cache;
        ConnectionString = testConnectionString;
    }


    public async Task<Substance> Get(string ID)
    {
        Substance subs = new();

        try
        {
            if (!cache.TryGetValue("AllSubstances", out List<Substance> allSubstances))
            {
                allSubstances = await GetAllSubstances();
            }

            subs = allSubstances.FirstOrDefault(p => p.ID == ID);
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

    public async Task<List<Substance>> GetList(List<string> IDs)
    {
        List<Substance> phraseList = new();

        try
        {
            if (!cache.TryGetValue("AllSubstances", out List<Substance> allPhrases))
            {
                allPhrases = await GetAllSubstances();
            }

            phraseList = (from a in allPhrases
                         join s in IDs on a.ID equals s
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
            using SqlConnection connection = new(ConnectionString);
            string sql = $"SELECT * FROM SUBSTANCES";

            using SqlCommand cmd = new(sql, connection);
            connection.Open();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                subsList.Add(GetSubstanceFromReader(reader));
            }
            await reader.CloseAsync();

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

