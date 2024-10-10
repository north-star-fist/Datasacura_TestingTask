using UnityEngine;
using VContainer;

namespace Datasakura.TestTask.ZooWorld.Config
{
    public abstract class AScriptableInstaller : ScriptableObject
    {
        public abstract void Install(IContainerBuilder builder);
    }
}
