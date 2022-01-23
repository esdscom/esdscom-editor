namespace eSDSCom.Editor.Shared.DocumentElements;
public class DatasheetFeed : BaseModel
{
    public DatasheetFeed()
    {

    }

    public Guid OrganizationId { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; }

    [MaxLength(250)]
    public string Name { get; set; }

    public int Status { get; set; }

    public string Comments { get; set; }

    public string DatasheetFeedString { get; set; }

    public XmlDocument DatasheetFeedXDoc { get; set; }

    public List<DatasheetFeedItem> DatasheetItems { get; set; }

    public int DatasheetCount { get; set; }
}

