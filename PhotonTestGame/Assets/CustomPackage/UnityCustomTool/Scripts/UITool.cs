using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.CustomTool;

namespace Unity.CustomUITool
{
    public static class UITool
    {
        private static GameObject m_CanvasObj = null; //2D canvas in scene.

        /// <summary>
        /// Find UI in Canvas.
        /// </summary>
        /// <param name="UIName"></param>
        /// <returns></returns>
        public static GameObject FindUIGameObject(string UIName)
        {
            if (m_CanvasObj == null)
                m_CanvasObj = UnityTool.FindGameObject("Canvas");
            if (m_CanvasObj == null)
                return null;
            return UnityTool.FindChildGameObject(m_CanvasObj, UIName);
        }

        /// <summary>
        /// Get UI component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Container">Parent object of try to find object.</param>
        /// <param name="UIName">The name of UI object.</param>
        /// <returns>Return component you want to find.</returns>
        public static T GetUIComponent<T>(GameObject Container, string UIName) where T : UnityEngine.Component
        {
            //Find child gameObjet;
            GameObject ChildGameObject = UnityTool.FindChildGameObject(Container, UIName);
            if (ChildGameObject == null)
                return null;

            T tempObj = ChildGameObject.GetComponent<T>();
            if (tempObj == null)
            {
                Debug.LogWarning("元件[" + UIName + "]不是[" + typeof(T) + "]");
                return null;
            }
            return tempObj;
        }
    }
}
