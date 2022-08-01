using System.Collections;
using SkalluUtils.PropertyAttributes; // download package via package manager https://github.com/Skallu0711/Skallu-Utils.git
using UnityEngine;

namespace SkalluUtils.Utils
{
    public class TopDownFov2D : MonoBehaviour
    {
        #region FIELD OF VIEW STATES
        [ReadOnlyInspector] public bool targetInsideViewOuterRadius;
        [ReadOnlyInspector] public bool targetSpotted;
        [ReadOnlyInspector] public bool targetInsideSafeZone;
        [ReadOnlyInspector] public bool targetInsideAttackRange;
        #endregion

        # region FIELD OF VIEW PARAMETERS
        public float viewOuterRadius; // Area that determines the ability to detect target within it, provided that it is also within the viewing angle cone
        public float viewInnerRadius; // The minimum area that determines the ability to detect target within it
        public float viewAngle; // Angle (in degrees), which determines the ability to spot objects within its area

        public bool useSpecialZones;
        public float safeZoneRadius; // Radius of an optional safe zone area
        public float attackRangeRadius; // Radius of an optional attack range area
        
        public float zoneCheckInterval = 0.02f; // Time interval between zone checks (i.e. fov update)
        public LayerMask obstacleLayerMask; // Layer with all obstacles, which is used during circle cast. Enemy cannot see through obstacles
        public GameObject target; // Target object
        # endregion
        
        #region FOV EDITOR VISUAL PARAMETERS
        public Color mainFovColor = Color.white; // view radius and view angle handles color
        public Color safeZoneColor = Color.yellow; // safe zone handles color
        public Color attackRangeColor = Color.red; // attack range handles color
        public float thickness = 0.5f; // handles thickness

        public readonly Color targetSpottedColor = Color.green;
        public readonly Color targetHiddenColor = Color.red;
        #endregion

        private void Awake()
        {
            if (target == null)
                target = GameObject.FindGameObjectWithTag("Player");
        }

        private void Start() => StartCoroutine(PerformZonesChecksWithDelay());

        /// <summary>
        /// Performs all zone checks with some delay for the better performance.
        /// </summary>
        private IEnumerator PerformZonesChecksWithDelay()
        {
            var delayTime = zoneCheckInterval;

            while (true)
            {
                yield return new WaitForSeconds(delayTime);
                
                CheckForVisibleTarget();
            
                // do not perform "safe zone" and "attack range" checks if they aren't used
                if (useSpecialZones)
                {
                    if (safeZoneRadius > 0) 
                        CheckSafeZone();
                    if (attackRangeRadius > 0)
                        CheckAttackRange();
                }
            }
        }

        /// <summary>
        /// Checks for visible targets inside view range and angle.
        /// </summary>
        private void CheckForVisibleTarget()
        {
            if (Vector2.SqrMagnitude(target.transform.position - transform.position) <= viewOuterRadius * viewOuterRadius) // when target is inside outer view radius
            {
                targetInsideViewOuterRadius = true;
                
                var directionToTarget = (target.transform.position - transform.position).normalized;
                var distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

                if (Vector2.SqrMagnitude(target.transform.position - transform.position) <= viewInnerRadius * viewInnerRadius) // when target is inside inner view radius
                {
                    // when raycast doesn't collide with any object from obstacle mask, it means, that target is spotted
                    targetSpotted = !Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayerMask);
                }
                else
                {
                    // when the target is inside view angle and raycast doesn't collide with any object from obstacle mask, it means, that target is spotted
                    if (Vector3.Angle(transform.right, directionToTarget) < viewAngle * 0.5f)
                        targetSpotted = !Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayerMask);
                    else
                        targetSpotted = false;
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
            if (Vector2.SqrMagnitude(target.transform.position - transform.position) <= radius * radius) // when target is inside inner view radius
            {
                var directionToTarget = (target.transform.position - transform.position).normalized;
                var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                    return true;
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