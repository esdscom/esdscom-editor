namespace eSDSCom.Editor.Server.Brokers;

public interface IPhraseBroker
{    
    Task<Phrase> Get(string phraseCode);
    Task<List<Phrase>> GetListByPrefix(string strucCodePrefix);
}

public class PhraseBroker : BaseBroker , IPhraseBroker
{
    private readonly IMemoryCache cache;

    public PhraseBroker(IMemoryCache _cache)
    {
        cache = _cache;
    }

    public PhraseBroker(IMemoryCache _cache, string testConnectionString)
    {
        cache = _cache;
        ConnectionString = testConnectionString;
    }

    public async Task<Phrase> Get(string strucCode)
    {
        Phrase phrase = new();

        try
        {
            if (!cache.TryGetValue("AllPhrases", out List<Phrase> allPhrases))
            {
                allPhrases = await GetAllPhrases();
            }

            phrase = allPhrases.FirstOrDefault(p => p.StrucCode == strucCode);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return phrase;
    }

    /// <summary>
    /// This method is based on 'Startswith' functionality
    /// </summary>
    /// <param name="strucCodePrefix"></param>
    /// <returns></returns>
    public async Task<List<Phrase>> GetListByPrefix(string strucCodePrefix)
    {
       List<Phrase> phraseList = new();

        try
        {
            if (!cache.TryGetValue("AllPhrases", out List<Phrase> allPhrases))
            {
                allPhrases = await GetAllPhrases();
            }

            phraseList = allPhrases.Where(p => p.StrucCode.StartsWith(strucCodePrefix)).ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return phraseList;
    }

    public async Task<List<Phrase>> GetAll()
    {
        List<Phrase> phraseList = new();

        try
        {
            if (!cache.TryGetValue("AllPhrases", out List<Phrase> allPhrases))
            {
                allPhrases = await GetAllPhrases();
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

    private async Task <List<Phrase>> GetAllPhrases()
    {
        List<Phrase> phraseList = new();

        try
        {
            using NpgsqlConnection dbConn = new(ConnectionString);
            string sql = $"SELECT * FROM PHRASES";

            using NpgsqlCommand cmd = new(sql, dbConn);
            dbConn.Open();
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                phraseList.Add(GetPhraseFromReader(reader));
            }
            await reader.CloseAsync();

            cache.Set("AllPhrases", phraseList);
        }

        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }

        return phraseList;
    }
}

