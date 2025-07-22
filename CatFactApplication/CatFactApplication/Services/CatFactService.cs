using System.Text.Json;
using CatFactApplication.Exceptions;
using CatFactApplication.Models;

namespace CatFactApplication.Services;


public interface ICatFactService
{
    public Task<CatFactDto> GetCatFactAsync();
    public Task AddFactToFileAsync(CatFactDto fact);
}


public class CatFactService(HttpClient httpClient, IFileService fileService) : ICatFactService
{
    private const string ApiUrl = "https://catfact.ninja/fact";
    private const string FileName = "catfacts.txt";

    
    public async Task<CatFactDto> GetCatFactAsync()
    {
        var response = await httpClient.GetAsync(ApiUrl);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var catFact = JsonSerializer.Deserialize<CatFact>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (catFact == null)
        {
            throw new FailedDeserializationException("Failed to deserialize cat fact from API response.");
        }

        var catFactDto = new CatFactDto
        {
            Fact = catFact.Fact, 
            Length = catFact.Length
        };
        
        await AddFactToFileAsync(catFactDto);

        return catFactDto;
    }

    
    public async Task AddFactToFileAsync(CatFactDto fact)
    {
        await fileService.AppendToFileAsync(FileName, $"{fact.Fact} (length: {fact.Length})");
    }
    
}