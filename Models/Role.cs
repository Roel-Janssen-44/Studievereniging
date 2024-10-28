using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Studievereniging.Models
{

    public class Role
    {
        public const string Admin = "Admin";
        public const string Member = "Member";
        public const string Guest = "Guest";
        public const string BoardMember = "BoardMember";
    }
}
