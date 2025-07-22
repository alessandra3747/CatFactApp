using System.ComponentModel.DataAnnotations;

namespace CatFactApplication.Models;

public class CatFactDto
{
    [Required]
    [MaxLength(200)]
    public string Fact { get; set; } = null!;

    [Required] 
    public int Length { get; set; }
    
}