namespace Bap.DependencyInjection
{
    public class GlobalInjectorBootstrapper : Bootstrapper
    {
        public override void Bootstrap()
        {
            Container.ConfigAsGlobal();
        }
    }
}