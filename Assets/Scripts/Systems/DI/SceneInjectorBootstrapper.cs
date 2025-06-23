namespace Bap.DependencyInjection
{
    public class SceneInjectorBootstrapper : Bootstrapper
    {
        public override void Bootstrap()
        {
            Container.ConfigForScene();
        }
    }
}