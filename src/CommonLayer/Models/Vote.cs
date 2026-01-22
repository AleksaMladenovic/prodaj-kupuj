using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class Vote
    {
        public required string UserId { get; set; }
        public required string Username { get; set; }
        public required string TargetId { get; set; }
        public required string TargetUsername { get; set; }
    }
}
