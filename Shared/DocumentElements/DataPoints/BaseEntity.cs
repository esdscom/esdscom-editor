namespace eSDSCom.Editor.Shared.DocumentElements;
public class BaseEntity
{
    [JsonIgnore]
    public string NodeName { get; set; }

    [JsonIgnore]
    public string DisplayName
    {
        get
        {
            string tmp = string.Empty;
            if (!string.IsNullOrEmpty(this.NodeName))
            {
                tmp = Regex.Replace(this.NodeName, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
            }

            return tmp;
        }
    }

    [JsonIgnore]
    public Enums.DataPointOccurence Occurs { get; set; } = Enums.DataPointOccurence.OptionalZeroOrOne;

    [JsonIgnore]
    public string Comments { get; set; }

    [JsonIgnore]
    public string Type { get; set; }

    [JsonIgnore]
    public string TypeComments { get; set; }
}
