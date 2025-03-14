﻿using VbtEgitimKampiMVC.Infrastructure.Persistence.Repositories.Helper;

namespace VbtEgitimKampiMVC.Core.Domain.Entities;

public class User : Entity<int>
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; } // Example: Admin, Visitor, etc.
    public int RoleId { get; set; } // Example: Admin, Visitor, etc.
    public DateTime RegisteredDate { get; set; }
}
