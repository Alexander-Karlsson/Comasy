using Comasy.Core.Entities;

namespace Comasy.Web.Models;

public record MenuRowData(MenuItem Item, bool IsFirst, bool IsLast, bool IsChild);