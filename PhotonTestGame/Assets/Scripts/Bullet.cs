using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    public PhotonView pv;

    private float timer;
    void Start()
    {
        timer = 1;
        pv = this.gameObject.GetComponent<PhotonView>();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (!pv.IsMine)
        {
            Destroy(_rb);
        }
    }

    void Update()
    {
        if (pv.IsMine)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}

