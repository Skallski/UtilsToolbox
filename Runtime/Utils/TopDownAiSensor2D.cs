using System.Collections;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

namespace SkalluUtils.Utils
{
    public class TopDownAiSensor2D : MonoBehaviour
    {
        [SerializeField, ReadOnly] private bool _targetInsideViewOuterRadius, _targetSpotted, _targetInsideSafeZone, _targetInsideAttackRange;
        [SerializeField] private float _viewOuterRadius, _viewInnerRadius, _viewAngle;
        [SerializeField] private bool _useSpecialZones;
        [SerializeField] private float _safeZoneRadius, _attackRangeRadius;
        [SerializeField] private float _sensorTick = 0.02f;
        [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private GameObject _targetObject;

        #region SCENE GUI PARAMETERS
        // these variables are used by custom editor, so do not delete them even though the IDE says they are never used
        [SerializeField] private Color _mainColor, _safeZoneColor, _attackRangeColor;
        [SerializeField] private float _thickness = 0.5f;
        #endregion
        
        public bool TargetInsideViewOuterRadius => _targetInsideViewOuterRadius;
        public bool TargetSpotted => _targetSpotted;
        public bool TargetInsideSafeZone => _targetInsideSafeZone;
        public bool TargetInsideAttackRange => _targetInsideAttackRange;
        public GameObject TargetObject => _targetObject;

        private void Awake()
        {
            if (_targetObject == null)
                _targetObject = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start() => StartCoroutine(ScanForTargetRoutine());

        /// <summary>
        /// Performs all zone checks with some delay for the better performance.
        /// </summary>
        private IEnumerator ScanForTargetRoutine()
        {
            while (true)
            {
                CheckForVisibleTarget();

                if (_useSpecialZones) // do not perform "safe zone" and "attack range" checks if they aren't used
                {
                    if (_safeZoneRadius > 0) CheckSafeZone();
                    if (_attackRangeRadius > 0) CheckAttackRange();
                }

                yield return new WaitForSeconds(_sensorTick);
            }
        }
        
        /// <summary>
        /// Checks for visible targets inside view range and angle.
        /// </summary>
        private void CheckForVisibleTarget()
        {
            var currentPos = transform.position;
            var targetPos = _targetObject.transform.position;
            
            if (Vector2.SqrMagnitude(targetPos - currentPos) <= _viewOuterRadius * _viewOuterRadius) // when target is inside outer view radius
            {
                _targetInsideViewOuterRadius = true;
            
                var directionToTarget = (targetPos - currentPos).normalized;
                var distanceToTarget = Vector2.Distance(currentPos, targetPos);
                
                if (Vector2.SqrMagnitude(targetPos - currentPos) <= _viewInnerRadius * _viewInnerRadius) // when target is inside inner view radius
                {
                    // when raycast doesn't collide with any object from obstacle mask, it means, that target is spotted
                    _targetSpotted = !Physics2D.Raycast(currentPos, directionToTarget, distanceToTarget, _obstacleLayerMask);
                }
                else
                {
                    // when the target is inside view angle and raycast doesn't collide with any object from obstacle mask, it means, that target is spotted
                    _targetSpotted = Vector3.Angle(transform.right, directionToTarget) < _viewAngle * 0.5f
                                    && !Physics2D.Raycast(currentPos, directionToTarget, distanceToTarget, _obstacleLayerMask);
                }
            }
            else
            {
                _targetInsideViewOuterRadius = false;
                _targetSpotted = false;
            }
        }
        
        /// <summary>
        /// Checks if player is inside specific radius
        /// </summary>
        /// <param name="radius"> float value considered as "radius" to check (e.g. "safe zone" or "attack range") </param>
        /// <param name="obstacleMask"> LayerMask considered as obstacle layer, which is used during circle cast </param>
        private bool CheckIfTargetIsInsideSpecificRadius(float radius, LayerMask obstacleMask)
        {
            var currentPos = transform.position;
            var targetPos = _targetObject.transform.position;
            
            if (Vector2.SqrMagnitude(targetPos - currentPos) <= radius * radius) // when target is inside inner view radius
            {
                var directionToTarget = (targetPos - currentPos).normalized;
                var distanceToTarget = Vector3.Distance(currentPos, targetPos);
            
                if (!Physics2D.Raycast(currentPos, directionToTarget, distanceToTarget, obstacleMask)) return true;
            }
        
            return false;
        }
        
        /// <summary>
        /// Checks if player is inside character's "safe zone". Value of "target inside safe zone" flag is the result of check action.
        /// </summary>
        private void CheckSafeZone() => _targetInsideSafeZone = CheckIfTargetIsInsideSpecificRadius(_safeZoneRadius, _obstacleLayerMask);

        /// <summary>
        /// Checks if player is inside character's "attack range". Value of "target inside attack range" flag is the result of check action
        /// </summary>
        private void CheckAttackRange() => _targetInsideAttackRange = CheckIfTargetIsInsideSpecificRadius(_attackRangeRadius, _obstacleLayerMask);
        
    }
}