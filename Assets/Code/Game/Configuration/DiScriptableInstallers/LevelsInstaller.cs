using UnityEngine;
using VContainer;

namespace Datasakura.TestTask.ZooWorld.Config
{
    [CreateAssetMenu(fileName = "Levels Installer", menuName = "Zoo World/DI Installers/Levels")]
    public class LevelsInstaller : AScriptableInstaller
    {
        [SerializeField]
        private LevelsSO _levels;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(_levels);
        }
    }
}
