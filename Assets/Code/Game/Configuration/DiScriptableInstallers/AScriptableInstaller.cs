using UnityEngine;
using VContainer;

namespace Datasacura.TestTask.ZooWorld.Config
{
    public abstract class AScriptableInstaller : ScriptableObject
    {
        public abstract void Install(IContainerBuilder builder);
    }
}
