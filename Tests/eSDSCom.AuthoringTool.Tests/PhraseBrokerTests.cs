namespace eSDSCom.Editor.Tests;

[Collection("BrokerTests")]
public class PhraseBrokerTests : BaseTestData
{
    private readonly PhraseBroker broker;
    public PhraseBrokerTests()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        broker = new(memoryCache, TestConnectionString);
    }

    [Fact]
    public async Task GetPhraseTest()
    {
        Phrase testPhrase = GetTestPhrase();

        Phrase phrase = await broker.Get(testPhrase.StrucCode);
        Assert.Equal(testPhrase.StrucCode, phrase.StrucCode);
        Assert.Equal(testPhrase.English, phrase.English);
        Assert.Equal(testPhrase.German, phrase.German);
    }

    [Fact]
    public async Task GetPhraseByPrefixTest()
    {
        string prefix = "01.01.03.00";  //this uses 'StartsWith' functionality

        List<Phrase> phraseList = await broker.GetListByPrefix(prefix);
        Assert.Equal(10, phraseList.Count);
    }

}

