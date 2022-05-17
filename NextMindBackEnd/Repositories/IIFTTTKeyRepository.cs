using NextMindBackEnd.Data.Models;
using System.Collections;

namespace NextMindBackEnd.Repositories
{
    public interface IIFTTTKeyRepository
    {
        public Task<List<IFTTTKey>> GetKeys(int Id);
        public Task<int> AddKey(string key, int Id);
    }
}
