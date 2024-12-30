using Microsoft.AspNetCore.Identity;

namespace WebTechLabs.DAL.Entities;

public class ApplicationUser : IdentityUser
{
    public byte[]? AvatarImage { get; set; }
}