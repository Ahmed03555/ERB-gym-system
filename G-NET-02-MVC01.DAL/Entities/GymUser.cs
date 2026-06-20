using G_NET_02_MVC01.DAL.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace G_NET_02_MVC01.DAL.Entities
{
    public class GymUser : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MaxLength(11)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$",
            ErrorMessage = "Egyptian phone format only.")]
        public string Phone { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
        public GymGender Gender { get; set; }

        public GymUserAddress Address { get; set; } = null!;
    }
    [Owned]
    public class GymUserAddress
    {
        [Required, MaxLength(100)]
        public string Street { get; set; } = null!;
        [Required, MaxLength(50)]
        public string City { get; set; } = null!;
        [Required, MaxLength(50)]
        public string State { get; set; } = null!;
        [Required, MaxLength(10)]
        public string ZipCode { get; set; } = null!;

        public int BuildingNumber { get; set; }
    }
}
