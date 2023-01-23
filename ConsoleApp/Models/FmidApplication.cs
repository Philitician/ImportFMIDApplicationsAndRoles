using Microsoft.Graph;

namespace ConsoleApp.Models;

public class FmidApplication {
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();

    public Application ToAppRegistration() =>
        new()
        {
            DisplayName = Name,
            AppRoles = Roles.Select(role => role.ToAppRole()).ToList()
        };
}