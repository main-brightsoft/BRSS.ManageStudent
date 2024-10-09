using BRSS.ManageStudent.Domain.Entity.Base;
using Microsoft.AspNetCore.Identity;

namespace BRSS.ManageStudent.Domain.Entity;

public class ApplicationUser:IdentityUser, IEntity<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string GetId()
    {
        return Id;
    }

    public void SetId(string id)
    {
        this.Id = id;
    }
}