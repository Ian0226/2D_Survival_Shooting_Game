using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace EnemyInstantiateSysForPhoton
{
    public class EnemyInitSystemForPhoton : MonoBehaviour
    {
        private static EnemyInitSystemForPhoton _instance;
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
        public static GameObject StartInstantiateSystem(GameObject obj, Vector3 position, float time)
        {
            if (_instance != null)
            {
                _instance.StartCoroutine(_instance.InsObj(obj, position, time));
            }
            return _instance._instantiatedObj;
        }

        public static GameObject StartInstantiateSystem(GameObject obj,List<Vector3> positions,float time,InstantiateMode mode)
        {
            if (_instance != null)
            {
                _instance.StartCoroutine(_instance.InsObjMutiplePosition(obj, positions, time, mode));
            }
            return _instance._instantiatedObj;
        }

        public static GameObject StartInstantiateSystemOneTime(GameObject obj, List<Vector3> positions)
        {
            int randInt = Random.Range(0, positions.Count);
            _instance._instantiatedObj = PhotonNetwork.Instantiate(obj.name, positions[randInt], obj.transform.rotation);

            return _instance._instantiatedObj;
        }

        public static void ShotdownInstantiateSystem()
        {
            _instance.StopAllCoroutines();
        }
        public static void ShotdownInstantiateSystem(IEnumerator enumerator)
        {
            _instance.StopCoroutine(enumerator);
        }

        IEnumerator InsObj(GameObject obj, Vector3 position, float time)
        {
            while (true)
            {
                _instantiatedObj = PhotonNetwork.Instantiate(obj.name, position, obj.transform.rotation);
                yield return new WaitForSeconds(time);
            }
        }

        IEnumerator InsObjMutiplePosition(GameObject obj,List<Vector3> positions,float time, InstantiateMode mode)
        {
            while (true)
            {
                switch (mode)
                {
                    case InstantiateMode.random:
                        int randInt = Random.Range(0, positions.Count);
                        _instantiatedObj = PhotonNetwork.Instantiate(obj.name, positions[randInt], obj.transform.rotation);
                        yield return new WaitForSeconds(time);
                        break;
                    case InstantiateMode.sequentailly:
                        int index = 0;
                        _instantiatedObj = PhotonNetwork.Instantiate(obj.name, positions[index], obj.transform.rotation);
                        index++;
                        yield return new WaitForSeconds(time);
                        break;
                }
            }
        }
    }
}

