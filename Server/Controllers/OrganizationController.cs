using eSDSCom.Editor.Shared;
using System.Net.Http;

namespace eSDSCom.Editor.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class OrganizationController : Controller
{
    private readonly OrganizationBroker oBkr;

    public OrganizationController()
    {
        oBkr = new OrganizationBroker();
    }

    [HttpGet]
    [Route("Get")]
    public async Task<ActionResult<Organization>> Get(Guid id)
    {
        return await oBkr.Get(id);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<Organization>> Add(Organization org)
    { 
        return await oBkr.Add(org);
    }

    [HttpPut]
    [Route("Update")]
    public async Task<ActionResult<Organization>> Update(Organization organization)
    {
        return await oBkr.Update(organization);
    }
}

