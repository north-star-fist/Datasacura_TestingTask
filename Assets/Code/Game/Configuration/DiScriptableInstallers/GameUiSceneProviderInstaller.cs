using Datasacura.TestTask.ZooWorld.Flow;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Datasacura.TestTask.ZooWorld.Config
{
    [CreateAssetMenu(
        fileName = "Game UI Scene Provider Installer",
        menuName = "Zoo World/DI Installers/Game UI Scene Provider"
    )]
    public class GameUiSceneProviderInstaller : AScriptableInstaller
    {
        [SerializeField, AssetReferenceUILabelRestriction(AddressablesTags.GameUiScene)]
        private AssetReference _gameUiScene;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<GameUiSceneProvider>(Lifetime.Scoped)
                .WithParameter(_gameUiScene)
                .As<IGameUiSceneProvider>();
        }
    }
}
