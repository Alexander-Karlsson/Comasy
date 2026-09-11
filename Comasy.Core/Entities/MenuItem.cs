using System;
using System.Collections.Generic;
using System.Text;

namespace Comasy.Core.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsVisible { get; set; } = true;

        // Nav Prop ----
        public int PageId { get; set; }
        public Page? Page { get; set; }
        // ----

        // Självreferens för undermenyer
        public int? ParentId { get; set; }
        public MenuItem? Parent { get; set; }
        public ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
}
