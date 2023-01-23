using Microsoft.Graph;

namespace ConsoleApp.Models;

public class Role {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Value { get; set; }
    public int ApplicationId { get; set; }
    public FmidApplication FmidApplication { get; set; }
    
    public AppRole ToAppRole() {
        return new AppRole 
        {
            Id = Guid.NewGuid(),
            DisplayName = Value,
            Description = Description,
            Value = Value,
            AllowedMemberTypes = new []{ "User" }, // Need some way to determine the type of member upon creation
            IsEnabled = true,
            ODataType = "#microsoft.graph.appRole",
            Origin = "Application"
        };
    }
}