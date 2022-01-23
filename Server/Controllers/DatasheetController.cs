

namespace eSDSCom.Editor.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class DatasheetController : Controller
{
    private readonly DatasheetBroker dBkr;


    public DatasheetController()
    {
        dBkr = new DatasheetBroker();
    }

    [HttpGet]
    [Route("Get")]
    public async Task<ActionResult<Datasheet>> Get(Guid organizationId, Guid sheetId)
    {
        return await dBkr.Get(organizationId, sheetId);
    }

    [HttpGet]
    [Route("GetForOrganization")]
    public async Task<ActionResult<List<Datasheet>>> GetForOrganization(Guid organizationId)
    {
        return await dBkr.GetForOrganization(organizationId);
    }

    //[HttpGet]
    //[Route("GetForOrganizationAndDocset")]
    //public async Task<ActionResult<List<Datasheet>>> GetForOrganizationAndDocSet(string organizationKey, string docSetId)
    //{
    //    return await dSvc.GetForOrganizationAndDocSet(organizationKey, docSetId);
    //}


    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<Datasheet>> Add(Datasheet datasheet)
    {
        return await dBkr.Add(datasheet);
    }

    [HttpPut]
    [Route("Update")]
    public async Task<ActionResult<Datasheet>> Update(Datasheet datasheet)
    {
        return await dBkr.Update(datasheet);
    }

    [HttpDelete]
    [Route("Delete")]
    public async Task<ActionResult<bool>> Delete(Guid dsId)
    {
        return await dBkr.Delete(dsId);
    }
}

