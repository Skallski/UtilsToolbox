using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UtilsToolbox.Utils.VFX.MeshParticleSystem.Particles
{
    /// <summary>
    /// Class representing single particle
    /// </summary>
    public class MeshParticle
    {
        public Vector2 Position { get; protected set; }
        public float Rotation { get; protected set; }
        public Vector2 Scale { get; protected set; }
        public MeshParticleSystem MeshParticleSystem { get; }
        public int UvIdx { get; protected set; }
        public int QuadIdx { get; }
        public bool IsParticleComplete { get; protected set; } = true;

        public MeshParticle(MeshParticleSystem meshParticleSystem, Vector2 position, float rotation, Vector2 scale, int uvIdx = 0)
        {
            MeshParticleSystem = meshParticleSystem;
            Position = position;
            Rotation = rotation;
            Scale = scale;
            UvIdx = Mathf.Clamp(uvIdx, 0, meshParticleSystem.ParticleUvPixelsArray.Length - 1);
            
            QuadIdx = meshParticleSystem.AddQuad(position, rotation, scale, uvIdx);
        }
        
        /// <summary>
        /// This method determines the particle behavior over time
        /// It is called every Update (every frame), so don't do heavy stuff here
        /// </summary>
        public void UpdateSelf()
        {
            if (IsParticleComplete)
            {
                return;
            }

            OnUpdate(); // stuff that happens when particle is updated

            MeshParticleSystem.UpdateQuad(QuadIdx, Position, Rotation, Scale, UvIdx); // updates quad
        }

        /// <summary>
        /// Called everytime particle is updated
        /// </summary>
        protected virtual void OnUpdate() {}

        public void DestroySelf([CanBeNull] Action onDestroy = null)
        {
            MeshParticleSystem.DestroyQuad(QuadIdx);
            
            onDestroy?.Invoke();
        }
    }
}