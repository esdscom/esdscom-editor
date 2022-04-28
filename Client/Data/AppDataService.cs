namespace eSDSCom.Editor.Client.Data;

public class AppDataService
{
    //  this class interacts with the api (in the server projecT)  for the entire application.
    //  sending an item to the db with a complex item in it, so those complex items - XML elements - 
    //  are stored in xml datatype fields in the db. 


    private HttpClient api;
    public AppDataService(HttpClient _api)
    {
        api = _api;
    }


    #region Users

    public async Task<User> AddUserAsync(User user)
    {
        User newUser = new();

        try
        {
            HttpResponseMessage response = await api.PostAsJsonAsync("user/add", user);
            newUser = await response.Content.ReadFromJsonAsync<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return newUser;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        User newUser = new();

        try
        {
            HttpResponseMessage response = await api.PutAsJsonAsync("user/update", user);
            newUser = await response.Content.ReadFromJsonAsync<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return newUser;
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        User user = await api.GetFromJsonAsync<User>($"user/get?id={id}");
        return user;
    }

    public async Task<List<User>> GetUserListForOrganizationAsync(Guid orgId)
    {
        List<User> userList = await api.GetFromJsonAsync<List<User>>($"user/getfororganization?organizationId={orgId}");
        return userList;
    }


    #endregion

    #region Organizations

    public async Task<Organization> AddOrganizationAsync(Organization organization)
    {       
        HttpResponseMessage response = await api.PostAsJsonAsync("organization/add", organization);
        return await response.Content.ReadFromJsonAsync<Organization>();
    }

    public async Task<Organization> UpdateOrganizationAsync(Organization organization)
    {
        HttpResponseMessage response = await api.PutAsJsonAsync("organization/update", organization);
        return await response.Content.ReadFromJsonAsync<Organization>();
    }

    public async Task<Organization> GetOrganizationAsync(Guid organizationId)
    {        
        Organization org = await api.GetFromJsonAsync<Organization>($"organization/get?id={organizationId}");
        return org;
    }

    #endregion


    #region Datasheets

    public async Task<Datasheet> AddDatasheetAsync(Datasheet datasheet)
    {    
        HttpResponseMessage response = await api.PostAsJsonAsync("datasheet/add", datasheet);
        Datasheet ds = await response.Content.ReadFromJsonAsync<Datasheet>();
        return ds;
    }

    public async Task<Datasheet> UpdateDatasheetAsync(Datasheet datasheet)
    {
        HttpResponseMessage response = await api.PutAsJsonAsync("datasheet/update", datasheet);
        Datasheet ds = await response.Content.ReadFromJsonAsync<Datasheet>();
        return ds;
    }

    public async Task<Datasheet> GetDatasheetAsync(Guid OrganizationId, Guid dsId)
    {
        Datasheet ds = await api.GetFromJsonAsync<Datasheet>($"datasheet/get?organizationId={OrganizationId}&dsId={dsId}");
        return ds;
    }

    public async Task<List<Datasheet>> GetDatasheetListForOrganizationAsync(Guid OrganizationId)
    {
        List<Datasheet> returnList = new();

        List<Datasheet> dsList = await api.GetFromJsonAsync<List<Datasheet>>($"datasheet/getfororganization?OrganizationId={OrganizationId}");

        foreach (Datasheet ds in dsList)
        {
            returnList.Add(ds);
        }

        return returnList;
    }

    public async Task<List<Datasheet>> GetDatasheetListForOrganizationAndDocumentSetAsync(Guid OrganizationId, Guid docSetId)
    {
        List<Datasheet> returnList = new();

        try
        {
            DatasheetFeed dsFeed = await GetDatasheetFeedAsync(OrganizationId, docSetId);

            List<Datasheet> dsList = new();

            //foreach (string dsId in dsFeed.DocumentIdList)
            //{
            //    Datasheet ds = await GetDatasheetAsync(OrganizationId, dsId);
            //    returnList.Add(ds);
            //}
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        return returnList;
    }



    #endregion


    #region DatasheetFeeds

    public async Task<DatasheetFeed> AddDatasheetFeedAsync(DatasheetFeed datasheetFeed)
    {        
        HttpResponseMessage response = await api.PostAsJsonAsync("datasheetfeed/add", datasheetFeed);
        return await response.Content.ReadFromJsonAsync<DatasheetFeed>();
    }

    public async Task<DatasheetFeed> UpdateDatasheetFeedAsync(DatasheetFeed datasheetFeed)
    {
        HttpResponseMessage response = await api.PutAsJsonAsync("datasheetfeed/update", datasheetFeed);
        return await response.Content.ReadFromJsonAsync<DatasheetFeed>();
    }

    public async Task<DatasheetFeedItem> AddDatasheetFeedItemAsync(DatasheetFeedItem dsfi)
    {        
        HttpResponseMessage response = await api.PostAsJsonAsync("datasheetfeeditem/add", dsfi);
        return await response.Content.ReadFromJsonAsync<DatasheetFeedItem>();
    }

    public async Task<DatasheetFeed> GetDatasheetFeedAsync(Guid organizationId, Guid datsheetFeedId)
    {
        return await api.GetFromJsonAsync<DatasheetFeed>($"datasheetfeed/get?organizationId={organizationId}&dsfId={datsheetFeedId}");
    }

    public async Task<List<DatasheetFeed>> GetDatasheetFeedListForOrganizationAsync(Guid OrganizationId)
    {
        return await api.GetFromJsonAsync<List<DatasheetFeed>>($"datasheetfeed/getfororganization?organizationId={OrganizationId}");
    }

    #endregion


    #region Substances

    public async Task<List<Substance>> GetAllSubstancesAsync()
    {
        return await api.GetFromJsonAsync<List<Substance>>($"substance/getall");
    }

    public async Task<Substance> GetSubstanceAsync(Guid substanceId)
    {
        return await api.GetFromJsonAsync<Substance>($"substance/getbyguid?substanceId={substanceId}");
    }

    #endregion
}

