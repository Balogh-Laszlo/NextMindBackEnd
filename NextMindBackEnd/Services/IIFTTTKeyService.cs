
using NextMindBackEnd.Data.Responses;

namespace NextMindBackEnd.Services
{
    public interface IIftttKeyService
    {
        public Task<GetKeysResponse> GetKeys(string token);
        public Task<AddKeyResponse> AddKey(string token, string key);
    }
}
