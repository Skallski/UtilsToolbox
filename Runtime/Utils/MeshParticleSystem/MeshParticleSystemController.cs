using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SkalluUtils.Utils.MeshParticleSystem
{
    [RequireComponent(typeof(MeshParticleSystem))]
    public class MeshParticleSystemController : MonoBehaviour
    {
        [SerializeField] private MeshParticleSystem _meshParticleSystem;
        private readonly List<SingleParticle> _singleParticleList = new List<SingleParticle>();

        public bool _simulated; // are particles updated or not

#if UNITY_EDITOR
        private void Reset()
        {
            if (_meshParticleSystem == null)
                _meshParticleSystem = GetComponent<MeshParticleSystem>();
        }
#endif

        private void Awake()
        {
            if (_meshParticleSystem == null)
                _meshParticleSystem = GetComponent<MeshParticleSystem>();
        }

        private void Update()
        {
            if (!_simulated)
                return;

            for (var i = 0; i < _singleParticleList.Count; i++)
            {
                var particle = _singleParticleList[i];
                particle.UpdateParticle();

                if (particle.LifespanEnd())
                {
                    particle.DestroySelf();
                    _singleParticleList.RemoveAt(i);
                    i--;
                }
            }
        }

        #region SPAWNING SINGLE PARTICLE
        private void SpawnSingleParticle(SingleParticle singleParticle) => _singleParticleList.Add(singleParticle);

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
            SpawnSingleParticle(new SingleParticle(spawnPosition, rotation, size, _meshParticleSystem,
                uvIdx, movementSpeed, moveDirection, rotationSpeed, animationFrameRate, despawnTime));
        }
        
        public void SpawnSingleParticleRandom(Vector3 spawnPosition, float rotation, Vector2 size,
            float movementSpeed = 0, Vector3 moveDirection = default, float rotationSpeed = 0, float animationFrameRate = 0, float despawnTime = 0)
        {
            SpawnSingleParticle(new SingleParticle(spawnPosition, rotation, size, _meshParticleSystem,
                Random.Range(0, _meshParticleSystem.ParticleUvPixelsArray.Length), movementSpeed, moveDirection, rotationSpeed, animationFrameRate, despawnTime));
        }

        public void SpawnSingleParticleAnimated(Vector3 spawnPosition, float rotation, Vector2 size, float animationFrameRate)
        {
            SpawnSingleParticle(new SingleParticle(spawnPosition, rotation, size, _meshParticleSystem,
                0, 0, default, 0, animationFrameRate, 0));
        }
        #endregion

        /// <summary>
        /// Destroys particles
        /// </summary>
        public void DestroyParticles() => _meshParticleSystem.ResetMesh();

        /// <summary>
        /// Class representing single particle
        /// </summary>
        private class SingleParticle
        {
            private readonly MeshParticleSystem _meshParticleSystem;
            private int _uvIdx; // index from UV pixel array
            private readonly int _quadIdx; // id of the quad
            
            private Vector3 _position; // current position
            private float _rotation; // rotation angle (in degrees)
            private readonly Vector2 _size; // size [x, y]

            private float _movementSpeed;
            private Vector3 _moveDirection; // move direction
            private float _rotationSpeed;

            private float _animationTick;
            private float _animationTickTimer;

            private float _despawnTime;
            private float _despawnTimer;
            
            public SingleParticle(Vector3 position, float rotation, Vector2 size, MeshParticleSystem meshParticleSystem, int uvIdx = 0,
                float movementSpeed = 0, Vector3 moveDirection = default, float rotationSpeed = 0, float animationTick = 0, float despawnTime = 0)
            {
                _uvIdx = uvIdx >= 0 && uvIdx < meshParticleSystem.ParticleUvPixelsArray.Length ? uvIdx : 0;
                _position = position;
                _rotation = rotation;
                _size = size;
                _meshParticleSystem = meshParticleSystem;

                _movementSpeed = movementSpeed;
                _moveDirection = moveDirection;
                _rotationSpeed = rotationSpeed;
                _animationTick = animationTick;
                _despawnTime = despawnTime;
                
                _quadIdx = meshParticleSystem.AddQuad(position, rotation, size, uvIdx);
            }

            /// <summary>
            /// This method determines the particle behavior over time
            /// It is called every Update (every frame), so don't do heavy stuff here
            /// </summary>
            public void UpdateParticle()
            {
                // move particle over time
                if (_movementSpeed > 0)
                    _position += _moveDirection * (_movementSpeed * Time.deltaTime);

                // rotate particle over time
                if (_rotationSpeed > 0)
                    _rotation += 360f * (_movementSpeed * Time.deltaTime);

                // animate particle over time
                if (_animationTick > 0)
                {
                    _animationTickTimer += Time.deltaTime;
                    if (_animationTickTimer >= _animationTick)
                    {
                        _animationTickTimer -= _animationTick;
                        
                        if (_uvIdx == 7)
                        {
                            _uvIdx = 0;
                            _animationTick = 0;
                        }
                        else
                        {
                            _uvIdx++;
                        }
                    }
                }

                // destroy particle after some time
                if (_despawnTime > 0)
                    _despawnTimer += Time.deltaTime;

                _meshParticleSystem.UpdateQuad(_quadIdx, _position, _rotation, _size, _uvIdx); // updates quad
            }

            public bool LifespanEnd()
            {
                if (_despawnTime > 0)
                    return _despawnTimer >= _despawnTime;

                return false;
            }

            public void DestroySelf() => _meshParticleSystem.DestroyQuad(_quadIdx);
        }

    }   
}