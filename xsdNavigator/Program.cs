using System.Xml.Linq;
using System.Xml.Serialization;

Console.WriteLine("Started");

//=========================================================================================================
// this is the 5.4 set of files - need to watch these for adds/drops

List<string> schemas = new();

schemas.Add( "c:/temp/xml/SDSComXML.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLCT.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLDT.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLDT_GHS.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_AR.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_BR.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_CA.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_CH.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_DE.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_DK.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_EU.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_JP.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_MX.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_NO.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_RU.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_TR.xsd");
schemas.Add( "c:/temp/xml/SDSComXMLNE_US.xsd");

var entityList = new List<Elem>();
var baseEntityList = new ElemList();

baseEntityList.Elems = new List<Elem>();

var enumEntities = new EnumList();
enumEntities.EnumItems = new();

foreach (string schema in schemas)
{
    var doc = XDocument.Load(schema);

    //==========================================================================================================================================================
    var elemList = (from e in doc.Descendants().Elements() select e).ToList();

   
    foreach (var elem in elemList)
    {
       

        if (elem is not null)
        {
            string elemName = elem.Attribute("name")?.Value;

            if (!string.IsNullOrEmpty(elemName))
            {
                Elem entity = new();
                entity.Name = elemName;
                entity.Min = elem.Attribute("minOccurs")?.Value;
                entity.Max = elem.Attribute("maxOccurs")?.Value;
                entity.Type = elem.Attribute("type")?.Value;
                entity.Source = schema.Replace(@"c:/temp/xml/","");

                var tmp2 = (from e in elem.Descendants().Elements() select e).Where(a => a.Name.LocalName == "documentation").FirstOrDefault();            

                if (tmp2 is not null)
                {
                    entity.Doc = tmp2.Value;
                }

                entityList.Add(entity);
            }

            //======================================================================================
                      
            XNamespace xs = doc.Root?.Name.Namespace;

            var enumerations = doc
               .Root?
               .Descendants(xs + "simpleType")
                .Where(t => (string)t.Attribute("name") == elemName)
               .Descendants(xs + "enumeration")
               .ToList();

            string enumValue = string.Empty;

            foreach (var element in enumerations)
            {                
                enumValue += $"{element.FirstAttribute.Value},";

            }

            if (!string.IsNullOrEmpty(enumValue) && !string.IsNullOrEmpty(elemName))
            {
                var enumEntity = new EnumItem
                {
                    Name = elemName,
                    Values = enumValue.TrimEnd(", ".ToCharArray())
                };

                var enumDoc = doc
                      .Root?
                      .Descendants(xs + "simpleType")
                       .Where(t => (string)t.Attribute("name") == elemName)
                      .Descendants(xs + "documentation")
                      .FirstOrDefault();
                if (enumDoc is not null)
                {
                    enumEntity.Comments = enumDoc.Value;
                }

                enumEntities.EnumItems.Add(enumEntity);
            }
        }
    }

}

baseEntityList.Elems = entityList;

StreamWriter writer = new (@"c:\temp\SchemaElements.xml", false);
XmlSerializer ser = new (baseEntityList.GetType());
ser.Serialize(writer, baseEntityList);
writer.Close();

StreamWriter enumwriter = new(@"c:\temp\EnumValues.xml", false);
XmlSerializer enumSer = new (enumEntities.GetType());
enumSer.Serialize(enumwriter, enumEntities);
enumwriter.Close();

Console.WriteLine("Finished");

return;

public class EnumList
{
    public List<EnumItem> EnumItems { get; set; }
}

public class EnumItem
{
    public string Name { get; set; }
    public string Comments {  get; set; }   
    public string Values { get; set; }
}


public class ElemList
{    
    public List<Elem> Elems { get; set; }
}


public class Elem
{
    public Elem()
    {

    }

    public string Name { get; set; }
    public string Min { get; set; }
    public string Max { get; set; }
    public string Type { get; set; }
    public string Source { get; set; }
    public string Doc { get; set; }
}

