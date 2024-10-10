using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Datasakura.TestTask.ZooWorld.Config
{
    public class GameManagerScene : LifetimeScope
    {
        [SerializeField]
        private GameManager _gameManager;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance<IGameManager, IGameSceneContext>(_gameManager);
        }
    }
}
