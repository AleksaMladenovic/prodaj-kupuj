using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Interfaces
{
    public interface IVoteRepository
    {
        Task AddVoteAsync(string roomId, Vote vote);
        Task<List<Vote>> GetVotesAsync(string roomId);
        Task ClearVotesAsync(string roomId);
    }
}
