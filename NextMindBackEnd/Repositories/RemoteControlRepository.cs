using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Data.Models;
using NextMindBackEnd.Data.Requests;
using NextMindBackEnd.Models;

namespace NextMindBackEnd.Repositories
{
    public class RemoteControlRepository : IRemoteControlRepository
    {
        private readonly DataContext context;
        public RemoteControlRepository(DataContext context)
        {
            this.context = context;
        }
        public async Task<ControllerDataToClient?> AddController(AddControllerRequest request, int? UserId)
        {
            User user;
            RemoteController _remoteController;
            ControllerDataToClient _controllerDataToClient = new ControllerDataToClient();
            List<PageToClient> pagesToClient = new List<PageToClient>(); 
            Console.WriteLine(request.Pages.Count);
            try
            {
                //find user
                user = await findUser(UserId);
                //create controller
                _remoteController = await addRemoteControl(request.ControllerName, user);
                Console.WriteLine(request.Pages.Count);
                foreach (var page in request.Pages)
                {
                    Console.WriteLine(page.Controls.Count);
                    //create page
                    Page _page = await addPage(page.Index, _remoteController);
                    PageToClient pageToClient = new PageToClient();
                    pageToClient.Id = _page.Id;
                    pageToClient.Index = page.Index;
                    List<ControlToClient> controlsToClient = new List<ControlToClient>();
                    List<Control> controls = new List<Control>();
                    foreach (var control in page.Controls)
                    {
                        IftttKey key;
                        Control con;
                        ControlToClient controlToClient = new ControlToClient();
                        //add key if it does not exists and find if it does
                        if (control.IFTTTKeyID != null && control.IFTTTKeyID > 0)
                        {
                            try
                            {
                                key = await findKey(control.IFTTTKeyID);
                            }catch (AddControllerException ex)
                            {
                                throw ex;
                            }

                        }
                        else
                        {

                            key = await addIFTTTKey(control.IFTTTKey);
                            Console.WriteLine(key.Id);

                        }
                        //add control if it does not exists or find if it does
                        if (control.ControlID != null && control.ControlID > 0)
                        {
                            con = await findControl(control.ControlID);
                        }
                        else
                        {

                            con = await addControl(control.ControlName, control.URL, key);

                        }
                        controls.Add(con);

                        controlToClient.IFTTTKey = new Key() { Id = key.Id, iftttKey = key.Key};
                        controlToClient.URL = con.URL;
                        controlToClient.Name = con.Name;
                        controlToClient.Id = con.Id;

                        controlsToClient.Add(controlToClient);

                    }
                    foreach (var control in controls)
                    {
                        await addToPageControl(control.Id, _page.Id, control, _page);
                    }
                    pageToClient.Controls = controlsToClient;
                    pagesToClient.Add(pageToClient);
                }
                _controllerDataToClient.Name = request.ControllerName;
                _controllerDataToClient.Pages = pagesToClient;
                _controllerDataToClient.Id = _remoteController.Id;
                return _controllerDataToClient;
            }
            catch(AddControllerException ex)
            {
                throw ex;
            }

        }
        public async Task<List<ControllerDataToClient>> GetControllers(int? UserId)
        {
            var result = new List<ControllerDataToClient>();
            var remoteControllers = await context.RemoteControllers.Where(r => r.User.Id == UserId).ToListAsync();
            foreach (var remoteController in remoteControllers)
            {
                var pages = new List<PageToClient>();
                var p = await context.Pages.Where(p=> p.RemoteController.Id == remoteController.Id).ToListAsync();      
                foreach(var _page in p)
                {
                    var controls = await context.PageControls.Where(p => p.PageID==_page.Id).ToListAsync();
                    var controlsToClient = new List<ControlToClient>();
                    foreach(var control in controls)
                    {
                        var c = await context.Controls.FindAsync(control.ControlID);
                        if (c != null)
                        {
                            var ifttt = await context.IftttKeys.FindAsync(c.IftttKeyId);
                            if (ifttt != null)
                            {
                                controlsToClient.Add(new ControlToClient()
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    URL = c.URL,
                                    IFTTTKey = new Key() { Id = ifttt.Id, iftttKey = ifttt.Key }
                                });
                            }
                        }
                       
                    }
                    pages.Add(new PageToClient()
                    {
                        Id = _page.Id,
                        Index = _page.Index,
                        Controls = controlsToClient
                    });

                    
                }
                result.Add(new ControllerDataToClient()
                {
                    Id = remoteController.Id,
                    Name = remoteController.Name,
                    Pages = pages
                });
           
            }
            return result;
        }

        private async Task addToPageControl(int id1, int id2, Control control, Page page)
        {
            var res = await context.PageControls.AddAsync(new PageControl() { Control = control, ControlID = id1, Page = page, PageID = id2 });
            if (!await Save())
            {
                throw new AddControllerException("Database error", 400);
            }
            if (res == null)
            {
                throw new AddControllerException($"can't create relation between controlId: {id1} and pageId: {id2}", 310);
            }
            return;
        }

        private async Task<Control> addControl(string? controlName, string? uRL, IftttKey key)
        {
            var control = await context.Controls.AddAsync(new Control() { IftttKey = key, Name = controlName, URL = uRL });
            if (!await Save())
            {
                throw new AddControllerException("Database error", 400);
            }
            if (control == null)
            {
                throw new AddControllerException($"Can't add control with name: {controlName}", 309);
            }
            return control.Entity;
        }

        private async Task<Control> findControl(int? controlID)
        {
            var control = await context.Controls.FindAsync(controlID);
            if (control == null)
            {
                throw new AddControllerException($"Control not found with Id: {controlID}", 308);
            }
            return control;
        }

        private async Task<Page> addPage(int index, RemoteController remoteController)
        {
            var page = await context.Pages.AddAsync( new Page() { Index = index, RemoteController = remoteController });
            if (!await Save())
            {
                throw new AddControllerException("Database error", 400);
            }
            if (page == null)
            {
                throw new AddControllerException($"Can't add page with index: {index}", 307);
            }
            return page.Entity;
        }

        private async Task<RemoteController> addRemoteControl(string controllerName, User user)
        {
            var controller = await context.RemoteControllers.AddAsync(new RemoteController() { Name = controllerName, User = user });
            if (!await Save())
            {
                throw new AddControllerException("Database error", 400);
            }
            if (controller == null)
            {
                throw new AddControllerException("Can't create remote controller!", 306);
            }
            return controller.Entity;
        }

        private async Task<User> findUser(int? userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new AddControllerException("User not found", 305);
            }
            return user;
        }

        private async Task<IftttKey> findKey(int? iFTTTKeyID)
        {

            var key = await context.IftttKeys.FindAsync(iFTTTKeyID);
            Console.WriteLine(key.Id);
            if (key != null)
            {
                return key;
            }
            else
            {
                throw new AddKeyException("IftttKey not found", 304);
            }

        }

        private async Task<IftttKey> addIFTTTKey(string? iFTTTKey)
        {
            var key = await context.IftttKeys.AddAsync(new IftttKey() { Key = iFTTTKey });
            if (!await Save())
            {
                throw new AddControllerException("Database error", 400);
            }
            if (key != null)
            {
                return key.Entity;
            }
            else
            {
                throw new AddControllerException("Can not add IftttKey", 303);
            }
        }
        private async Task<bool> Save()
        {
            var save = await context.SaveChangesAsync();
            return save > 0 ? true : false;
        }


    }
}
