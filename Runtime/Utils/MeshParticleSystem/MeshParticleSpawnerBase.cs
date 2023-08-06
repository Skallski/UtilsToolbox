using System.Collections.Generic;
using SkalluUtils.Utils.MeshParticleSystem.Particles;
using UnityEngine;

namespace SkalluUtils.Utils.MeshParticleSystem
{
    [RequireComponent(typeof(MeshParticleSystem))]
    public abstract class MeshParticleSpawnerBase : MonoBehaviour
    {
        [SerializeField] protected MeshParticleSystem _meshParticleSystem;
        private readonly List<MeshParticle> _spawnedParticles = new List<MeshParticle>();

#if UNITY_EDITOR
        private void Reset()
        {
            if (_meshParticleSystem == null)
            {
                _meshParticleSystem = GetComponent<MeshParticleSystem>();
            }
        }
#endif

        protected void UpdateParticles()
        {
            for (var i = 0; i < _spawnedParticles.Count; i++)
            {
                var particle = _spawnedParticles[i];
                particle.UpdateSelf();
                    
                if (particle.IsParticleComplete)
                {
                    _spawnedParticles.RemoveAt(i);
                    i--;
                }
            }
        }
        
        /// <summary>
        /// Spawns single particle
        /// </summary>
        /// <param name="meshParticle"></param>
        public void SpawnSingleParticle(MeshParticle meshParticle)
        {
            _spawnedParticles.Add(meshParticle);
        }

        /// <summary>
        /// Spawns single particle
        /// </summary>
        /// <param name="spawnPosition"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="uvIdx"></param>
        public void SpawnSingleParticle(Vector3 spawnPosition, float rotation, Vector2 scale, int uvIdx = 0)
        {
            SpawnSingleParticle(
                new MeshParticle(_meshParticleSystem, spawnPosition, rotation, scale, uvIdx));
        }

        public void Clear()
        {
            _spawnedParticles.Clear();
            _meshParticleSystem.ResetMesh();
        }
    }
}