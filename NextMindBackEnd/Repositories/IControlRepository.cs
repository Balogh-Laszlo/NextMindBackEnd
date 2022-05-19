using NextMindBackEnd.Data.Models;

namespace NextMindBackEnd.Repositories
{
    public interface IControlRepository
    {
        public Task<List<ControlToClient>> GetControls(int id);
    }
}
