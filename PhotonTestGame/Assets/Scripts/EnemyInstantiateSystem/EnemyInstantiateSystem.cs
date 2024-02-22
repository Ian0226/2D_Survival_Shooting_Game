using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyInstantiateSys
{
    public class EnemyInstantiateSystem : MonoBehaviour
    {
        private static EnemyInstantiateSystem _instance;
        private GameObject _instantiatedObj;
        private void Awake()
        {
            _instance = this;
        }
        public static GameObject StartInstantiateSystem(GameObject obj, Vector3 position, float time)
        {
            if(_instance != null)
            {
                _instance.StartCoroutine(_instance.InsObj(obj, position, time));
            }
            return _instance._instantiatedObj;
        }

        public static void ShotdownInstantiateSystem()
        {
            _instance.StopAllCoroutines();
        }

        IEnumerator InsObj(GameObject obj,Vector3 position,float time)
        {
            while (true)
            {
                _instantiatedObj = Instantiate(obj, position, obj.transform.rotation);
                yield return new WaitForSeconds(time);
            }
        }
    }
}

