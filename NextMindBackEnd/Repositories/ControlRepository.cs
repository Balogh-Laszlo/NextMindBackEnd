using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Data.Models;

namespace NextMindBackEnd.Repositories
{
    public class ControlRepository : IControlRepository
    {
        private readonly DataContext context;
        public ControlRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<ControlToClient>> GetControls(int id)
        {

            List<ControlToClient> res = new List<ControlToClient>();

            var remoteControllers = await context.RemoteControllers.Where(c => c.User.Id == id).ToListAsync();
            if (remoteControllers.Count() == 0)
            {
                return res;
            }
            var pages = new List<Page>();
            foreach (var remoteController in remoteControllers)
            {
                if (remoteController != null)
                {
                    var p = await context.Pages.Where(c => c.RemoteController.Id == remoteController.Id).ToListAsync();
                    foreach(var page in p)
                    {
                        pages.Add(page);
                    }
                }
            }
            if (pages.Count() == 0)
            {
                return res;
            }
            var controls = new List<Control>();
            foreach (var page in pages)
            {
                if (page != null)
                {
                    var c = await context.PageControls.Where(c => c.PageID == page.Id).Select(s => s.Control).ToListAsync();
                    foreach (var control in c)
                    {
                        controls.Add(control);
                    }
                }
            }
            if (controls.Count() == 0)
            {
                return res;
            }
            foreach (var control in controls)
            {
                Console.WriteLine(control);
                ControlToClient controlToClient = new ControlToClient();
                
                if (control != null)
                {

                        var iftttkey = await context.IftttKeys.FindAsync(control.IftttKeyId);
                        if (iftttkey != null)
                        {
                            controlToClient.IFTTTKey = new Key() { Id = iftttkey.Id, iftttKey = iftttkey.Key };
                        }
                    
                    //controlToClient.IftttKey = control.IftttKey.Key;
                    controlToClient.URL = control.URL;
                    controlToClient.Name = control.Name;
                    controlToClient.Id = control.Id;
                    if (!IsControlAdded(res,controlToClient))
                    {
                        res.Add(controlToClient);
                    }
                        
                }
            }
            return res;
        }
        private bool IsControlAdded(List<ControlToClient> controls, ControlToClient control)
        {
            foreach (var controlToClient in controls)
            {
                if (controlToClient.Id == control.Id)
                {

                    return true;
                }
            }
            return false;

        }
    }

}
