namespace eSDSCom.Editor.Client.Services;

public static class Utils
{
    #region base entities operations

    /// <summary>
    /// read the elements extracted from the various xsd files
    /// into basentities for display in various datapoint components
    /// </summary>
    /// <returns></returns>
    public static List<BaseEntity> GetSchemaElements()
    {
        List<BaseEntity> returnList = new();
        var elemList = ReadSchemaElementsResource();
        var sourceList = elemList.Elems;

        sourceList.ForEach(s =>
        {
            returnList.Add(new BaseEntity()
            {
                NodeName = s.Name,
                Occurs = GetOccursValue(s.Min, s.Max),
                Comments = s.Doc,
                Type = string.IsNullOrEmpty(s.Type) ? s.Type : s.Type.Replace("eSDScom:", string.Empty)
            }
        );
        });

        return returnList;
    }

    /// <summary>
    /// read the enums and their associated values from the various xsd files
    /// into an object for use in textdatapoint and  components
    /// </summary>
    /// <returns></returns>
    public static List<ElementEnum> GetSchemaEnums()
    {
        List<ElementEnum> returnList = new();
        var enumList = ReadSchemaEnumsResource();
        var sourceList = enumList.EnumItems;

        sourceList.ForEach(s =>
        {
            returnList.Add(new ElementEnum()
            {
                Name = s.Name,
                Values = s.Values,
                Comments = s.Comments
            }
        );
        });

        return returnList;
    }

    public static List<Region> GetRegions()
    {
        List<Region> regionList = new();

        regionList.Add(new Region { Name = "Argentina", Suffix = "AR" });
        regionList.Add(new Region { Name = "Austria", Suffix = "AT" });
        regionList.Add(new Region { Name = "Brazil", Suffix = "BR" });
        regionList.Add(new Region { Name = "Canada", Suffix = "CA" });
        regionList.Add(new Region { Name = "Switzerland", Suffix = "CH" });
        regionList.Add(new Region { Name = "Germany", Suffix = "DE" });
        regionList.Add(new Region { Name = "Denmark", Suffix = "DK" });
        regionList.Add(new Region { Name = "Europe", Suffix = "EU" });
        regionList.Add(new Region { Name = "Japan", Suffix = "JP" });
        regionList.Add(new Region { Name = "Mexico", Suffix = "MX" });
        regionList.Add(new Region { Name = "Norway", Suffix = "NO" });
        regionList.Add(new Region { Name = "Russia", Suffix = "RU" });
        regionList.Add(new Region { Name = "Turkey", Suffix = "TR" });
        regionList.Add(new Region { Name = "United States", Suffix = "US" });

        return regionList;
    }


    public static List<Region> GetRegionsFromString(string selectedRegions)
    {
        List<Region> retRegions = new();

        List<Region> regions = GetRegions();

        var regionArray = selectedRegions.Split(",");

        foreach (var region in regionArray)
        {
            var reg = regions.First(r => r.Suffix == region);
            retRegions.Add(reg);
        }
        return retRegions;
    }

    public static List<Phrase> GetPhrases()
    {
        var phraseList = ReadPhraseResource();

        return phraseList;
    }


    private static ElemList ReadSchemaElementsResource()
    {
        var list = new ElemList();

        try
        {
            var myAssembly = Assembly.GetExecutingAssembly();
            string name = $"eSDSCom.Editor.Client.Data.SchemaElements.xml";
            using Stream stream = myAssembly.GetManifestResourceStream(name);
            XmlSerializer ser = new(list.GetType());
            list = (ElemList)ser.Deserialize(stream);
        }
        catch (Exception ex)
        {
            Console.Write(ex.StackTrace);
            throw;
        }

        return list;
    }

