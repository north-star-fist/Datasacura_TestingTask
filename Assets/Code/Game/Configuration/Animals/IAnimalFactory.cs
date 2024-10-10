using UnityEngine;

namespace Datasacura.TestTask.ZooWorld
{
    public interface IAnimalFactory
    {
        /// <summary>
        /// Creates an animal instance.
        /// </summary>
        public abstract IAnimal GetAnimal(IGameSceneContext sceneContext, Vector3 newLocation, Quaternion newRotation);

        /// <summary>
        /// Releases an animal instance.
        /// </summary>
        public abstract bool ReleaseAnimal(IGameSceneContext sceneContext, IAnimal animal);
    }
}
