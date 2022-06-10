namespace eSDSCom.Editor.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SubstanceController : Controller
{
    readonly SubstanceBroker sBkr;
    IConfiguration Configuration;

    public SubstanceController(IMemoryCache cache, IConfiguration _configuration)
    {
        Configuration = _configuration;
        string connString = Configuration.GetConnectionString("LocalConnectionString");
        sBkr = new(cache, connString);
    }

    [HttpGet]
    [Route("Get")]
    public async Task<ActionResult<Substance>> Get(Guid id)
    {
        Substance subs = await sBkr.Get(id);
        return Ok(subs);
    }

    [HttpGet]
    [Route("GetByGuid")]
    public async Task<ActionResult<Substance>> GetByGuid(string substanceId)
    {
        Substance subs = await sBkr.GetById(substanceId);
        return Ok(subs);
    }


    [HttpGet]
    [Route("GetByECNumber")]
    public async Task<ActionResult<Substance>> GetByECNumber(string ecNumber)
    {
        Substance subs = await sBkr.GetByECNumber(ecNumber);
        return Ok(subs);
    }

    [HttpGet]
    [Route("GetByGuid")]
    public async Task<ActionResult<Substance>> GetByCASNumber(string casNumber)
    {
        Substance subs = await sBkr.GetByCASNumber(casNumber);
        return Ok(subs);
    }

    [HttpGet]
    [Route("GetList")]
    public async Task<ActionResult<Substance>> GetList(List<string> IDs)
    {
        List<Substance> subsList = await sBkr.GetList(IDs);
        return Ok(subsList);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<Substance>> GetAll()
    {
        List<Substance> subsList = await sBkr.GetAll();
        return Ok(subsList);
    }
}

