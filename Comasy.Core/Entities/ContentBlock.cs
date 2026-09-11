using Comasy.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comasy.Core.Entities
{
    public class ContentBlock
    {
        public int Id { get; set; }

        // FK -----
        public int PageId { get; set; }
        public Page? Page { get; set; }
        // -----

        public BlockType BlockType { get; set; }
        public ContentZone Zone { get; set; } = ContentZone.Main;
        public int SortOrder { get; set; }

        // Innehåll i blocket
        public string? Text { get; set; }
        public string? ImageUrl { get; set; }
        public string? AltText { get; set; }
        public string? LinkUrl { get; set; }
        public string? LinkText { get; set; }
    }
}
