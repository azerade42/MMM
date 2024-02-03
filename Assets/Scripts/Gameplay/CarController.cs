using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
   [SerializeField] private RailPath pathToFollow;
   [SerializeField] private AnimationCurve animationCurve;

   List<Vector3> pathPositions;

   int currentIndex;

   public void Init(int startingIndex, List<Vector3> path)
   {
      currentIndex = startingIndex;
      pathPositions = path;
   }

   // private void Start()
   // {
   //    pathPositions = pathToFollow.GetRailPath();

   //    currentIndex = 3; // remove later when spawning cars
   //    MoveToNextPosition();
   // }

   private void OnEnable()
   {
      LaneManager.HitPerfect += MoveToNextPosition;
   }

   private void OnDisable()
   {
      LaneManager.HitPerfect -= MoveToNextPosition;
   }

   private void MoveToNextPosition()
   {
      Vector3 startPos = pathPositions[currentIndex];
      if (currentIndex + 1 >= pathPositions.Count) currentIndex = 0;
      Vector3 endPos = pathPositions[currentIndex + 1];

      StartCoroutine(EaseToNextPosition(startPos, endPos, 0.5f));
      transform.LookAt(endPos, Vector3.up);
      currentIndex++;
   }

   private IEnumerator EaseToNextPosition(Vector3 startPos, Vector3 endPos, float easeTime)
   {
      float curTime = 0;
      
      while (curTime < easeTime)
      {
         curTime += Time.deltaTime;
         transform.position = Vector3.Lerp(startPos, endPos, animationCurve.Evaluate(curTime/easeTime));
         yield return null;
      }       
   }

   // testing different easing methods

   // public static float remap(float val, float in1, float in2, float out1, float out2)
   //  {
   //      return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
   //  }

   // private float EaseInOutElastic(float delta)
   // {
   //    float constant = 2 * Mathf.PI / 4.5f;
   //    float y = delta;
      
   //    y = delta < 0.5f ? Mathf.Pow(2, 20 * delta - 10) * Mathf.Sin((20 * delta - 11.125f) * constant) / 2 :
   //     Mathf.Pow(2, -20 * delta + 10) * Mathf.Sin((20 * delta - 11.125f) * constant) / 2 + 1;

   //    return y;
   // }

   // private float EaseInOutBack(float delta)
   // {
   //    float c1 = 0.70158f;
   //    float c2 = c1 * 1.525f;
   //    float y = delta;

   //    y = delta < 0.5f ? (Mathf.Pow(2 * delta, 2) * ((c2 + 1) * 2 * delta - c2)) / 2 :
   //       (Mathf.Pow(2 * delta - 2, 2) * ((c2 + 1) * (delta * 2 - 2) + c2) + 2) / 2;
      
   //    return y;
   // }
}
