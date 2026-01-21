using BusinessLayer.Services;
using CommonLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class SecretWordService : ISecretWordService
    {
        private readonly ISecretWordRepository _repository;
        private static readonly Random _random = new Random();

        public SecretWordService(ISecretWordRepository repository)
        {
            _repository = repository;
        }

        public async Task SeedWordsAsync()
        {
            await _repository.SeedWordsAsync();
        }

        public async Task<List<string>> GetAllWordsAsync()
        {
            return await _repository.GetAllWordsAsync();
        }

        public async Task<string> GetRandomSecretWordAsync()
        {
            var words = await _repository.GetAllWordsAsync();

            if (words == null || words.Count == 0)
            {
                return "NEMA_RECI";
            }

            int index = _random.Next(words.Count);
            return words[index];
        }
    }
}


