using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlayTimeHandler
{
    public class GameTimeHandleSystem : MonoBehaviour
    {
        [Header("The TextMeshPro component that display time")]
        //public TMPro.TextMeshProUGUI TimeText;
        public UnityEngine.UI.Text TimeText;

        private static bool GameStop = false;

        private int second, minute, hour;

        public static void SetGameStopState(bool state)
        {
            GameStop = state;
        }
        /// <summary>
        /// Call this method in update to start display game time.
        /// </summary>
        public void CountGameTimeHandler()
        {
            if (!TimeText && GameStop)
                return;

            second = (int)Time.timeSinceLevelLoad;
            minute = second % 3600 / 60;
            hour = second / 3600;
            second = second % 3600 % 60;
            TimeText.text = hour + " : " +minute + " : "+ second;
        }
        private void Update()
        {
            CountGameTimeHandler();
        }
    }
}