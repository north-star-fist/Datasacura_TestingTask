using R3;
using UnityEngine;

namespace Datasacura.TestTask.ZooWorld
{
    public interface IAnimal
    {
        /// <summary>
        /// Event that is raised on collision with another animal.
        /// </summary>
        public Observable<(IAnimal, IAnimal)> OnAnimalCollision { get; }

        /// <summary>
        /// Returns true if the animal is predator.
        /// </summary>
        bool IsPredator { get; }

        /// <summary>
        /// Returns current position of the animal.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Updates state of the animal. Should be invoked each frame.
        /// </summary>
        /// <param name="deltaTime"> time spent since previous frame </param>
        /// <returns> position of the animal after update </returns>
        public Vector3 Tick(float deltaTime);

        /// <summary>
        /// Sets destination for the animal. If it is not null animal should move towards it.
        /// </summary>
        /// <param name="destination"> new destination or null </param>
        public void SetDestination(Vector3? destination);
    }
}
