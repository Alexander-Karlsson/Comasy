
namespace Comasy.Core.Entities
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Nav Prop
        public ICollection<ContentBlock> ContentBlocks { get; set; } = new List<ContentBlock>();
    }
}
