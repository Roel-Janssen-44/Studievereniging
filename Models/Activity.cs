using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace Studievereniging.Models
{
    public class Activity
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public int? MaxParticipants { get; set; }
        public double? Price { get; set; }
        public DateTime? RegistrationDeadline { get; set; }
        public string? Category { get; set; }
        public DateTime? Calendar { get; set; }
        public string? Image { get; set; }
        public bool? IsPublic { get; set; }

        //[AllowNull]
        //public int AdminId { get; set; }

        //[AllowNull]
        //public virtual Admin? Admin { get; set; }

        [AllowNull]
        public int? AdminId{ get; set; } 
        [AllowNull]
        public Admin Admin { get; set; } = null!;

        public List<User> Organisers { get; set; } = new List<User>();
        public List<User> Participants { get; set; } = new List<User>();




    }
}
