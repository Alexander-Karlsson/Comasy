using System;
using System.Collections.Generic;
using System.Text;

namespace Comasy.Core.Entities
{

    // Lagrar sidviningar, inte unika besökare.
    // Eventuellt att jag testar hasha IP-adress för att fånga unika besökare.
    public class PageView
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public Page? Page { get; set; }
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    }
}
