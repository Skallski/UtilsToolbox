using UnityEngine;

namespace SkalluUtils.Utils.Performance.Culling
{
    [System.Serializable]
    public struct CullingBoundingBox
    {
        [field: SerializeField] public Vector3 BoundingBoxSize { get; set; }
        [field: SerializeField] public Vector3 BoundingBoxOffset { get; set; }
        public Bounds Bounds { get; private set; }

        public void Setup(Vector3 position)
        {
            Bounds = new Bounds(position + BoundingBoxOffset, BoundingBoxSize);
        }

        public void DrawBoundingBox(Vector3 position)
        {
            Gizmos.DrawWireCube(position + BoundingBoxOffset, BoundingBoxSize);
        }
    }
}