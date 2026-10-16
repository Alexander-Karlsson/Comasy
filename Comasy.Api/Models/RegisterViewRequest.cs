using System.ComponentModel.DataAnnotations;

namespace Comasy.Api.Models;

public class RegisterViewRequest
{
    [Range(1, int.MaxValue, ErrorMessage ="PageId måste vara ett positivt tal!")]
    public int PageId { get; set; }
}