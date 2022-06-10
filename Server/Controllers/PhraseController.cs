namespace eSDSCom.Editor.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PhraseController : Controller
{
    private readonly PhraseBroker pBkr;
    private IConfiguration Configuration;

    public PhraseController(IMemoryCache cache, IConfiguration configuration)
    {
        Configuration = configuration;
        string connString = Configuration.GetConnectionString("LocalConnectionString");
        pBkr = new(cache, connString);       
    }

    [HttpGet]
    [Route("Get")]
    public async Task<ActionResult<Phrase>> Get(string strucCode)
    {
        Phrase phrase = await pBkr.Get(strucCode);
        return Ok(phrase);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<List<Phrase>>> GetAll()
    {
        List<Phrase> phraseList = await pBkr.GetAll();
        return Ok(phraseList);
    }

    [HttpGet]
    [Route("GetListByPrefix")]
    public async Task<ActionResult<List<Phrase>>> GetListByPrefix(string strucCodePrefix)
    {
        List<Phrase> phraseList = await pBkr.GetListByPrefix(strucCodePrefix);
        return Ok(phraseList);
    }
}

