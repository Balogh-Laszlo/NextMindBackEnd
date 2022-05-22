using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Data.Requests;
using NextMindBackEnd.Data.Responses;
using NextMindBackEnd.Repositories;
using NextMindBackEnd.Utils;

namespace NextMindBackEnd.Services
{
    public class RemoteControlService : IRemoteControlService
    {
        private readonly IRemoteControlRepository remoteControlRepository;
        public RemoteControlService(IRemoteControlRepository repo)
        {
            remoteControlRepository = repo;
        }
        public async Task<AddControllerResponse> AddController(AddControllerRequest request)
        {
            var UserId = TokenMethods.Instance.ValidateToken(request.Token)?.Id;
            if(UserId == null || UserId < 1)
            {
                throw new AddControllerException("Invalid Token", 301);
            }
            try
            {
                var res = await remoteControlRepository.AddController(request,UserId);
                if(res == null)
                {
                    throw new AddControllerException("Data base error!", 302);
                }
                var response = new AddControllerResponse();
                response.Code = 200;
                response.Message = "Success";
                response.Controller = res;
                return response;
            }catch(AddControllerException ex)
            {
                var response = new AddControllerResponse();
                response.Code = ex.Code;
                response.Message = ex.Message;
                response.Controller = null;
                return response;
            }
        }

        public async Task<GetRemoteControllersResponse> GetControllers(string token)
        {
            var UserId = TokenMethods.Instance.ValidateToken(token)?.Id;
            if (UserId == null || UserId < 1)
            {
                throw new GetRemoteControllersException("Invalid Token", 301);
            }
            var response = new GetRemoteControllersResponse();
            try
            {
                response.RemoteControllers = await remoteControlRepository.GetControllers(UserId);
                response.Code = 200;
                response.Message = "Success";
                return response;
            }catch (GetRemoteControllersException ex)
            {
                response.Code = ex.Code;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
