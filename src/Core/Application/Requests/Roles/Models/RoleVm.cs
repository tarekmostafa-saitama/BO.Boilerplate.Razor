using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Roles.Models;

public class RoleVm
{
    public string Id { get; set; }
    public string Name { get; set; }


    public List<string> Permissions { get; set; }
}