using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlayTimeHandler
{
    public class GameTimeHandleSystem : MonoBehaviour
    {
        [Header("The TextMeshPro component that display time")]
        public TMPro.TextMeshProUGUI TimeText;

        /// <summary>
        /// Call this method in update to start display game time.
        /// </summary>
        public void CountGameTimeHandler()
        {
            if (!TimeText)
                return;
            TimeText.text = Time.timeSinceLevelLoad.ToString();
        }
    }
}