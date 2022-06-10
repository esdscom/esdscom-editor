namespace eSDSCom.Editor.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class DatasheetFeedItemController : Controller
{
    private readonly DatasheetFeedItemBroker dBkr;
    private IConfiguration Configuration;

    public DatasheetFeedItemController(IConfiguration configuration)
    {
        Configuration = configuration;
        string connString = Configuration.GetConnectionString("LocalConnectionString");
        dBkr = new DatasheetFeedItemBroker(connString);
        
    }

    [HttpGet]
    [Route("Get")]
    public async Task<ActionResult<DatasheetFeedItem>> Get(Guid datasheetFeedId, Guid datasheetId)
    {
        return await dBkr.Get(datasheetFeedId, datasheetId);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<DatasheetFeedItem>> Add(DatasheetFeedItem dsfi)
    {
        return await dBkr.Add(dsfi);
    }

    [HttpDelete]
    [Route("Delete")]
    public async Task<ActionResult<bool>> Delete(Guid datasheetFeedId, Guid datasheetId)
    {
        return await dBkr.Delete(datasheetFeedId, datasheetId);
    }
}


