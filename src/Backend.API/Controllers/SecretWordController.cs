using Microsoft.AspNetCore.Mvc;
using CommonLayer.Interfaces;

namespace Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecretWordsController : ControllerBase
    {
        private readonly ISecretWordService _secretWordService;

        public SecretWordsController(ISecretWordService secretWordService)
        {
            _secretWordService = secretWordService;
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            await _secretWordService.SeedWordsAsync();
            return Ok("Seeding završen.");
        }
        [HttpGet]
       
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var words = await _secretWordService.GetAllWordsAsync();
                return Ok(new { count = words.Count, words = words });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Greška pri čitanju iz baze: " + ex.Message });
            }
        }
        [HttpGet("random")]
        public async Task<IActionResult> GetRandomWord()
        {
            var word = await _secretWordService.GetRandomSecretWordAsync();
            return Ok(new { secretWord = word });
        }
    }
}