    private static List<Phrase> ReadPhraseResource()
    {
        PhraseList list = new();
        List<Phrase> returnList = new();
        try
        {
            var myAssembly = Assembly.GetExecutingAssembly();
            string name = $"eSDSCom.Editor.Client.Data.Phrases.xml";
            using Stream stream = myAssembly.GetManifestResourceStream(name);
            XmlSerializer ser = new(list.GetType());
            var phraseList = (PhraseList)ser.Deserialize(stream);
            returnList = phraseList.PhraseItems;
        }
        catch (Exception ex)
        {
            Console.Write(ex.StackTrace);
            throw;
        }

        return returnList;
    }

    private static EnumList ReadSchemaEnumsResource()
    {
        var list = new EnumList();

        try
        {
            var myAssembly = Assembly.GetExecutingAssembly();
            string name = $"eSDSCom.Editor.Client.Data.EnumValues.xml";
            using Stream stream = myAssembly.GetManifestResourceStream(name);
            XmlSerializer ser = new(list.GetType());
            list = (EnumList)ser.Deserialize(stream);
        }
        catch (Exception ex)
        {
            Console.Write(ex.StackTrace);
            throw;
        }

        return list;
    }

    public static XmlDocument GetDocumentSetXml()
    {
        XmlDocument xDoc = new();

        try
        {
            var myAssembly = Assembly.GetExecutingAssembly();
            string name = $"eSDSCom.Editor.Client.Data.5.4.1-starter.xml";
            using Stream stream = myAssembly.GetManifestResourceStream(name);
            xDoc.Load(stream);
        }
        catch (Exception ex)
        {
            Console.Write(ex.StackTrace);
            throw;
        }

        return xDoc;
    }

    public static XmlDocument GetDatasheetXml()
    {
        XmlDocument xDoc = new();

        try
        {
            var myAssembly = Assembly.GetExecutingAssembly();
            string name = $"eSDSCom.Editor.Client.Data.5.4.1-starter.xml";
            using Stream stream = myAssembly.GetManifestResourceStream(name);
            xDoc.Load(stream);
            string dataSheetXML = xDoc.DocumentElement.SelectSingleNode("Datasheet").OuterXml;
            xDoc.LoadXml(dataSheetXML);
        }
        catch (Exception ex)
        {
            Console.Write(ex.StackTrace);
            throw;
        }

        return xDoc;
    }

