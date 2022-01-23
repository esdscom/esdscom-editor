
namespace eSDSCom.Editor.Shared.Models;

public class Phrase
{

    /*
        INSERT INTO PHRASES
            (STRUCCODE, XPATH,REGION,ORIGCODE,ENGLISH,GERMAN,REVDATE,SOURCE,INFO,ORIGIN)
        SELECT
           MY_XML.Phrase.query('StrucCode').value('.', 'NVARCHAR(100)'),
           MY_XML.Phrase.query('XPath').value('.', 'NVARCHAR(250)'),
           MY_XML.Phrase.query('Region').value('.', 'NVARCHAR(50)'),
           MY_XML.Phrase.query('OrigCode').value('.', 'NVARCHAR(50)'),
           MY_XML.Phrase.query('English').value('.', 'NVARCHAR(max)'),
           MY_XML.Phrase.query('German').value('.', 'NVARCHAR(max)'),
           MY_XML.Phrase.query('RevDate').value('.', 'NVARCHAR(50)'),
           MY_XML.Phrase.query('Source').value('.', 'NVARCHAR(250)'),
           MY_XML.Phrase.query('Info').value('.', 'NVARCHAR(250)'),
           MY_XML.Phrase.query('Origin').value('.', 'NVARCHAR(250)')
        FROM (SELECT CAST(MY_XML AS xml)
              FROM OPENROWSET(BULK 'C:\Temp\Phrases.xml', SINGLE_BLOB) AS T(MY_XML)) AS T(MY_XML)
              CROSS APPLY MY_XML.nodes('PhraseList/PhraseItems/Phrase') AS MY_XML (Phrase);
*/

    public Phrase()
    {

    }

    public string StrucCode { get; set; }
    public string XPath { get; set; }
    public string Region { get; set; }
    public string OrigCode { get; set; }
    public string English { get; set; }
    public string German { get; set; }
    public string RevDate { get; set; }
    public string Source { get; set; }
    public string Info { get; set; }
    public string Origin { get; set; }
}

