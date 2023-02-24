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
            public Vector2Int _uv00Pixels; // left-bottom texture corner (in pixels)
            public Vector2Int _uv11Pixels; // right-top texture corner (in pixels)
        }
        
        private struct UvCoordinates
        {
            public Vector2 uv00; // left-bottom texture corner (normalized value)
            public Vector2 uv11; // right-top texture corner (normalized value)
        }

        #region INSPECTOR VARIABLES
        [SerializeField] private int _sortingOrder = 1;
        [Space]
        [SerializeField] private ParticleUvPixels[] _particleUvPixelsArray;
        #endregion
        
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;
        private Vector3[] _verts;
        private Vector2[] _uv;
        private int[] _tris;
        
        private UvCoordinates[] _uvCoordinatesArray;
        private int _currentQuadIdx;
        private const int MAX_QUAD_IDX = 15000; // limiter
        private bool _updateVerts, _updateUv, _updateTris, _updateBounds = false;
        
        public ParticleUvPixels[] ParticleUvPixelsArray => _particleUvPixelsArray;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.sortingOrder = _sortingOrder;
            
            Setup();
        }

        private void LateUpdate() => UpdateMeshArrays();
        
        /// <summary>
        /// Setup new mesh
        /// </summary>
        private void Setup()
        {
            // calculates mesh related arrays
            _verts = new Vector3[4 * MAX_QUAD_IDX];
            _uv = new Vector2[4 * MAX_QUAD_IDX];
            _tris = new int[6 * MAX_QUAD_IDX];

            // creates new mesh and apply calculated values to it
            _mesh = new Mesh {vertices = _verts, uv = _uv, triangles = _tris};
            GetComponent<MeshFilter>().mesh = _mesh;
            
            // sets up UV Normalized Array
            var material = _meshRenderer.material;
            var mainTexture = material.mainTexture;
            var textureWidth = mainTexture.width;
            var textureHeight = mainTexture.height;
            
            // creates uv coordinates and adds them to the list
            var uvCoordinatesList = new List<UvCoordinates>();
            foreach (var particleUvPixels in _particleUvPixelsArray)
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
        /// Resets current mesh
        /// </summary>
        public void ResetMesh()
        {
            _currentQuadIdx = 0;
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
            if (_currentQuadIdx >= MAX_QUAD_IDX) // reset current quad index, when mesh is full
                _currentQuadIdx = 0;

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
            // update vertices
            var vIdx = quadIdx * 4;
            var vIdx0 = vIdx;
            var vIdx1 = vIdx + 1;
            var vIdx2 = vIdx + 2;
            var vIdx3 = vIdx + 3;
            
            _verts[vIdx0] = newPosition + Quaternion.Euler(0, 0, newRotation - 180) * newSize; // left-bottom corner
            _verts[vIdx1] = newPosition + Quaternion.Euler(0, 0, newRotation - 270) * newSize; // left-top corner
            _verts[vIdx2] = newPosition + Quaternion.Euler(0, 0, newRotation - 0) * newSize; // right-top corner
            _verts[vIdx3] = newPosition + Quaternion.Euler(0, 0, newRotation - 90) * newSize; // right-bottom corner

            // update uvs
            var uvCoordinates = _uvCoordinatesArray[uvIndex];
            _uv[vIdx0] = uvCoordinates.uv00;
            _uv[vIdx1] = new Vector2(uvCoordinates.uv00.x, uvCoordinates.uv11.y);
            _uv[vIdx2] = uvCoordinates.uv11;
            _uv[vIdx3] = new Vector2(uvCoordinates.uv11.x, uvCoordinates.uv00.y);
                
            // update triangles
            var tIdx = quadIdx * 6;

            _tris[tIdx + 0] = vIdx0;
            _tris[tIdx + 1] = vIdx1;
            _tris[tIdx + 2] = vIdx2;

            _tris[tIdx + 3] = vIdx0;
            _tris[tIdx + 4] = vIdx2;
            _tris[tIdx + 5] = vIdx3;
            
            // set flags to update mesh arrays
            _updateVerts = true;
            _updateUv = true;
            _updateTris = true;
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
            
            _updateVerts = true;
        }

        /// <summary>
        /// Update mesh arrays according to flag state
        /// </summary>
        private void UpdateMeshArrays()
        {
            if (_updateVerts)
            {
                _mesh.vertices = _verts;
                _updateVerts = false;

                _updateBounds = true;
            }

            if (_updateUv)
            {
                _mesh.uv = _uv;
                _updateUv = false;
                
                _updateBounds = true;
            }
            
            if (_updateTris)
            {
                _mesh.triangles = _tris;
                _updateTris = false;
                
                _updateBounds = true;
            }

            if (_updateBounds)
            {
                _mesh.RecalculateBounds();
                _updateBounds = false;
            }
        }
        
    }
}