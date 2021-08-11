// ReSharper disable VirtualMemberCallInConstructor
namespace SportBattles.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using SportBattles.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Games = new HashSet<Game>();
            this.UserTeams = new HashSet<UserTeam>();
            this.Predictions = new HashSet<Prediction>();
            this.TennisPredictions = new HashSet<TennisPrediction>();
        }

        public string ProfilePictureId { get; set; }

        public virtual Image ProfilePicture { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public virtual ICollection<Prediction> Predictions { get; set; }

        public virtual ICollection<TennisPrediction> TennisPredictions { get; set; }

        public virtual ICollection<UserTeam> UserTeams { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
