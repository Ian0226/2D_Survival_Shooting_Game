using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.CustomTool
{
    public static class UnityTool
    {
        /// <summary>
        /// Find object in scene.
        /// </summary>
        /// <param name="GameObjectName">GameObject name that you want to find.</param>
        /// <returns></returns>
        public static GameObject FindGameObject(string GameObjectName)
        {
            GameObject pTmpGameObj = GameObject.Find(GameObjectName);
            if (pTmpGameObj == null)
            {
                Debug.LogWarning("場景中找不到GameObject[" + GameObjectName + "]物件");

                return null;
            }
            return pTmpGameObj;
        }

        /// <summary>
        /// Find child object in container.
        /// </summary>
        /// <param name="Container">Parent object of try to find gameObject.</param>
        /// <param name="gameObjectName">The object you want to find.</param>
        /// <returns></returns>
        public static GameObject FindChildGameObject(GameObject Container, string gameObjectName)
        {
            if (Container == null)
            {
                Debug.LogError("NGUICustomTools.GetChild:Container - null");

                return null;
            }

            Transform pGameObjectTF = null;

            //Is container object itself or not.
            if (Container.name == gameObjectName)
            {
                pGameObjectTF = Container.transform;
            }
            else
            {
                Transform[] allChildren = Container.transform.GetComponentsInChildren<Transform>();
                foreach (Transform child in allChildren)
                {
                    if (child.name == gameObjectName)
                    {
                        if (pGameObjectTF == null)
                            pGameObjectTF = child;
                        else
                            Debug.LogWarning("Container[" + Container.name + "[下找出重複的物件名稱[" + gameObjectName + "]");
                    }
                }
            }

            //Find nothing.
            if (pGameObjectTF == null)
            {
                Debug.Log("物件[" + Container.name + "]找不到子物件[" + gameObjectName + "]");

                return null;
            }

            return pGameObjectTF.gameObject;
        }
    }
}
