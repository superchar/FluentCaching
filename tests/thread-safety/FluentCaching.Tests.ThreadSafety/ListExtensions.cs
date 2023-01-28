namespace FluentCaching.Tests.ThreadSafety
{
    public static class ListExtensions
    {
        public static T GetRandomItem<T>(this IList<T> list)
        {
            if (list?.Any() == false)
            {
                return default;
            }

            var random = new Random();
            return list[random.Next(list.Count - 1)];
        }
    }
}