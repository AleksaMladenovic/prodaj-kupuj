using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Interfaces
{
    public interface IClueRepository
    {
        Task AddClueToRoomAsync(string roomId, Clue clue);
        Task <List<Clue>>GetCluesFromRoomAsync(string roomId);
    }
}
