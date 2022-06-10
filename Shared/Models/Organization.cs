namespace eSDSCom.Editor.Shared.Models;

public class Organization : BaseModel
{
    public Organization()
    {

    }

    public string OrganizationType { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string InfoExSysString { get; set; }

    public XmlDocument InfoExSysXDoc { get; set; }

}

