
namespace eSDSCom.Editor.Tests;

[Collection("BrokerTests")]
public class SubstanceBrokerTests : BaseTestData
{
    private readonly SubstanceBroker broker;
    public SubstanceBrokerTests()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        broker = new(memoryCache, TestConnectionString);
    }

    [Fact]
    public async Task GetByIDSubstanceTest()
    {
        Substance subs = GetTestSubstance();

        Substance newsubs = await broker.Get(subs.Id);
        Assert.NotNull(newsubs);
        Assert.Equal(subs.SubstanceId, newsubs.SubstanceId);

    }

    [Fact]
    public async Task GetAllSubstancesTest()
    {
        List<Substance> subsList = await broker.GetAll();
        Assert.NotNull(subsList);
        Assert.Equal(9999, subsList.Count);

    }

    [Fact]
    public async Task GetByGuidSubstanceTest()
    {
        Substance subs = GetTestSubstance();

        Substance newsubs = await broker.GetById(subs.SubstanceId);
        Assert.NotNull(newsubs);
        Assert.Equal(subs.SubstanceId, newsubs.SubstanceId);
        Assert.Equal(subs.Name, newsubs.Name);
    }


    [Fact]
    public async Task GetByECNumberSubstanceTest()
    {
        Substance subs = GetTestSubstance();

        Substance newsubs = await broker.GetByECNumber(subs.ECNumber);
        Assert.NotNull(newsubs);
        Assert.Equal(subs.Name, newsubs.Name);
    }

    [Fact]
    public async Task GetByCASNumberSubstanceTest()
    {
        Substance subs = GetTestSubstance();

        Substance newsubs = await broker.GetByCASNumber(subs.CASNumber);
        Assert.NotNull(newsubs);
        Assert.Equal(subs.Name, newsubs.Name);
    }

    [Fact]
    public async Task GetList()
    {
        List<string> substanceIDs = new();
        substanceIDs.Add("100.003.133");
        substanceIDs.Add("100.337.580");
        substanceIDs.Add("100.101.684");

        List<Substance> subsList = await broker.GetList(substanceIDs);
        Assert.NotNull(subsList);
        Assert.Equal(7, subsList.Count);
    }


}

