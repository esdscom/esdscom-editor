namespace eSDSCom.Editor.Shared.DocumentElements;

public class BooleanDataPoint : BaseEntity
{
    public BooleanDataPoint()
    {

    }

    [XmlText]
    public bool BoolValue { get; set; }

}
