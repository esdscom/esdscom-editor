namespace eSDSCom.Editor.Shared.DocumentElements;

public class DatasheetFeedItem
{
    public DatasheetFeedItem()
    {

    }

    public Guid DatasheetFeedId { get; set; }

    public Guid DatasheetId { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Datasheet Datasheet { get; set; }

}

