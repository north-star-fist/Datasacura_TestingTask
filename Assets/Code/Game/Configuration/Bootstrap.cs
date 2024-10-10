using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Datasacura.TestTask.ZooWorld.Config
{
    public class Bootstrap : LifetimeScope
    {
        [SerializeField]
        private AScriptableInstaller[] _diInstallers;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            if (_diInstallers != null)
            {
                foreach (var installer in _diInstallers)
                {
                    installer.Install(builder);
                }
            }
            builder.RegisterEntryPoint<BootstrapEntryPoint>();
        }
    }
}
