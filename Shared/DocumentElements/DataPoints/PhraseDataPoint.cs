namespace eSDSCom.Editor.Shared.DocumentElements;
public class PhraseDataPoint : BaseEntity
{
    public PhraseDataPoint()
    {

    }

    [XmlIgnore]
    public List<Phrase> Phrases { get; set; }

    [XmlText]
    public string PhraseText
    {
        get
        {
            var tmp = Phrases.Select(p => p.English).ToArray();
            return string.Join(",", tmp);
        }
    }
}