    public static XmlDocument GetInfoExpSysXml()
    {
        XmlDocument xDoc = new();

        try
        {
            var myAssembly = Assembly.GetExecutingAssembly();
            string name = $"eSDSCom.Editor.Client.Data.InformationFromExportingSystem.xml";
            using Stream stream = myAssembly.GetManifestResourceStream(name);
            xDoc.Load(stream);
        }
        catch (Exception ex)
        {
            Console.Write(ex.StackTrace);
            throw;
        }

        return xDoc;
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

    public class EnumList
    {
        public List<EnumItem> EnumItems { get; set; }
    }

    public class EnumItem
    {
        public EnumItem()
        {

        }
        public string Name { get; set; }
        public string Comments { get; set; }
        public string Values { get; set; }
    }

    public class PhraseList
    {
        public List<Phrase> PhraseItems { get; set; }
    }

    #endregion





    public static string GetOccursText(Enums.DataPointOccurence occursValue, bool IsPhrase = false)
    {
        string occurs = occursValue switch
        {
            Enums.DataPointOccurence.RequiredExactlyOnce => "Required - exactly one value",
            Enums.DataPointOccurence.OptionalZeroOrOne => "Optional -  zero or one values",
            Enums.DataPointOccurence.RequiredOnceOrMore => "Required - one or more values",
            Enums.DataPointOccurence.OptionalZeroOrMore => "Optional -  zero or more values",
            _ => "*** No occur setting has been defined *** ",
        };

        if (IsPhrase)
        {
            occurs = occurs.Replace("value", "phrase");
        }
        return occurs;
    }

    private static Enums.DataPointOccurence GetOccursValue(string min, string max)
    {

        /*

            * <xsd:element name="A"/>
            means A is required and must appear exactly once.

            <xsd:element name="A" minOccurs="0"/>
            means A is optional and may appear at most once.

            <xsd:element name="A" maxOccurs="unbounded"/>
            means A is required and may repeat an unlimited number of times.

            <xsd:element name="A" minOccurs="0" maxOccurs="unbounded"/>
            means A is optional and may repeat an unlimited number of times.
         */


        Enums.DataPointOccurence retVal = Enums.DataPointOccurence.OptionalZeroOrMore;

        //============================================================================================
        //  RequiredExactlyOnce conditions

        if (string.IsNullOrEmpty(min) && string.IsNullOrEmpty(max))
        {
            retVal = Enums.DataPointOccurence.RequiredExactlyOnce;
            return retVal;
        }

        if (min == "1" && max == "1")
        {
            retVal = Enums.DataPointOccurence.RequiredExactlyOnce;
            return retVal;
        }

        if (string.IsNullOrEmpty(min) && max == "1")
        {
            retVal = Enums.DataPointOccurence.RequiredExactlyOnce;
            return retVal;
        }

        if (min == "1" && max == "1")
        {
            retVal = Enums.DataPointOccurence.RequiredExactlyOnce;
            return retVal;
        }

        //============================================================================================
        //  RequiredOneOrMore conditions

        if (string.IsNullOrEmpty(min) && max == "unbounded")
        {
            retVal = Enums.DataPointOccurence.RequiredOnceOrMore;
            return retVal;
        }

        if (min == "1" && max == "unbounded")
        {
            retVal = Enums.DataPointOccurence.RequiredOnceOrMore;
            return retVal;
        }


        //============================================================================================
        //  OptionalZeroOrOne conditions

        if (min == "0" && max == "1")
        {
            retVal = Enums.DataPointOccurence.OptionalZeroOrOne;
            return retVal;
        }

        if (min == "0" && string.IsNullOrEmpty(max))
        {
            retVal = Enums.DataPointOccurence.OptionalZeroOrOne;
            return retVal;
        }

        //============================================================================================
        //  OptionalZeroOrMore conditions

        if (min == "0" && max == "unbounded")
        {
            retVal = Enums.DataPointOccurence.OptionalZeroOrMore;
            return retVal;
        }


        //Console.WriteLine($"Value not handled-> min: {min}, max: {max}");
        return retVal;



    }

    public static string SplitCamelCase(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
    }

    public static string GetStatusText(int status)
    {
        return status switch
        {
            1 => "Draft",
            2 => "Published",
            3 => "Archived",
            _ => "Not Specified",
        };
    }

    public static string GetSectionNumber(string sectionName)
    {
        string retVal = "";

        switch (sectionName)
        {
            case "IdentificationSubstPrep":
                retVal = "01";
                break;
            case "HazardIdentification":
                retVal = "02";
                break;
            case "Composition":
                retVal = "03";
                break;
            case "FirstAidMeasures":
                retVal = "04";
                break;
            case "FireFightingMeasures":
                retVal = "05";
                break;
            case "AccidentalReleaseMeasures":
                retVal = "06";
                break;
            case "HandlingAndStorage":
                retVal = "07";
                break;
            case "ExposureControlPersonalProtection":
                retVal = "08";
                break;
            case "PhysicalChemicalProperties":
                retVal = "09";
                break;
            case "StabilityReactivity":
                retVal = "10";
                break;
            case "ToxicologicalInformation":
                retVal = "11";
                break;
            case "EcologicalInformation":
                retVal = "12";
                break;
            case "DisposalConsiderations":
                retVal = "13";
                break;
            case "TransportInformation":
                retVal = "14";
                break;
            case "RegulatoryInformation":
                retVal = "15";
                break;
            case "OtherInformation":
                retVal = "16";
                break;
        }



        return retVal;
    }
}

