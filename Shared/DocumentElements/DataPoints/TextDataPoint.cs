namespace eSDSCom.Editor.Shared.DocumentElements;
public class TextDataPoint : BaseEntity
{
    public TextDataPoint()
    {

    }

    public string Data { get; set; }


    public DateTime DateStamp { get; set; }

    [JsonIgnore]
    public string PermittedValues { get; set; }

    [JsonIgnore]
    public int MaxLength { get; set; } = 0;

    [JsonIgnore]
    public string RegexPattern { get; set; }
}

