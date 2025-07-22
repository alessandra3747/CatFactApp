namespace CatFactApplication.Services;

public interface IFileService
{
    Task AppendToFileAsync(string fileName, string content); 
}

public class FileService : IFileService
{
    public async Task AppendToFileAsync(string fileName, string content)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        await File.AppendAllTextAsync(fullPath, content + Environment.NewLine);
    }
}