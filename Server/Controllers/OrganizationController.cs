using eSDSCom.Editor.Shared;
using System.Net.Http;

namespace eSDSCom.Editor.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class OrganizationController : Controller
{
    private readonly OrganizationBroker oBkr;
    private IConfiguration Configuration;

    public OrganizationController(IConfiguration configuration)
    {
        Configuration = configuration;
        string connString = Configuration.GetConnectionString("LocalConnectionString");
        oBkr = new OrganizationBroker(connString);       
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

