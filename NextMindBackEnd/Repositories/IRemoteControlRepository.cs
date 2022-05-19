using NextMindBackEnd.Data.Models;
using NextMindBackEnd.Data.Requests;

namespace NextMindBackEnd.Repositories
{
    public interface IRemoteControlRepository
    {
        public Task<ControllerDataToClient?> AddController(AddControllerRequest request, int? UserId);
    }
}
