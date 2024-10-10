using System.Collections.Generic;
using Datasakura.TestTask.ZooWorld.Flow;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Datasakura.TestTask.ZooWorld.Config
{
    /// <summary>
    /// Installer that configures <see cref="IAppFlow"/> and registers it in DI container.
    /// </summary>
    [CreateAssetMenu(menuName = "Zoo World/DI Installers/AppFlow", fileName = "AppFlow Installer")]
    public class AppFlowInstaller : AScriptableInstaller
    {
        [SerializeField, AssetReferenceUILabelRestriction(AddressablesTags.GameManagerScene)]
        private AssetReference _gameManagerScene;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<AppFlow>(Lifetime.Scoped).As<IAppFlow>();

            builder.RegisterBuildCallback(container =>
            {
                // Creating App State Machine when DI Context is ready
                var appFlow = container.Resolve<IAppFlow>();
                appFlow.RegisterState(new AppStateBoot());
                IList<LevelSO> levels = container.Resolve<LevelsSO>().Levels;
                IGameManagerSceneProvider gameManagerSceneProvider = container.Resolve<IGameManagerSceneProvider>();
                IGameLevelSceneProvider levelScenesProvider = container.Resolve<IGameLevelSceneProvider>();
                IGameUiSceneProvider uiSceneProvider = container.Resolve<IGameUiSceneProvider>();
                AppStateGame gameState = new AppStateGame(levels, gameManagerSceneProvider, levelScenesProvider, uiSceneProvider);
                appFlow.RegisterState(gameState);
            });
        }
    }
}
