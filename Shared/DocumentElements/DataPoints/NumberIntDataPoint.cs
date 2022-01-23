namespace eSDSCom.Editor.Shared.DocumentElements;
public class NumberIntDataPoint : BaseEntity
{
    public NumberIntDataPoint()
    {

    }

    [XmlText]
    public int Data { get; set; }

    [XmlIgnore]
    public string Restriction { get; set; }    // will have to be set in code for each occurence
}
