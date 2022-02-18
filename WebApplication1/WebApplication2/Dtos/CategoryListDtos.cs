using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Dtos
{
    public class CategoryListDtos
    {
        public List<CategoryListItemDtos> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
