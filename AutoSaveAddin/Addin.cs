using System.IO;
using System.Threading.Tasks;
using AutoSaveAddin.Model;
using Aveva.ApplicationFramework;
using Aveva.ApplicationFramework.Presentation;
using Newtonsoft.Json;

namespace AutoSaveAddin
{
    public class Addin : IAddin
    {        
        public string Name => "AutoSaveAddin";

        public string Description => "AutoSaveAddin";

        public void Start(ServiceManager serviceManager)
        {
            ICommandManager commandManager = DependencyResolver.GetImplementationOf<ICommandManager>();
            commandManager.Commands.Add(new AutoSaveFormCommand());
            
            AutoSaveServer.Start();          
        }

        public void Stop()
        {            
            AutoSaveServer.Stop();       
        }
    }
}