using Datasacura.TestTask.ZooWorld.StateMachine;
using VContainer;

namespace Datasacura.TestTask.ZooWorld.Config
{
    public class AppFlow : DiStateMachine, IAppFlow
    {
        public AppFlow(IObjectResolver resolver) : base(resolver) { }
    }
}
