using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ErieHackMVP1.Models
{
    public enum YesNo { Yes, No }

    public enum Carriers { Verizon, TMobile, ATT, Alltel, Boost, Cricket, Sprint, MetroPCS, Nextel, StraightTalk, USCellular, VirginMobile }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        public YesNo IsSubscribedToUpdates { get; set; }
        public Carriers Carrier { get; set; }
        public string SMSRoute { get; set; }

        [Required]
        public string County { get; set; }

        public ApplicationUser()
        {
           var Reports = new List<Report>();
        }

        public virtual ICollection<Report> Reports { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Report> Reports { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}