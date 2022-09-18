using System.Collections;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

namespace SkalluUtils.Utils
{
    public class TopDownAiSensor2D : MonoBehaviour
    {
        #region SENSOR FLAGS
        [SerializeField, ReadOnlyInspector] private bool targetInsideViewOuterRadius, targetSpotted, targetInsideSafeZone, targetInsideAttackRange;
        public bool TargetInsideViewOuterRadius => targetInsideViewOuterRadius;
        public bool TargetSpotted => targetSpotted;
        public bool TargetInsideSafeZone => targetInsideSafeZone;
        public bool TargetInsideAttackRange => targetInsideAttackRange;
        #endregion

        #region SENSOR PARAMETERS
        [SerializeField] private float viewOuterRadius, viewInnerRadius, viewAngle;
        [SerializeField] private bool useSpecialZones;
        [SerializeField] private float safeZoneRadius, attackRangeRadius;
        
        [SerializeField] private float sensorTick = 0.02f;
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private GameObject targetObject;
        public GameObject TargetObject => targetObject;
        #endregion
        
        #region SCENE GUI PARAMETERS
        // these variables are used by custom editor, so do not delete them even though the IDE says they are never used
        [SerializeField] private Color mainColor, safeZoneColor, attackRangeColor;
        [SerializeField] private float thickness = 0.5f;
        #endregion

        private void Awake()
        {
            if (targetObject == null)
                targetObject = GameObject.FindGameObjectWithTag("Player");
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

                if (useSpecialZones) // do not perform "safe zone" and "attack range" checks if they aren't used
                {
                    if (safeZoneRadius > 0) CheckSafeZone();
                    if (attackRangeRadius > 0) CheckAttackRange();
                }

                yield return new WaitForSeconds(sensorTick);
            }
        }
        
        /// <summary>
        /// Checks for visible targets inside view range and angle.
        /// </summary>
        private void CheckForVisibleTarget()
        {
            var currentPos = transform.position;
            var targetPos = targetObject.transform.position;
            
            if (Vector2.SqrMagnitude(targetPos - currentPos) <= viewOuterRadius * viewOuterRadius) // when target is inside outer view radius
            {
                targetInsideViewOuterRadius = true;
            
                var directionToTarget = (targetPos - currentPos).normalized;
                var distanceToTarget = Vector2.Distance(currentPos, targetPos);
                
                if (Vector2.SqrMagnitude(targetPos - currentPos) <= viewInnerRadius * viewInnerRadius) // when target is inside inner view radius
                {
                    // when raycast doesn't collide with any object from obstacle mask, it means, that target is spotted
                    targetSpotted = !Physics2D.Raycast(currentPos, directionToTarget, distanceToTarget, obstacleLayerMask);
                }
                else
                {
                    // when the target is inside view angle and raycast doesn't collide with any object from obstacle mask, it means, that target is spotted
                    targetSpotted = Vector3.Angle(transform.right, directionToTarget) < viewAngle * 0.5f
                                    && !Physics2D.Raycast(currentPos, directionToTarget, distanceToTarget, obstacleLayerMask);
                }
            }
            else
            {
                targetInsideViewOuterRadius = false;
                targetSpotted = false;
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
            var targetPos = targetObject.transform.position;
            
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
        private void CheckSafeZone() => targetInsideSafeZone = CheckIfTargetIsInsideSpecificRadius(safeZoneRadius, obstacleLayerMask);

        /// <summary>
        /// Checks if player is inside character's "attack range". Value of "target inside attack range" flag is the result of check action
        /// </summary>
        private void CheckAttackRange() => targetInsideAttackRange = CheckIfTargetIsInsideSpecificRadius(attackRangeRadius, obstacleLayerMask);
        
    }
}