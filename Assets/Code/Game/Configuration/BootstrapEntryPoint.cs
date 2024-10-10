﻿using System.Threading;
using Cysharp.Threading.Tasks;
using Datasakura.TestTask.ZooWorld.Flow;
using VContainer;
using VContainer.Unity;

namespace Datasakura.TestTask.ZooWorld.Config
{
    /// <summary>
    /// Entry point of the application logic (See VContainer docs).
    /// </summary>
    public class BootstrapEntryPoint : IAsyncStartable
    {
        private readonly IAppFlow _appStateService;

        [Inject]
        public BootstrapEntryPoint(IAppFlow appStateService)
        {
            _appStateService = appStateService;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            await _appStateService.GoToStateAsync<AppStateBoot>(null);
        }
    }
}
