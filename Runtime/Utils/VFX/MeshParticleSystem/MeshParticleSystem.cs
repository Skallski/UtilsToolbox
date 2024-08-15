using System;
using System.Collections.Generic;
using UnityEngine;

namespace UtilsToolbox.Utils.VFX.MeshParticleSystem
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshParticleSystem : MonoBehaviour
    {
        [Serializable]
        public struct ParticleUvPixels
        {
            [Tooltip("bottom-left texture corner (in pixels)")]
            public Vector2Int _uv00Pixels;
            
            [Tooltip("top-right texture corner (in pixels)")]
            public Vector2Int _uv11Pixels;
        }
        
        private struct UvCoordinates
        {
            public Vector2 uv00; // bottom-left texture corner (normalized value)
            public Vector2 uv11; // top-right texture corner (normalized value)
        }

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private int _sortingOrder = 1;
        [field: SerializeField] public ParticleUvPixels[] ParticleUvPixelsArray { get; private set; }

        private Mesh _mesh;
        private Vector3[] _verts;
        private Vector2[] _uvs;
        private int[] _tris;
        
        private UvCoordinates[] _uvCoordinatesArray;
        private int _currentQuadIdx;
        private const int MAX_QUAD_IDX = 15000; // max amount of quads that single mesh can handle
        private bool _updateMesh; // flag that allows mesh to update once par frame only when the update is needed

#if UNITY_EDITOR
        private void Reset()
        {
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
        }
#endif
        
        private void Awake()
        {
            SetupMesh();
        }

        private void LateUpdate()
        {
            if (_updateMesh)
            {
                UpdateMesh();
            }
        }

        #region MESH GENERATION
        /// <summary>
        /// Setup new mesh
        /// </summary>
        private void SetupMesh()
        {
            _currentQuadIdx = 0;
            _meshRenderer.sortingOrder = _sortingOrder;
            
            // calculates mesh related arrays
            _verts = new Vector3[4 * MAX_QUAD_IDX];
            _uvs = new Vector2[4 * MAX_QUAD_IDX];
            _tris = new int[6 * MAX_QUAD_IDX];

            // creates new mesh and apply calculated values to it
            _mesh = new Mesh {vertices = _verts, uv = _uvs, triangles = _tris};
            GetComponent<MeshFilter>().mesh = _mesh;
            
            // sets up UV Normalized Array
            var material = _meshRenderer.material;
            var mainTexture = material.mainTexture;
            var textureWidth = mainTexture.width;
            var textureHeight = mainTexture.height;
            
            // creates uv coordinates and adds them to the list
            var uvCoordinatesList = new List<UvCoordinates>();
            foreach (var particleUvPixels in ParticleUvPixelsArray)
            {
                uvCoordinatesList.Add(new UvCoordinates
                {
                    // calculates normalized texture UV Coordinates
                    uv00 = new Vector2((float) particleUvPixels._uv00Pixels.x / textureWidth, (float) particleUvPixels._uv00Pixels.y / textureHeight),
                    uv11 = new Vector2((float) particleUvPixels._uv11Pixels.x / textureWidth, (float) particleUvPixels._uv11Pixels.y / textureHeight)
                }); 
            }

            _uvCoordinatesArray = uvCoordinatesList.ToArray();
        }

        /// <summary>
        /// Updates mesh
        /// </summary>
        private void UpdateMesh()
        {
            if (_updateMesh == false)
            {
                return;
            }
            
            _mesh.vertices = _verts;
            _mesh.uv = _uvs;
            _mesh.triangles = _tris;
            _mesh.RecalculateBounds();

            _updateMesh = false;
        }
        
        /// <summary>
        /// Resets current mesh
        /// </summary>
        public void ResetMesh()
        {
            SetupMesh();
        }
        #endregion

        #region QUAD GENERATION
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
            if (_currentQuadIdx >= MAX_QUAD_IDX) // reset current quad index, when mesh is full
            {
                _currentQuadIdx = 0;
            }

            UpdateQuad(_currentQuadIdx, position, rotation, size, uvIndex);
                
            var spawnedQuadIdx = _currentQuadIdx;
            _currentQuadIdx += 1;

            return spawnedQuadIdx;
        }

        /// <summary>
        /// Updates quad values
        /// </summary>
        /// <param name="quadIdx"> index to identify quad to update </param>
        /// <param name="newPosition"> position to set </param>
        /// <param name="newRotation"> rotation to set </param>
        /// <param name="newSize"> size to set </param>
        /// <param name="uvIndex"> uv index to set </param>
        public void UpdateQuad(int quadIdx, Vector3 newPosition, float newRotation, Vector2 newSize, int uvIndex)
        {
            // update mesh vertices
            var vIdx = quadIdx * 4;
            var vIdx0 = vIdx;
            var vIdx1 = vIdx + 1;
            var vIdx2 = vIdx + 2;
            var vIdx3 = vIdx + 3;
            
            _verts[vIdx0] = newPosition + Quaternion.Euler(0, 0, newRotation - 180) * newSize; // left-bottom corner
            _verts[vIdx1] = newPosition + Quaternion.Euler(0, 0, newRotation - 270) * newSize; // left-top corner
            _verts[vIdx2] = newPosition + Quaternion.Euler(0, 0, newRotation - 0) * newSize; // right-top corner
            _verts[vIdx3] = newPosition + Quaternion.Euler(0, 0, newRotation - 90) * newSize; // right-bottom corner

            // update mesh UVs
            var uvCoordinates = _uvCoordinatesArray[uvIndex];
            _uvs[vIdx0] = uvCoordinates.uv00;
            _uvs[vIdx1] = new Vector2(uvCoordinates.uv00.x, uvCoordinates.uv11.y);
            _uvs[vIdx2] = uvCoordinates.uv11;
            _uvs[vIdx3] = new Vector2(uvCoordinates.uv11.x, uvCoordinates.uv00.y);
                
            // update mesh tris
            var tIdx = quadIdx * 6;
            
            _tris[tIdx + 0] = vIdx0;
            _tris[tIdx + 1] = vIdx1;
            _tris[tIdx + 2] = vIdx2;
            
            _tris[tIdx + 3] = vIdx0;
            _tris[tIdx + 4] = vIdx2;
            _tris[tIdx + 5] = vIdx3;
            
            _updateMesh = true;
        }

        /// <summary>
        /// Destroys quad
        /// </summary>
        /// <param name="quadIdx"> index to identify the quad to destroy</param>
        public void DestroyQuad(int quadIdx)
        {
            // destroy vertices
            var vIdx = quadIdx * 4;
            var vIdx0 = vIdx;
            var vIdx1 = vIdx + 1;
            var vIdx2 = vIdx + 2;
            var vIdx3 = vIdx + 3;
            
            _verts[vIdx0] = Vector3.zero; // left-bottom corner
            _verts[vIdx1] = Vector3.zero; // left-top corner
            _verts[vIdx2] = Vector3.zero; // right-top corner
            _verts[vIdx3] = Vector3.zero; // right-bottom corner

            _updateMesh = true;
        }
        #endregion
    }
}