﻿using Datasacura.TestTask.ZooWorld.Flow;
using UnityEngine;
using VContainer;

namespace Datasacura.TestTask.ZooWorld.Config
{
    [CreateAssetMenu(fileName = "Level Provider Installer", menuName = "Zoo World/DI Installers/Level Provider")]
    public class LevelScenesInstaller : AScriptableInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<GameLevelSceneProvider>(Lifetime.Scoped)
                .As<IGameLevelSceneProvider>();
        }
    }
}