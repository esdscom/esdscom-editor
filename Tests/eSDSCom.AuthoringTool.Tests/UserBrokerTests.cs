namespace eSDSCom.Editor.Tests;

[Collection("BrokerTests")]
public class UserBrokerTests : BaseTestData
{
    private readonly UserBroker uBkr;
    public UserBrokerTests() 
    {
        uBkr = new(TestConnectionString);
    }

    [Fact]
    public async Task GetUser()
    {
        var userGuid1 = Guid.NewGuid();
        var orgGuid = Guid.NewGuid();

        var newUser = GetTestUser(userGuid1, orgGuid);
        var user = await uBkr.Add(newUser);
        Assert.Equal(user.Id, userGuid1);

        newUser = await uBkr.Get(userGuid1);
        Assert.NotNull(newUser);

        await uBkr.Delete(userGuid1);
    }

    [Fact]
    public async Task GetUsersForOrganization()
    {
        var userGuid1 = Guid.NewGuid();
        var userGuid2 = Guid.NewGuid();
        var orgGuid = Guid.NewGuid();

        var newUser1 = GetTestUser(userGuid1, orgGuid);
        await uBkr.Add(newUser1);
               
        var newUser2 = GetTestUser(userGuid2, orgGuid);
        await uBkr.Add(newUser2);

        var userList = await uBkr.GetForOrganization(orgGuid, true);
        Assert.NotNull(userList);
        Assert.Equal(2, userList.Count);

        await uBkr.Delete(userGuid1);
        await uBkr.Delete(userGuid2);
    }


    [Fact]
    public async Task AddAndDeleteUser()
    {
        var userGuid = Guid.NewGuid();
        var orgGuid = Guid.NewGuid();

        var newUser = GetTestUser(userGuid, orgGuid);  
        var user = await uBkr.Add(newUser);
        Assert.Equal(user.Id, userGuid);
        
        await uBkr.Delete(userGuid);
    }


    [Fact]
    public async Task UpdateUser()
    {
        var userGuid = Guid.NewGuid();
        var orgGuid = Guid.NewGuid();

        var newUser = GetTestUser(userGuid, orgGuid);
        await uBkr.Add(newUser);

        newUser.Name = "Updated User Name";
        newUser.Role = 2;
        var user = await uBkr.Update(newUser);
        Assert.Equal(user.Name, newUser.Name);
        Assert.Equal(user.Role, newUser.Role);

        await uBkr.Delete(userGuid);
    }

    [Fact]
    public async Task DeleteUser()
    {
        var userGuid = Guid.NewGuid();
        var orgGuid = Guid.NewGuid();

        var newUser = GetTestUser(userGuid, orgGuid);
        await uBkr.Add(newUser);

        bool deletedOK = await uBkr.Delete(userGuid);
        Assert.True(deletedOK);
    }
}

