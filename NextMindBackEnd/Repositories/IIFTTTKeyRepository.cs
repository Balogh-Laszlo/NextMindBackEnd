using NextMindBackEnd.Data.Models;
using System.Collections;

namespace NextMindBackEnd.Repositories
{
    public interface IIftttKeyRepository
    {
        public Task<List<IftttKey>> GetKeys(int Id);
        public Task<int> AddKey(string key, int Id);
    }
}
