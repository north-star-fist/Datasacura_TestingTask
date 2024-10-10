using Datasakura.TestTask.ZooWorld.StateMachine;
using VContainer;

namespace Datasakura.TestTask.ZooWorld.Config
{
    public class AppFlow : DiStateMachine, IAppFlow
    {
        public AppFlow(IObjectResolver resolver) : base(resolver) { }
    }
}
