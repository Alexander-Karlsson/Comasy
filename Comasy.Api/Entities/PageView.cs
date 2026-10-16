namespace Comasy.Api.Entities;

public class PageView
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
}