
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using FluentCaching.Api;

namespace FluentCaching
{
    public static class CachingExtensions
    {
        public static CachingKeyBuilder<T> Cache<T>(this T targetObject) => new CachingKeyBuilder<T>(targetObject);
    }
}
