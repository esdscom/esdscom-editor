namespace eSDSCom.Editor.Shared.DocumentElements;
public class NumberFloatDataPoint : BaseEntity
{
    public NumberFloatDataPoint()
    {

    }

    [XmlText]
    public float Data { get; set; }

    [XmlIgnore]
    public string Restriction { get; set; }    // will have to be set in code for each occurence
}
