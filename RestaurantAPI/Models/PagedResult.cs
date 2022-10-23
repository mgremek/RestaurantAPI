using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Models
{
    public class PagedResult<T>
    {
        public List<T> Items{ get; set; }
        public int TotalPages { get; set; }
        public int ItemFrom { get; set; }
        public int ItemTo { get; set; }
        public int TotalItemsCount { get; set; }
        public PagedResult(List<T> items, int totalCount, int pageSize, int pagenumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            ItemFrom = pageSize * (pagenumber - 1) + 1;
            //TODO jeżeli itemów jest np. 5 a klient wysle zapytanie z pageSize = 7, wtedy itemTo jest przekłamane
            ItemTo = ItemFrom + pageSize - 1;
            TotalPages = (int) Math.Ceiling(totalCount /(double) pageSize);
        }
    }
}
