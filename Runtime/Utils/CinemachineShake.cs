using Cinemachine;
using UnityEngine;

namespace SkalluUtils.Utils
{
    public class CinemachineShake : MonoBehaviour
    {
         public static CinemachineShake Self { get; private set; }

         private CinemachineVirtualCamera cinemachineVirtualCamera;
         private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
         private float startMagnitude;
         private float shakeTimer;
         private float shakeTimerTotal;

         private void Awake()
         {
             if (Self != null && Self != this)
             {
                 Destroy(gameObject);
             }
             else
             {
                 Self = this;
                 
                 cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
                 cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
             }
         }

         private void Start()
         {
             cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
             cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 5;
         }

         private void Update()
         {
             if (shakeTimer > 0)
             {
                 shakeTimer -= Time.deltaTime;
                 cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startMagnitude, 0, 1 - (shakeTimer / shakeTimerTotal));;
             }
         }
         
         public void ShakeCamera(float magnitude, float time)
         {
             cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = magnitude;

             startMagnitude = magnitude;
             shakeTimer = time;
             shakeTimerTotal = time;
         }
    }   
}