using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using TMPro;
using System;

public class EnemyController : MonoBehaviourPunCallbacks
{
    [Header("Enemy Objects")]
    public GameObject EnemyBodyContainer;
    public Transform EnemyAttackRangeObjGroup;

    private Transform _enemyAttackLeft;
    private Transform _enemyAttackRight;

    [Header("Enemy properties")]
    [SerializeField] private float enemyHp;
    [SerializeField] private float damage;
    [SerializeField] private float attackRate;
    private float enemyHpMax;

    [Header("Enemy UI Properties")]
    //[SerializeField] private TextMeshProUGUI _hpTextUI;
    [SerializeField] private Image _hpImage;
    private GameObject hpUI;

    private PhotonView _pv;
    private Animator _anim;

    [Header(" ")]
    [SerializeField]
    private Transform _followTarget;
    private Transform _currentAttackTarget;

    private Action enemyCurrentAction;

    private EnemyFollowPlayerSystem enemyFollowPlayerSystem;

    private List<Transform> playerList;

    private int layerMaskPlayer;

    private float nextTimeToAttack = 0.0f;

    //For test
    [SerializeField]
    private GameObject rayHitObj;

    public Transform GetCurrentFollowTarget()
    {
        return _followTarget;
    }

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        //_hpTextUI.gameObject.SetActive(false);
        _anim = EnemyBodyContainer.GetComponentInChildren<Animator>();

        enemyFollowPlayerSystem = GetComponent<EnemyFollowPlayerSystem>();

        enemyCurrentAction = EnemyFollowPlayer;

        _enemyAttackLeft = EnemyAttackRangeObjGroup.transform.GetChild(0);
        _enemyAttackRight = EnemyAttackRangeObjGroup.transform.GetChild(1);

        layerMaskPlayer = 1 << LayerMask.NameToLayer("Player");

        enemyHpMax = enemyHp;

        hpUI = _hpImage.transform.parent.gameObject;

        playerList = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>().GetPlayerList();
        SetCurrentTarget(playerList);

        SetUIState(false);
    }

    private void Update()
    {
        if(enemyCurrentAction!=null)
            enemyCurrentAction();

        EnemyAttackHandler();

        if (_followTarget == null && PhotonNetwork.LocalPlayer.IsMasterClient)
            SetCurrentTarget(playerList);

        if (_pv.IsMine)
        {
            if (enemyHp <= 0)
            {
                PhotonNetwork.Destroy(this.gameObject);
                //_pv.RPC("Death", RpcTarget.All);
            }
        }
    }

    public void SetCurrentTarget(List<Transform> players)
    {
        if (players.Count > 0)
        {
            int targetIndex = UnityEngine.Random.Range(0, players.Count);
            _followTarget = players[targetIndex];
        }
    }

    private void EnemyFollowPlayer()
    {
        if (_currentAttackTarget == null && _followTarget != null && PhotonNetwork.LocalPlayer.IsMasterClient)
            enemyFollowPlayerSystem.MoveToTarget(_followTarget);

        if (EnemyHitPlayerHandler())
            enemyCurrentAction = EnemyAttackHandler;
    }

    //Use to detect if player is in this enemy's attack range
    private bool EnemyHitPlayerHandler()
    {
        bool isHit = false;
        RaycastHit2D hit = Physics2D.Raycast(_enemyAttackLeft.position, _enemyAttackRight.position - _enemyAttackLeft.position,
            (_enemyAttackRight.position - _enemyAttackLeft.position).magnitude, layerMaskPlayer);
        //These are for test
        if (hit)
            rayHitObj = hit.collider.gameObject;
        else
            rayHitObj = null;
        //Debug.DrawRay(_enemyAttackLeft.position, _enemyAttackRight.position- _enemyAttackLeft.position, Color.red);

        if (!hit || hit.collider.gameObject.tag != "Player")
        {
            isHit = false;
        }
        else
        {
            _currentAttackTarget = hit.collider.transform;
            isHit = true;
        }
        
        return isHit;
    }

    private void EnemyAttackHandler()
    {
        SetAttackAnimeState(true);

        if(Time.timeSinceLevelLoad >= nextTimeToAttack)
        {
            if (_currentAttackTarget != null)
            {
                _currentAttackTarget.gameObject.GetComponent<mainGame.PlayerController>().TakeDamage(this.damage);
            }
            nextTimeToAttack = Time.timeSinceLevelLoad + attackRate;
        }

        if (!EnemyHitPlayerHandler())
        {
            SetAttackAnimeState(false);
            _currentAttackTarget = null;
            enemyCurrentAction = EnemyFollowPlayer;
        }
    }

    private void SetAttackAnimeState(bool attackState)
    {
        _anim.SetBool("isAttack", attackState);
    }

    //Health method
    public float GetHealth()
    {
        return enemyHp;
    }

    public void TakeDamage(float damage)
    {
        enemyHp -= damage;
        //_hpTextUI.text = enemyHp.ToString();
        UpdateHpBar();
        _pv.RPC("SetNewHp", RpcTarget.Others, this.enemyHp);
        _pv.RPC("OnAttack", RpcTarget.All);
        //_pv.RPC("TakeDamageRPC", RpcTarget.Others, damage);
    }

    public void SetUIState(bool state)
    {
        hpUI.SetActive(state);
        //_hpTextUI.gameObject.SetActive(state);
        //_pv.RPC("SetUIStateRPC", RpcTarget.Others, state);
    }

    private void UpdateHpBar()
    {
        float percent = enemyHp / enemyHpMax;
        _hpImage.transform.localScale = new Vector3(percent, _hpImage.transform.localScale.y, _hpImage.transform.localScale.z);
    }

    [PunRPC]
    private void OnAttack()
    {
        _anim.SetTrigger("isOnDamage");
    }

    /*[PunRPC]
    private void SetUIStateRPC(bool state)
    {
        _hpTextUI.gameObject.SetActive(state);
    }*/

    [PunRPC]
    private void SetNewHp(float newHp)
    {
        enemyHp = newHp;
        //_hpTextUI.text = enemyHp.ToString();
    }

    /*
    [PunRPC]
    private void TakeDamageRPC(float damage)
    {
        enemyHp -= damage;
        _hpTextUI.text = enemyHp.ToString();
    }
    */


    [PunRPC]
    private void Death()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        } 
    }
}
