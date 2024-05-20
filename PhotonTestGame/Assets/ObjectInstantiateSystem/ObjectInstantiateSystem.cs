using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectInstantiateSys
{
    public class ObjectInstantiateSystem : MonoBehaviour
    {
        private static ObjectInstantiateSystem _instance;
        private GameObject _instantiatedObj;

        public enum InstantiateMode
        {
            random,
            sequentailly
        }
        private void Awake()
        {
            _instance = this;
        }

        /// <summary>
        /// Instantiate a object in one place.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="position"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static GameObject StartInstantiateSystem(GameObject obj, Vector3 position, float time)
        {
            if(_instance != null)
            {
                _instance.StartCoroutine(_instance.InsObj(obj, position, time));
            }
            return _instance._instantiatedObj;
        }

        /// <summary>
        /// Instantiate object in mutiple position.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="positions"></param>
        /// <param name="time"></param>
        /// <param name="mode">Random or sequentailly instantiate in position list.</param>
        /// <returns></returns>
        public static GameObject StartInstantiateSystem(GameObject obj, List<Vector3> positions, float time, InstantiateMode mode)
        {
            if (_instance != null)
            {
                _instance.StartCoroutine(_instance.InsObjMutiplePosition(obj, positions, time, mode));
            }
            return _instance._instantiatedObj;
        }

        /// <summary>
        /// Only instantiate object one time.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static GameObject StartInstantiateSystemOneTime(GameObject obj, List<Vector3> positions)
        {
            int randInt = Random.Range(0, positions.Count);
            _instance._instantiatedObj = Instantiate(obj, positions[randInt], obj.transform.rotation);

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

        IEnumerator InsObjMutiplePosition(GameObject obj, List<Vector3> positions, float time, InstantiateMode mode)
        {
            while (true)
            {
                switch (mode)
                {
                    case InstantiateMode.random:
                        int randInt = Random.Range(0, positions.Count);
                        _instantiatedObj = Instantiate(obj, positions[randInt], obj.transform.rotation);
                        yield return new WaitForSeconds(time);
                        break;
                    case InstantiateMode.sequentailly:
                        int index = 0;
                        _instantiatedObj = Instantiate(obj, positions[index], obj.transform.rotation);
                        index++;
                        yield return new WaitForSeconds(time);
                        break;
                }
            }
        }
    }
}

