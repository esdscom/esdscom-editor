namespace eSDSCom.Editor.Shared.Models;

public class User : BaseModel
{
    public User() { }
    public Guid OrganizationId { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public int Role { get; set; }

    public bool IsActive { get; set; }
}

