using UnityEngine;

public class Environment : MonoBehaviour
{
   [ContextMenu("RotateAllEnvironment")]
   private void RotateAllEnvironment() {
      for (int i = 0; i < transform.childCount; i++)
      {
         var environmentElement = transform.GetChild(i);
         var randomAngle = environmentElement.transform.rotation;
         randomAngle.eulerAngles = new Vector3(0, Random.Range(1, 359), 0);
         environmentElement.transform.rotation = randomAngle;
      }      
   }
}
