using UnityEngine;
using System.Collections;

namespace UMA
{
    /// <summary>
    /// Auxillary slot which adds player movement script on character creation.
    /// </summary>
    public class PlayerControllerSlotScript : MonoBehaviour
    {       
        public float TakedownFOV; 
        public void OnDnaApplied(UMAData umaData)
        {
            var playerController = GetComponent<PlayerController>();
            if (playerController == null)
            {
                playerController = umaData.gameObject.AddComponent<PlayerController>();
            }
            playerController.TakedownFOV = TakedownFOV;

        }
    }
}
