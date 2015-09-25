using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public interface ISearchService<T>
    {
        Task<IEnumerable<T>> ContainsSearch(string searchText, CancellationToken token);
    }
}