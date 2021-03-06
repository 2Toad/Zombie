﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Barricade;

namespace Zombie.Models
{
    public class User : LocalizedModel, IClaimUser
    {
        // IClaimUser
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        IEnumerable<IClaim> IClaimUser.Claims { get { return Claims; } }
        public string AccessToken { get; set; }
        public DateTime? AccessTokenExpiration { get; set; }

        public string ActivationCode { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<Claim> Claims { get; set; }

        public User()
        {
            AccessTokenExpiration = null;
        }

        internal static void Seed(SiteContext context)
        {
            var users = new[] {
                new User {
                    Username = "Admin1",
                    Email = "2toad.zombie+admin1@gmail.com",
                    PasswordHash = SecurityContext.GeneratePasswordHash("Pass123!", "b331P8^Gf5BNfq!lLcS$!7b@144N9XR6"),
                    PasswordSalt = "b331P8^Gf5BNfq!lLcS$!7b@144N9XR6",
                    FirstName = "Admin", 
                    LastName = "One",
                    LanguageId = context.Languages.Single(x => x.Locale == "en-US").Id,
                    Claims = new List<Claim> {context.Claims.Single(x => x.Type == "Role" && x.Value == "Admin")}
                },
                new User {
                    Username = "User1",
                    Email = "2toad.zombie+admin1@gmail.com",
                    PasswordHash = SecurityContext.GeneratePasswordHash("Pass123!", "aK2Ik5Y@LLcTo8PGf29Zx#!*!S99416s"),
                    PasswordSalt = "aK2Ik5Y@LLcTo8PGf29Zx#!*!S99416s",
                    FirstName = "User", 
                    LastName = "One",
                    LanguageId = context.Languages.Single(x => x.Locale == "es").Id,
                    Claims = new List<Claim> {context.Claims.Single(x => x.Type == "Role" && x.Value == "User")}
                }
            };

            context.Users.AddOrUpdate(u => new { u.Username }, users);
        }
    }

    internal static class UserConfiguration
    {
        internal static DbModelBuilder ConfigureUser(this DbModelBuilder modelBuilder)
        {
            var table = modelBuilder.Entity<User>();

            // IClaimUser
            table.Property(x => x.Username).IsRequired().HasMaxLength(15);
            table.Property(x => x.PasswordHash).IsRequired().HasMaxLength(88).HasColumnType("char");
            table.Property(x => x.PasswordSalt).IsRequired();
            table.Property(x => x.AccessToken).HasMaxLength(32).HasColumnType("char");

            table.Property(x => x.ActivationCode).HasMaxLength(32);
            table.Property(x => x.Email).HasMaxLength(254);
            table.Property(x => x.FirstName).HasMaxLength(25);
            table.Property(x => x.LastName).HasMaxLength(25);

            return modelBuilder;
        }
    }
}
