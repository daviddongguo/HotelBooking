﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace david.hotelbooking.domain.Entities.RBAC
{
    public class RolePermission
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int PermissionId { get; set; }

        [ForeignKey("RoleId")]
        [JsonIgnore]
        public virtual Role Role { get; set; }
        [ForeignKey("PermissionId")]
        [JsonIgnore]
        public virtual Permission Permission { get; set; }
    }
}
