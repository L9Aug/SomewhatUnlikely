using UnityEngine;
using System.Collections;


namespace UMA
{
    /// <summary>
    /// Auxillary slot which adds player movement script on character creation.
    /// </summary>
    public class PlayerMovementSlotScript : MonoBehaviour
    {
        public void OnDnaApplied(UMAData umaData)
        {
            var playerMovement = umaData.GetComponent<PlayerMovement>();
            if (playerMovement == null)
                umaData.gameObject.AddComponent<PlayerMovement>();
        }
    }

}
