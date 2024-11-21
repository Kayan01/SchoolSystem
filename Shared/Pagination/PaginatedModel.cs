using IPagedList;
using System.Collections.Generic;

namespace Shared.Pagination
{
    public class PaginatedModel<T> : BasePagedList<T> where T : class
    {
        public PaginatedModel(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount) : base(pageNumber, pageSize, totalCount)
        {
            Items = items;
        }
        public override IEnumerable<T> Items { get; }
    }
}
