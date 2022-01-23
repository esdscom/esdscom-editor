namespace eSDSCom.Editor.Shared.DocumentElements;

public class DateDataPoint : BaseEntity
{
    public DateDataPoint()
    {

    }

    [XmlText]
    public DateTime DateValue { get; set; }

    [XmlIgnore]
    public DateTime DateStamp { get; set; }
}

