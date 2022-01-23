namespace eSDSCom.Editor.Tests;

[Collection("BrokerTests")]
public class DatasheetFeedBrokerTests : BaseTestData
{
    readonly DatasheetFeedBroker dsfBkr;
    readonly UserBroker uBkr;


    public DatasheetFeedBrokerTests()
    {
        dsfBkr = new(TestConnectionString);
        uBkr = new(TestConnectionString);
    }

    [Fact]
    public async Task GetDatasheetFeed()
    {
        var dsfGuid = Guid.NewGuid();
        var orgGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        await uBkr.Add(GetTestUser(userGuid, orgGuid));

        DatasheetFeed dsf = GetTestDatasheetFeed(dsfGuid, orgGuid, userGuid);
        await dsfBkr.Add(dsf);

        var newdsf = await dsfBkr.Get(orgGuid,dsfGuid);
        Assert.NotNull(newdsf);
        Assert.Equal(dsfGuid, newdsf.Id);

        await dsfBkr.Delete(dsfGuid);
    }

    [Fact]
    public async Task AddAndDeleteDatasheetFeed()
    {
        var dsfGuid = Guid.NewGuid();
        var orgGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        DatasheetFeed dsf = GetTestDatasheetFeed(dsfGuid, orgGuid, userGuid);
        var newDSF = await dsfBkr.Add(dsf);
        Assert.Equal(newDSF.Id, dsfGuid);

        bool deletedOk = await dsfBkr.Delete(dsfGuid);
        Assert.True(deletedOk);
    }

    [Fact]
    public async Task UpdateDatasheetFeed()
    {
        var dsfGuid = Guid.NewGuid();
        var orgGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        DatasheetFeed dsf = GetTestDatasheetFeed(dsfGuid, orgGuid, userGuid);
        await dsfBkr.Add(dsf);

        dsf.Name = "A New Name";
        dsf.UserName = "A New User Name";
        var newDSF = await dsfBkr.Update(dsf);
        Assert.Equal(newDSF.Name, dsf.Name);
        Assert.Equal(newDSF.UserName, dsf.UserName);

        await dsfBkr.Delete(dsfGuid);
    }

    [Fact]
    public async Task DeleteDatasheetFeed()
    {
        var dsfGuid = Guid.NewGuid();
        var orgGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        DatasheetFeed dsf = GetTestDatasheetFeed(dsfGuid, orgGuid, userGuid);
        await dsfBkr.Add(dsf);

        bool deletedOK = await dsfBkr.Delete(dsfGuid);
        Assert.True(deletedOK);
    }
}