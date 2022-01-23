using User = eSDSCom.Editor.Shared.Models.User;

namespace eSDSCom.Editor.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly UserBroker uBkr;
    public UserController()
    {
        uBkr = new UserBroker();
    }

    [HttpGet]
    [Route("Get")]
    public async Task<ActionResult<User>> Get(Guid id)
    {
        User user = await uBkr.Get(id);
        return Ok(user);
    }

    //[HttpGet]
    //[Route("GetByEmail")]
    //public async Task<ActionResult<User>> GetByEmail(string email)
    //{
    //    User user = await uSvc.GetByEmail(email);

    //    if (user == null)
    //    {
    //        user = new();
    //    }

    //    return Ok(user);
    //}

    [HttpGet]
    [Route("GetForOrganization")]
    public async Task<ActionResult<List<User>>> GetForOrganization(Guid organizationId, bool activeOnly = true)
    {
          List<User> userList = await uBkr.GetForOrganization(organizationId, activeOnly);
        return Ok(userList);
    }

    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<User>> Add(User user)
    {
        user = await uBkr.Add(user);
        return Ok(user);
    }

    [HttpPut]
    [Route("Update")]
    public async Task<ActionResult<User>> Update(User user)
    {
        user = await uBkr.Update(user);
        return Ok(user);
    }
}

