using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Core.Helpers
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public List<T> Items { get; }

        private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
