using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Studievereniging.Models
{
    public class Admin : User
    {
        [AllowNull]
        public ICollection<Activity>? Activities { get; } = new List<Activity>();
    }
}
