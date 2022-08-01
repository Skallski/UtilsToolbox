using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SkalluUtils.Utils.MeshParticleSystem
{
    [RequireComponent(typeof(MeshParticleSystem))]
    public class MeshParticleSystemController : MonoBehaviour
    {
        private static MeshParticleSystem meshParticleSystem;
        private static List<SingleParticle> singleParticleList = new List<SingleParticle>();

        public bool simulated = false; // are particles updated or not

        private void Awake() => meshParticleSystem = GetComponent<MeshParticleSystem>();

        private void Update()
        {
            if (!simulated) return;

            foreach (var particle in singleParticleList)
            {
                particle.UpdateParticle(particle.uvIdx);
            }
        }

        /// <summary>
        /// Spawns single particle
        /// </summary>
        /// <param name="spawnPosition"> spawn position </param>
        /// <param name="moveDirection"> movement direction if particle system is updated </param>
        /// <param name="rotation"> rotation </param>
        /// <param name="size"> size </param>
        public static void SpawnSingleParticle(Vector3 spawnPosition, Vector3 moveDirection, float rotation, Vector2 size) =>
            singleParticleList.Add(new SingleParticle(spawnPosition, moveDirection, rotation, size, meshParticleSystem));

        /// <summary>
        /// Destroys particles
        /// </summary>
        public static void DestroyParticles() => meshParticleSystem.ResetMesh();

        /// <summary>
        /// Class representing single particle
        /// </summary>
        private class SingleParticle
        {
            private MeshParticleSystem meshParticleSystem;
            
            private Vector3 position; // current position
            private Vector3 direction; // move direction
            private float rotation; // rotation angle (in degrees)
            private Vector2 size; // size [x, y]
            
            internal int uvIdx; // index from UV pixel array
            private int quadIdx; // id

            public SingleParticle(Vector3 position, Vector3 direction, float rotation, Vector2 size, MeshParticleSystem meshParticleSystem)
            {
                this.position = position;
                this.direction = direction;
                this.rotation = rotation;
                this.size = size;
                this.meshParticleSystem = meshParticleSystem;
                
                uvIdx = Random.Range(0, meshParticleSystem.particleUvPixelsArray.Length); // random index from UV pixel array
                quadIdx = meshParticleSystem.AddQuad(position, rotation, size, uvIdx);
            }

            /// <summary>
            /// This method determines the particle behavior over time
            /// It is called every Update (every frame), so don't do heavy stuff here
            /// </summary>
            public void UpdateParticle(int uvIdx)
            {
                // position += direction * Time.deltaTime;
                // rotation += 360f * Time.deltaTime;
                
                meshParticleSystem.UpdateQuad(quadIdx, position, rotation, size, uvIdx); // updates quad
            }
        }

    }   
}