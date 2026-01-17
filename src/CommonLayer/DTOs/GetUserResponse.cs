using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.DTOs
{
    public class GetUserResponse
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public long GamesPlayed { get; set; }
        public long WinsLikeCrewmate { get; set; }
        public long WinsLikeImpostor { get; set; }
        public long TotalScore { get; set; }

        public GetUserResponse()
        {
            UserId = string.Empty;
            Username = string.Empty;
            Email = string.Empty;
            GamesPlayed = 0;
            WinsLikeCrewmate = 0;
            WinsLikeImpostor = 0;
            TotalScore = 0;
        }

        
    }
}
