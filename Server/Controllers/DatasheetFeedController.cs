namespace eSDSCom.Editor.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class DatasheetFeedController : Controller
{
   readonly DatasheetFeedBroker dsfBkr;
   readonly DatasheetFeedItemBroker dsfiBkr;
   readonly DatasheetBroker dsBkr;

   IConfiguration Configuration;

    public DatasheetFeedController(IConfiguration _configuration)
    {
        Configuration = _configuration;
        string connString = Configuration.GetConnectionString("LocalConnectionString");
        dsfBkr = new(connString);
        dsfiBkr = new(connString);
        dsBkr = new(connString);
    }

    [HttpGet]
    [Route("Get")]
    public async Task<ActionResult<DatasheetFeed>> Get(Guid organizationId, Guid dsfId, bool includeDatasheets = true)
    {
        DatasheetFeed dsf = await dsfBkr.Get(organizationId,dsfId);
        dsf.DatasheetItems = await dsfiBkr.GetForDatasheetFeed(dsf.Id);

        if (includeDatasheets)
        {
            foreach (var dsfi in dsf.DatasheetItems)
            {
                dsfi.Datasheet = await dsBkr.Get(organizationId, dsfi.DatasheetId);
            }
        }

        return dsf;
    }   

    [HttpGet]
    [Route("GetForOrganization")]
    public async Task<ActionResult<List<DatasheetFeed>>> GetForOrganization(Guid organizationId, bool includeDatasheets = true)
    {  
        List<DatasheetFeed> dsfList = await dsfBkr.GetForOrganization(organizationId);        
        foreach (var dsf in dsfList)
        {
            dsf.DatasheetItems = await dsfiBkr.GetForDatasheetFeed(dsf.Id);
            dsf.DatasheetCount = dsf.DatasheetItems.Count;

            if (includeDatasheets)
            {
                foreach (var dsfi in dsf.DatasheetItems)
                {
                    dsfi.Datasheet = await dsBkr.Get(organizationId, dsfi.DatasheetId);
                }
            }
        }

        return dsfList;
    }

    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<DatasheetFeed>> Add(DatasheetFeed dsFeed)
    {
        return await dsfBkr.Add(dsFeed);
    }

    [HttpPut]
    [Route("Update")]
    public async Task<ActionResult<DatasheetFeed>> Update(DatasheetFeed dsFeed)
    {
        return await dsfBkr.Update(dsFeed);
    }

    [HttpDelete]
    [Route("Delete")]
    public async Task<ActionResult<bool>> Delete(Guid dsfId)
    {
        return await dsfBkr.Delete(dsfId);
    }
}


