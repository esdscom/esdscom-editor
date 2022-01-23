namespace eSDSCom.Editor.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PhraseController : Controller
{
    private readonly PhraseBroker pBkr;
    public PhraseController(IMemoryCache cache)
    {
        pBkr = new(cache);
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

