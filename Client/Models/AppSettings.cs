namespace eSDSCom.Editor.Client.Models;

public class AppSettings
{
    public AppSettings()
    {

    }

    public string ESDSphracCatalogueName { get; set; }

    public string ESDSPhraCVersion { get; set; }

    public string ESDSphracPhraseCatalogueId { get; set; }

    public string ESDSXmlVersion { get; set; }

    public List<string> Regions { get; set; }
}

