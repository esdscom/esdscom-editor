namespace eSDSCom.Editor.Shared.DocumentElements;
public class ListTextDataPoint : BaseEntity
{
    public ListTextDataPoint()
    {

    }

    public string Data { get; set; }

    public DateTime DateStamp { get; set; }

    public string PermittedValues { get; set; }

    public string RegexPattern { get; set; }

}

