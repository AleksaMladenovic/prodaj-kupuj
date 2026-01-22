using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.DTOs
{
    public class SendClueDto
    {
        public required string UserId { get; set; }
        public required string Username { get; set; }
        public required string ClueWord { get; set; }
        public required DateTime Timestamp { get; set; }
    }
}
