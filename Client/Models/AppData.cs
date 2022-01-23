namespace eSDSCom.Editor.Client.Models;

public class AppData
{
    public AppData()
    {

    }

    public List<BaseEntity> SchemaElements { get; set; }

    public List<ElementEnum> EnumItems { get; set; }

    public XmlDocument StarterDocument { get; set; }

    public List<Phrase> Phrases { get; set; } = new();

    public List<Region> Regions { get; set; } = new(); 

}

