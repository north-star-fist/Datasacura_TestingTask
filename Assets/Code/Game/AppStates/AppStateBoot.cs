using Cysharp.Threading.Tasks;
using Datasacura.TestTask.ZooWorld.Config;
using Datasacura.TestTask.ZooWorld.StateMachine;
using VContainer;

namespace Datasacura.TestTask.ZooWorld.Flow
{
    public class AppStateBoot : IState
    {
        private IAppFlow _appStateService;

        [Inject]
        public void Construct(IAppFlow appStateService) => _appStateService = appStateService;

        public UniTask OnEnterAsync() => UniTask.CompletedTask;

        public UniTask OnExitAsync() => UniTask.CompletedTask;

        public async UniTask StartAsync()
        {
            await _appStateService.GoToStateAsync<AppStateGame>(null);
        }
    }
}
