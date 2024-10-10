using UnityEngine;

namespace Datasakura.TestTask.ZooWorld.Config
{
    /// <summary>
    /// Animal configuration (and factory).
    /// </summary>
    public abstract class AAnimalFactorySO : ScriptableObject, IAnimalFactory
    {
        public abstract IAnimal GetAnimal(IGameSceneContext sceneContext, Vector3 newLocation, Quaternion newRotation);
        public abstract bool ReleaseAnimal(IGameSceneContext sceneContext, IAnimal animal);
    }
}
