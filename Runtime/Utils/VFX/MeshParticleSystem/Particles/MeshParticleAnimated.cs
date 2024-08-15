using UnityEngine;

namespace UtilsToolbox.Utils.VFX.MeshParticleSystem.Particles
{
    public class MeshParticleAnimated : MeshParticle
    {
        private float _animationTick;
        private float _animationTimer;
        private int _animationCycleEndFrameIndex;
        
        public MeshParticleAnimated(MeshParticleSystem meshParticleSystem, Vector2 position, float rotation, Vector2 scale,
            float animationTick, int animationCycleEndFrameIndex, int uvIdx = 0) : base(meshParticleSystem, position, rotation, scale, uvIdx)
        {
            _animationTick = animationTick;
            _animationCycleEndFrameIndex = animationCycleEndFrameIndex;

            _animationTimer = 0;

            IsParticleComplete = false;
        }

        protected override void OnUpdate()
        {
            if (_animationTick > 0)
            {
                _animationTimer += Time.deltaTime;
                if (_animationTimer >= _animationTick)
                {
                    _animationTimer -= _animationTick;
                    
                    if (UvIdx != _animationCycleEndFrameIndex)
                    {
                        UvIdx++;
                    }
                    else
                    {
                        IsParticleComplete = true; 
                    }
                }
            }

            if (IsParticleComplete)
            {
                DestroySelf();
            }
            
        }
    }
}