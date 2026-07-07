using Aveva.ApplicationFramework;
using Aveva.ApplicationFramework.Presentation;

namespace AutoSaveAddin
{
    public class Addin : IAddin
    {
        public string Name
        {
            get { return "AutoSaveAddin"; }
        }

        public string Description
        {
            get { return "AutoSaveAddin"; }
        }

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
