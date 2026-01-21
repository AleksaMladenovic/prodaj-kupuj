using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Interfaces
{
     public interface ISecretWordService
     {
        Task SeedWordsAsync();
        Task<List<string>> GetAllWordsAsync();
        Task<string> GetRandomSecretWordAsync();
     }
    
}
