using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Utils.MeshParticleSystem
{
    [RequireComponent(typeof(MeshParticleSystem))]
    public class MeshParticleSystemController : MonoBehaviour
    {
        [SerializeField] private MeshParticleSystem meshParticleSystem;
        private readonly List<SingleParticle> singleParticleList = new List<SingleParticle>();

        public bool simulated = false; // are particles updated or not

        private void Awake() => meshParticleSystem = GetComponent<MeshParticleSystem>();

        private void Update()
        {
            if (!simulated) return;

            for (var i = 0; i < singleParticleList.Count; i++)
            {
                var particle = singleParticleList[i];
                particle.UpdateParticle();

                if (particle.LifespanEnd())
                {
                    particle.DestroySelf();
                    singleParticleList.RemoveAt(i);
                    i--;
                }
            }
        }

        #region SPAWNING SINGLE PARTICLE
        private void SpawnSingleParticle(SingleParticle singleParticle) => singleParticleList.Add(singleParticle);

        /// <summary>
        /// Spawns single particle
        /// </summary>
        /// <param name="spawnPosition"> spawn position </param>
        /// <param name="rotation"> rotation </param>
        /// <param name="size"> size </param>
        /// <param name="uvIdx"></param>
        /// <param name="movementSpeed"> OPTIONAL</param>
        /// <param name="moveDirection"> OPTIONAL</param>
        /// <param name="rotationSpeed"> OPTIONAL</param>
        /// <param name="animationFrameRate"> OPTIONAL</param>
        /// <param name="despawnTime"> OPTIONAL</param>
        public void SpawnSingleParticle(Vector3 spawnPosition, float rotation, Vector2 size,
            int uvIdx = 0, float movementSpeed = 0, Vector3 moveDirection = default, float rotationSpeed = 0, float animationFrameRate = 0, float despawnTime = 0)
        {
            SpawnSingleParticle(new SingleParticle(spawnPosition, rotation, size, meshParticleSystem,
                uvIdx, movementSpeed, moveDirection, rotationSpeed, animationFrameRate, despawnTime));
        }
        
        public void SpawnSingleParticleRandom(Vector3 spawnPosition, float rotation, Vector2 size,
            float movementSpeed = 0, Vector3 moveDirection = default, float rotationSpeed = 0, float animationFrameRate = 0, float despawnTime = 0)
        {
            SpawnSingleParticle(new SingleParticle(spawnPosition, rotation, size, meshParticleSystem,
                Random.Range(0, meshParticleSystem.particleUvPixelsArray.Length), movementSpeed, moveDirection, rotationSpeed, animationFrameRate, despawnTime));
        }

        public void SpawnSingleParticleAnimated(Vector3 spawnPosition, float rotation, Vector2 size, float animationFrameRate)
        {
            SpawnSingleParticle(new SingleParticle(spawnPosition, rotation, size, meshParticleSystem,
                0, 0, default, 0, animationFrameRate, 0));
        }
        #endregion

        /// <summary>
        /// Destroys particles
        /// </summary>
        public void DestroyParticles() => meshParticleSystem.ResetMesh();

        /// <summary>
        /// Class representing single particle
        /// </summary>
        private class SingleParticle
        {
            private readonly MeshParticleSystem meshParticleSystem;
            private int uvIdx; // index from UV pixel array
            private readonly int quadIdx; // id of the quad
            
            private Vector3 position; // current position
            private float rotation; // rotation angle (in degrees)
            private Vector2 size; // size [x, y]

            private float movementSpeed;
            private Vector3 moveDirection; // move direction
            private float rotationSpeed;

            private float animationTick;
            private float animationTickTimer;

            private float despawnTime;
            private float despawnTimer;
            
            public SingleParticle(Vector3 position, float rotation, Vector2 size, MeshParticleSystem meshParticleSystem, int uvIdx = 0,
                float movementSpeed = 0, Vector3 moveDirection = default, float rotationSpeed = 0, float animationTick = 0, float despawnTime = 0)
            {
                this.uvIdx = uvIdx >= 0 && uvIdx < meshParticleSystem.particleUvPixelsArray.Length ? uvIdx : 0;
                this.position = position;
                this.rotation = rotation;
                this.size = size;
                this.meshParticleSystem = meshParticleSystem;

                this.movementSpeed = movementSpeed;
                this.moveDirection = moveDirection;
                this.rotationSpeed = rotationSpeed;
                this.animationTick = animationTick;
                this.despawnTime = despawnTime;
                
                quadIdx = meshParticleSystem.AddQuad(position, rotation, size, uvIdx);
            }

            /// <summary>
            /// This method determines the particle behavior over time
            /// It is called every Update (every frame), so don't do heavy stuff here
            /// </summary>
            public void UpdateParticle()
            {
                // move particle over time
                if (movementSpeed > 0) position += moveDirection * (movementSpeed * Time.deltaTime);

                // rotate particle over time
                if (rotationSpeed > 0) rotation += 360f * (movementSpeed * Time.deltaTime);

                // animate particle over time
                if (animationTick > 0)
                {
                    animationTickTimer += Time.deltaTime;
                    if (animationTickTimer >= animationTick)
                    {
                        animationTickTimer -= animationTick;
                        
                        if (uvIdx == 7)
                        {
                            uvIdx = 0;
                            animationTick = 0;
                        }
                        else
                        {
                            uvIdx++;
                        }
                    }
                }

                // destroy particle after some time
                if (despawnTime > 0) despawnTimer += Time.deltaTime;

                meshParticleSystem.UpdateQuad(quadIdx, position, rotation, size, uvIdx); // updates quad
            }

            public bool LifespanEnd()
            {
                if (despawnTime > 0)
                    return despawnTimer >= despawnTime;

                return false;
            }

            public void DestroySelf() => meshParticleSystem.DestroyQuad(quadIdx);
        }

    }   
}