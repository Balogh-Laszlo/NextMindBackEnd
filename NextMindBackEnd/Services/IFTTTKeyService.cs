using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Data.Models;
using NextMindBackEnd.Data.Responses;
using NextMindBackEnd.Repositories;
using NextMindBackEnd.Utils;

namespace NextMindBackEnd.Services
{
    public class IftttKeyService : IIftttKeyService
    {
        private readonly IIftttKeyRepository repo;
        public IftttKeyService(IIftttKeyRepository repo)
        {
            this.repo = repo;
        }

        public async Task<AddKeyResponse> AddKey(string token, string key)
        {
            if(token == null)
            {
                throw new AddKeyException("Token null", 301);
            }
            if(key == null)
            {
                throw new AddKeyException("Key empty", 302);
            }
            var tokenData = TokenMethods.Instance.ValidateToken(token);
            if(tokenData == null)
            {
                throw new AddKeyException("Invalid token!", 303);
            }
            AddKeyResponse response = new AddKeyResponse();
            try
            {
                var repoResponse = await repo.AddKey(key, tokenData.Id);
                if(repoResponse>0)
                {
                    response.Code = 200;
                    response.Message = "Success";
                    response.KeyId = repoResponse;
                }
                else
                {
                    throw new AddKeyException("Couldn't reach the database!", 304);
                }
                return response;
            }catch(Exception ex)
            {
                throw new AddKeyException(ex.Message, 400);
            }
        }

        private Task<AddKeyResponse> Ok(AddKeyResponse response)
        {
            throw new NotImplementedException();
        }

        public async Task<GetKeysResponse> GetKeys(string token)
        {
            if(token == null)
            {
                throw new GetKeysException("Token null!");
            }
            var tokendata = TokenMethods.Instance.ValidateToken(token);
            if(tokendata == null)
            {
                throw new GetKeysException("Invalid token");
            }
            var response = new GetKeysResponse();
            try
            {
                var repoResponse = await repo.GetKeys(tokendata.Id);
                if(repoResponse != null)
                {
                    response.Code = 200;
                    response.Message = "Success";
                    var keys = new List<Key>();
                    foreach(var r in repoResponse)
                    {
                        keys.Add(new Key() { Id = r.Id, iftttKey = r.Key });
                    }
                    response.Keys = keys ;
                }
                else
                {
                    throw new GetKeysException("couldn't reach the database!");
                }
                return response;
            }catch(Exception ex)
            {
                throw new GetKeysException(ex.Message);
            }
            

        }
    }
}
