namespace eSDSCom.Editor.Tests;

[Collection("BrokerTests")]
public class OrganizationBrokerTests : BaseTestData
{
    private readonly OrganizationBroker oBkr;

    public OrganizationBrokerTests() 
    {
        oBkr = new(TestConnectionString);
    }


    [Fact]
    public async Task GetOrganization()
    {
        var orgGuid = Guid.NewGuid();

        Organization org = GetTestOrganization(orgGuid);
        await oBkr.Add(org);

        org = await oBkr.Get(orgGuid);
        Assert.NotNull(org);

        await oBkr.Delete(orgGuid);
    }


    [Fact]
    public async Task AddAndDeleteOrganization()
    {
        var orgGuid = Guid.NewGuid();

        Organization org = GetTestOrganization(orgGuid);
        var newOrg = await oBkr.Add(org);
        Assert.Equal(newOrg.Id, orgGuid);
        
        var deletedOk = await oBkr.Delete(orgGuid);
        Assert.True(deletedOk);
    }

    [Fact]
    public async Task UpdateOrganization()
    {
        var orgGuid = Guid.NewGuid();

        Organization org = GetTestOrganization(orgGuid);
        await oBkr.Add(org);

        org.Name = "A New Name";
        org.Address = "A New Address";
        var newOrg = await oBkr.Update(org);
        Assert.Equal(newOrg.Name, org.Name);
        Assert.Equal(newOrg.Address, org.Address);

        await oBkr.Delete(orgGuid);

    }

    [Fact]
    public async Task DeleteOrganization()
    {
        var orgGuid = Guid.NewGuid();

        Organization org = GetTestOrganization(orgGuid);
        await oBkr.Add(org);

        bool deletedOK = await oBkr.Delete(orgGuid);
        Assert.True(deletedOK);
    }
}

