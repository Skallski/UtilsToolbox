using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Utils.MeshParticleSystem
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshParticleSystem : MonoBehaviour
    {
        [Serializable]
        public struct ParticleUvPixels
        {
            public Vector2Int uv00Pixels; // left-bottom texture corner (in pixels)
            public Vector2Int uv11Pixels; // right-top texture corner (in pixels)
        }
        
        private struct UvCoordinates
        {
            public Vector2 uv00; // left-bottom texture corner (normalized value)
            public Vector2 uv11; // right-top texture corner (normalized value)
        }

        [SerializeField] private int sortingOrder = 1;
        [Space]

        #region UV RELATED VARIABLES
        public ParticleUvPixels[] particleUvPixelsArray;
        private UvCoordinates[] uvCoordinatesArray;
        #endregion

        #region MESH RELATED VARIABLES
        private MeshRenderer meshRenderer;
        private Mesh mesh;
        private Vector3[] verts;
        private Vector2[] uv;
        private int[] tris;
        #endregion

        #region QUAD INDEX AND LIMITER
        private int currentQuadIdx;
        private const int maxQuadIdx = 15000; // limiter
        #endregion

        #region MESH ARRAYS UPDATER FLAGS
        private bool updateVerts = false;
        private bool updateUv = false;
        private bool updateTris = false;
        private bool updateBounds = false;
        #endregion
        
        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.sortingOrder = sortingOrder;
            
            Setup();
        }

        private void LateUpdate() => UpdateMeshArrays();
        
        /// <summary>
        /// Setup new mesh
        /// </summary>
        private void Setup()
        {
            // calculates mesh related arrays
            verts = new Vector3[4 * maxQuadIdx];
            uv = new Vector2[4 * maxQuadIdx];
            tris = new int[6 * maxQuadIdx];

            // creates new mesh and apply calculated values to it
            mesh = new Mesh {vertices = verts, uv = uv, triangles = tris};
            GetComponent<MeshFilter>().mesh = mesh;
            
            // sets up UV Normalized Array
            var material = meshRenderer.material;
            var mainTexture = material.mainTexture;
            var textureWidth = mainTexture.width;
            var textureHeight = mainTexture.height;
            
            var uvCoordinatesList = new List<UvCoordinates>();
            foreach (var particleUvPixels in particleUvPixelsArray)
            {
                UvCoordinates uvCoordinates = new UvCoordinates
                {
                    // calculates normalized texture UV Coordinates
                    uv00 = new Vector2((float) particleUvPixels.uv00Pixels.x / textureWidth, (float) particleUvPixels.uv00Pixels.y / textureHeight),
                    uv11 = new Vector2((float) particleUvPixels.uv11Pixels.x / textureWidth, (float) particleUvPixels.uv11Pixels.y / textureHeight)
                };
                
                uvCoordinatesList.Add(uvCoordinates); // adds created uv coordinates to list
            }

            uvCoordinatesArray = uvCoordinatesList.ToArray();
        }
        
        /// <summary>
        /// Resets current mesh
        /// </summary>
        public void ResetMesh()
        {
            currentQuadIdx = 0;
            Setup();
        }

        /// <summary>
        /// Adds new quad
        /// </summary>
        /// <param name="position"> initial position </param>
        /// <param name="rotation"> initial rotation </param>
        /// <param name="size"> initial size </param>
        /// <param name="uvIndex"> uv index </param>
        /// <returns> index of spawned quad </returns>
        public int AddQuad(Vector3 position, float rotation, Vector2 size, int uvIndex)
        {
            if (currentQuadIdx >= maxQuadIdx) // reset current quad index, when mesh is full
                currentQuadIdx = 0;

            UpdateQuad(currentQuadIdx, position, rotation, size, uvIndex);
                
            var spawnedQuadIdx = currentQuadIdx;
            currentQuadIdx += 1;

            return spawnedQuadIdx;
        }

        /// <summary>
        /// Updates quad values
        /// </summary>
        /// <param name="quadIdx"> index to identify quad </param>
        /// <param name="newPosition"> position to set </param>
        /// <param name="newRotation"> rotation to set </param>
        /// <param name="newSize"> size to set </param>
        /// <param name="uvIndex"> uv index to set </param>
        public void UpdateQuad(int quadIdx, Vector3 newPosition, float newRotation, Vector2 newSize, int uvIndex)
        {
            // relocate vertices
            var vIdx = quadIdx * 4;
            var vIdx0 = vIdx;
            var vIdx1 = vIdx + 1;
            var vIdx2 = vIdx + 2;
            var vIdx3 = vIdx + 3;
            
            verts[vIdx0] = newPosition + Quaternion.Euler(0, 0, newRotation - 180) * newSize; // left-bottom corner
            verts[vIdx1] = newPosition + Quaternion.Euler(0, 0, newRotation - 270) * newSize; // left-top corner
            verts[vIdx2] = newPosition + Quaternion.Euler(0, 0, newRotation - 0) * newSize; // right-top corner
            verts[vIdx3] = newPosition + Quaternion.Euler(0, 0, newRotation - 90) * newSize; // right-bottom corner

            // uv
            var uvCoordinates = uvCoordinatesArray[uvIndex];
            uv[vIdx0] = uvCoordinates.uv00;
            uv[vIdx1] = new Vector2(uvCoordinates.uv00.x, uvCoordinates.uv11.y);
            uv[vIdx2] = uvCoordinates.uv11;
            uv[vIdx3] = new Vector2(uvCoordinates.uv11.x, uvCoordinates.uv00.y);
                
            // create triangles
            var tIdx = quadIdx * 6;

            tris[tIdx + 0] = vIdx0;
            tris[tIdx + 1] = vIdx1;
            tris[tIdx + 2] = vIdx2;

            tris[tIdx + 3] = vIdx0;
            tris[tIdx + 4] = vIdx2;
            tris[tIdx + 5] = vIdx3;
            
            // set flags to update mesh arrays
            updateVerts = true;
            updateUv = true;
            updateTris = true;
        }

        /// <summary>
        /// Update mesh arrays according to flag state
        /// </summary>
        private void UpdateMeshArrays()
        {
            if (updateVerts)
            {
                mesh.vertices = verts;
                updateVerts = false;

                updateBounds = true;
            }

            if (updateUv)
            {
                mesh.uv = uv;
                updateUv = false;
                
                updateBounds = true;
            }
            
            if (updateTris)
            {
                mesh.triangles = tris;
                updateTris = false;
                
                updateBounds = true;
            }

            if (updateBounds)
            {
                mesh.RecalculateBounds();
                updateBounds = false;
            }
        }
    }
}