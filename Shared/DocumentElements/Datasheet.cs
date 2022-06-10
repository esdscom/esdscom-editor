namespace eSDSCom.Editor.Shared.DocumentElements;
public class Datasheet : BaseModel
{
    public Datasheet()
    {

    }

    public Guid OrganizationId { get; set; }

    public Guid UserId { get; set; }

    //not in table
    public string UserName { get; set; }

    [MaxLength(250)]
    public string Name { get; set; }

    public int Status { get; set; }

    public string Comments { get; set; }

    public string DatasheetString { get; set; }

    public XmlDocument DatasheetXDoc { get; set; }

    public string RegionsString { get; set; }
   
    public List<Region> Regions { get; set; }

    public string MaterialType { get; set; }

}

