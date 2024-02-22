using UnityEngine;

namespace SkalluUtils.Utils.VFX.MeshParticleSystem.Particles
{
    public class MeshParticleTimed : MeshParticle
    {
        private float _lifespan;
        
        public MeshParticleTimed(MeshParticleSystem meshParticleSystem, Vector2 position, float rotation, Vector2 scale,
            float lifespan, int uvIdx = 0) : base(meshParticleSystem, position, rotation, scale, uvIdx)
        {
            _lifespan = lifespan;
        }

        protected override void OnUpdate()
        {
            if (_lifespan >= 0)
            {
                _lifespan -= Time.deltaTime;
            }
            else
            {
                IsParticleComplete = true;
            }
            
            if (IsParticleComplete)
            {
                DestroySelf();
            }
        }
    }
}