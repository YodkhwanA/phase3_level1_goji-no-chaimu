//using System.Collections;
//using UnityEngine;

//public class PlayerFootsteps : MonoBehaviour
//{
//    public AudioClip footStepSFX;
//    private FPSMovement movement;

//    private void Start()
//    {
//        movement = GetComponent<FPSMovement>();
//        StartCoroutine(PlayerFootsteps());
//    }
//    private IEnumerator PlayerFootsteps()
//    {
//        while (true)
//        {
//            if (movement.movement.magnitude > 0.1f)
//            {
//                AudioManager.instance.PlaySFX(footStepSFX);
//            }
//            yield return new WaitForSeconds(0.35f);
//        }
//    }
//}
