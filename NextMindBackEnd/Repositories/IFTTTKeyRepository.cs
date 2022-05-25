using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Data.Models;
using System.Collections;

namespace NextMindBackEnd.Repositories
{
    public class IftttKeyRepository : IIftttKeyRepository
    {
        private readonly DataContext context;
        public IftttKeyRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddKey(string key, int Id)
        {
            try
            {
                var result = await context.IftttKeys.AddAsync(new IftttKey() { Key = key});
                var res = result.Entity.Id;
                if (Save())
                {
                    res = 0;
                }
                return res;
            }catch (Exception ex)
            {
                throw new AddKeyException(ex.Message, 400);
            }
        }

        public async Task<List<IftttKey>> GetKeys(int Id)
        {
            try
            {
                var res = new List<IftttKey>();
                var remoteControllers = await context.RemoteControllers.Where(u => u.User.Id == Id).Select(s => s.Id).ToListAsync();
                var pages = await context.Pages.Where(p => remoteControllers.Contains(p.RemoteController.Id)).Select(s => s.Id).ToListAsync();
                var keys = await context.PageControls.Where(p => pages.Contains(p.PageID)).Select(s => s.Control.IftttKey).ToListAsync();
                foreach (var key in keys)
                {
                    if (!isAlreadySelected(key,res))
                    {
                        res.Add(key);
                    }
                }
                return res;
            }catch (Exception ex)
            {
                throw new GetKeysException(ex.Message);
            }
        }
        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved>0? true : false;
        }
        private bool isAlreadySelected(IftttKey key, List<IftttKey> list)
        {
            foreach (var item in list)
            {
                if(item.Key == key.Key)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
