using Datasakura.TestTask.ZooWorld.Flow;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Datasakura.TestTask.ZooWorld.Config
{
    [CreateAssetMenu(
        fileName = "GameManager Scene Provider Installer",
        menuName = "Zoo World/DI Installers/GameManager Scene Provider"
    )]
    public class GameManagerInstaller : AScriptableInstaller
    {
        [SerializeField, AssetReferenceUILabelRestriction(AddressablesTags.GameManagerScene)]
        private AssetReference _gameManagerScene;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<GameManagerSceneProvider>(Lifetime.Scoped)
                .WithParameter(_gameManagerScene)
                .As<IGameManagerSceneProvider>();
        }
    }
}
