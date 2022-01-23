namespace eSDSCom.Editor.Shared.DocumentElements;
public class UrlDataPoint : BaseEntity
{
    public UrlDataPoint()
    {

    }

    [XmlText]
    public string Data { get; set; }
}
