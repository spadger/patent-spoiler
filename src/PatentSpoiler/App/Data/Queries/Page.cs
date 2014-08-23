using System.Collections.Generic;

namespace PatentSpoiler.App.Data.Queries
{
    public static class Page
    {
        public static PageOf<T> Of<T>(IEnumerable<T> items, int count)
        {
            return new PageOf<T>(items, count);
        }
    }

    public class PageOf<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Count { get; private set; }

        public PageOf(IEnumerable<T> items, int count)
        {
            Items = items;
            Count = count;
        }
    }
}