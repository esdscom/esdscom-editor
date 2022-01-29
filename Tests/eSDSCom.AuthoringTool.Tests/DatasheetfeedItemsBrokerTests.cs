namespace eSDSCom.Editor.Tests;

[Collection("BrokerTests")]
public class DatasheetfeedItemsBrokerTests : BaseTestData
{
    private readonly DatasheetFeedItemBroker dsfiBkr;

    public DatasheetfeedItemsBrokerTests()
    {
        dsfiBkr = new(TestConnectionString);
    }

    [Fact]
    public async Task GetDatasheetFeedItem()
    {
        var dsfGuid = Guid.NewGuid();
        var dsGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        DatasheetFeedItem dsfi = GetTestDatasheetFeedItem(dsfGuid, dsGuid,userGuid);
        await dsfiBkr.Add(dsfi); 

        dsfi = await dsfiBkr.Get(dsfGuid, dsGuid);
        Assert.NotNull(dsfi);

        await dsfiBkr.Delete(dsfGuid, dsGuid);
    }

    [Fact]
    public async Task GetDatasheetFeedItemsForDatasheetFeed()
    {
        var dsfGuid = Guid.NewGuid();
        var dsGuid = Guid.NewGuid();
        var ds1Guid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        DatasheetFeedItem dsfi = GetTestDatasheetFeedItem(dsfGuid, dsGuid, userGuid);
        await dsfiBkr.Add(dsfi);

        DatasheetFeedItem dsfi1 = GetTestDatasheetFeedItem(dsfGuid, ds1Guid, userGuid);
        await dsfiBkr.Add(dsfi1);

        List<DatasheetFeedItem> dsfiList = await dsfiBkr.GetForDatasheetFeed(dsfGuid);
        Assert.NotNull(dsfiList);
        Assert.Equal(2,dsfiList.Count);

        await dsfiBkr.Delete(dsfGuid, dsGuid);
        await dsfiBkr.Delete(dsfGuid, ds1Guid);
    }

    [Fact]
    public async Task AddAndDeleteDatasheetFeedItem()
    {
        var dsfGuid = Guid.NewGuid();
        var dsGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        DatasheetFeedItem dsfi = GetTestDatasheetFeedItem(dsfGuid, dsGuid, userGuid);

        var newDSFI = await dsfiBkr.Add(dsfi);

        Assert.Equal(newDSFI.DatasheetFeedId, dsfGuid);
        Assert.Equal(newDSFI.DatasheetId, dsGuid);

        bool deletedOK = await dsfiBkr.Delete(dsfGuid, dsGuid);

        Assert.True(deletedOK);
    } 
}

