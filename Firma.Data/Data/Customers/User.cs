﻿using System.ComponentModel.DataAnnotations;
using Firma.Data.Data.Customers;
using Firma.Data.Data.Movie;

namespace Firma.Data.Data.Customers
{
    public class User 
    {
        [Key]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Wrong email format")]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required(ErrorMessage ="HashedPassword is required")]
        [MaxLength(256)]
        public required string HashedPassword { get; set; } 

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(15)]
        [Phone(ErrorMessage = "Wrong phone number format, area code is required")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Registration date is required")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "User role is required")]
        public UserRole Role { get; set; } = UserRole.User;
        public ICollection<Recenzja> Recenzje { get; set; } = new List<Recenzja>();

    }

    public enum UserRole
    {
        User,
        Admin,
        Moderator
    }
}
