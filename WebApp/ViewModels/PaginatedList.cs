using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; set; }
        public int PageStartValue { get; set; }
        public int PageEndValue { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            PageStartValue = (pageIndex - 1) * pageSize + 1;
            if (TotalPages == pageIndex)
                PageEndValue = TotalCount;
            else
                PageEndValue = pageIndex * pageSize;
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }
        public bool HasPreviousTwo
        {
            get
            {
                return (PageIndex > 2);
            }
        }
        public bool HasPreviousThree
        {
            get
            {
                return (PageIndex > 3);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        public bool HasNext2
        {
            get
            {
                return ( PageIndex + 1 < TotalPages);
            }
        }
         public bool HasNext3
        {
            get
            {
                return (PageIndex + 2 < TotalPages);
            }
        }
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            try
            {
                var count = source.Count();
                var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                return new PaginatedList<T>(items, count, pageIndex, pageSize);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                throw;
            }
        }

    }
}