﻿namespace IdentityWebApp.Areas.Admin.Models
{
    public class AssignRoleToUserViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Exist { get; set; }
    }
}
