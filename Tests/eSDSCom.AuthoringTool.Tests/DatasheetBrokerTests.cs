using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSDSCom.Editor.Tests;

[Collection("BrokerTests")]
public class DatasheetBrokerTests : BaseTestData
{
    private readonly DatasheetBroker dsBkr;
    private readonly UserBroker uBkr;

    public DatasheetBrokerTests()
    {
        dsBkr = new(TestConnectionString);
        uBkr = new(TestConnectionString);
    }

    [Fact]
    public async Task GetDatasheet()
    {
        var orgGuid = Guid.NewGuid();
        var dsGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        var user = GetTestUser(userGuid, orgGuid);
        await uBkr.Add(user);     

        Datasheet ds = GetTestDatasheet(dsGuid, orgGuid, userGuid);
        var newDS = await dsBkr.Add(ds);

        Assert.Equal(dsGuid, newDS.Id);

        ds = await dsBkr.Get(orgGuid, dsGuid);
        Assert.NotNull(ds);
        Assert.Equal(dsGuid, newDS.Id);

        await dsBkr.Delete(dsGuid);
        await uBkr.Delete(userGuid);
    }

    [Fact]
    public async Task GetDatasheetMetadataOnly()
    {
        var orgGuid = Guid.NewGuid();
        var dsGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        var user = GetTestUser(userGuid, orgGuid);
        await uBkr.Add(user);
        Datasheet ds = GetTestDatasheet(dsGuid, orgGuid, userGuid);
        await dsBkr.Add(ds);

        ds = await dsBkr.GetMetadataOnly(dsGuid);
        Assert.NotNull(ds);
        Assert.Equal(dsGuid, ds.Id);
        Assert.True(string.IsNullOrEmpty(ds.DatasheetString));

        await dsBkr.Delete(dsGuid);
        await uBkr.Delete(userGuid);
    }

    [Fact]
    public async Task GetDatasheetsForOrganization()
    {

        var orgGuid = Guid.NewGuid();
        var dsGuid = Guid.NewGuid();
        var ds1Guid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        var user = GetTestUser(userGuid, orgGuid);
        await uBkr.Add(user);

        Datasheet ds = GetTestDatasheet(dsGuid, orgGuid, userGuid);
        var newDS = await dsBkr.Add(ds);
        Assert.Equal(newDS.Id, dsGuid);
       
        Datasheet ds1 = GetTestDatasheet(ds1Guid, orgGuid, userGuid);
        var newDS1 = await dsBkr.Add(ds1);
        Assert.Equal(newDS1.Id, ds1Guid);

        List<Datasheet> dsList = await dsBkr.GetForOrganization(orgGuid);
        Assert.NotNull(dsList);
        Assert.Equal(2, dsList.Count);

        await dsBkr.Delete(dsGuid);        
        await dsBkr.Delete(ds1Guid);
        await uBkr.Delete(userGuid);
    }

    [Fact]
    public async Task AddAndDeleteDatasheet()
    {
        var orgGuid = Guid.NewGuid();
        var dsGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        Datasheet ds = GetTestDatasheet(dsGuid, orgGuid, userGuid);
        var newDS = await dsBkr.Add(ds);
        Assert.Equal(newDS.Id, dsGuid);

        await dsBkr.Delete(dsGuid);
    }

    [Fact]
    public async Task UpdateDatasheet()
    {
        var orgGuid = Guid.NewGuid();
        var dsGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        Datasheet ds = GetTestDatasheet(dsGuid, orgGuid, userGuid);
        await dsBkr.Add(ds);

        ds.Name = "A New Name";
        ds.UserName = "A New User Name";
        var newDS = await dsBkr.Update(ds);
        Assert.Equal(newDS.Name, ds.Name);
        Assert.Equal(newDS.UserName, ds.UserName);

        await dsBkr.Delete(dsGuid);
    }

    [Fact]
    public async Task DeleteDatasheet()
    {
        var orgGuid = Guid.NewGuid();
        var dsGuid = Guid.NewGuid();
        var userGuid = Guid.NewGuid();

        Datasheet ds = GetTestDatasheet(dsGuid, orgGuid, userGuid);
        var newDS = await dsBkr.Add(ds);
        Assert.Equal(newDS.Id, dsGuid);

        bool deletedOK = await dsBkr.Delete(dsGuid);
        Assert.True(deletedOK);
    }
}
