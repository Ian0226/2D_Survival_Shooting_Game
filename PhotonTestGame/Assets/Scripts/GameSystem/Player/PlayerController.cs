using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;
using TMPro;


public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("Player input key")]
    public KeyCode MoveRight;
    public KeyCode MoveLeft;
    public KeyCode Jump;

    public float speed;
    public float jumpPower;
    public float bulletPower;
    public float hp;
    private float hpMax;

    [Header("The object of player")]
    public GameObject CharacterContainer;
    public GameObject HeadObj;
    public GameObject HeadLight;
    public GameObject WeaponContainer;
    public BoxCollider2D BoxCollider;

    [Header("UI objects")]
    [SerializeField]
    private Image hpImage;
    [SerializeField]
    private TextMeshProUGUI nameText;

    private Transform _transform;
    private PhotonView _pv;
    private Rigidbody2D _rb;
    private Camera _mainCamera;
    private CinemachineVirtualCamera _vCamera;
    private Animator _anim;
    [Header("This will place the initial weapon")]
    [SerializeField]
    private GameObject _weaponObj;
        
    private bool isGround;

    void Start()
    {
        _transform = this.transform;
        _pv = this.gameObject.GetComponent<PhotonView>();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        _vCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        _anim = CharacterContainer.GetComponentInChildren<Animator>();

        InitPlayerProperties();

        if (!_pv.IsMine)
        {
            Destroy(_rb);
        }

        nameText.text = _pv.Owner.NickName;

        hpMax = hp;
    }

    void Update()
    {
        if (!_pv.IsMine)
            return;
        PlayerInputControl();
        PlayerLookDirectControl();
        HandleShooting();
        PlayerJumpHandler();

        _vCamera.Follow = this.transform;
        //_mainCameraObj.transform.position = new Vector3(_transform.position.x, _transform.position.y, -10);
    }
    private void InitPlayerProperties()
    {
        //hp = 100;
    }
    void PlayerInputControl()
    {
        if (!Input.anyKey)
        {
            _anim.SetBool("isRun", false);
        }
        if (Input.GetKey(MoveLeft))
        {
            _transform.position += Vector3.left * speed * Time.deltaTime;
            _anim.SetBool("isRun", true);

        }
        if (Input.GetKey(MoveRight))
        {
            _transform.position += Vector3.right * speed * Time.deltaTime;
            _anim.SetBool("isRun", true);
                
        }
        if (Input.GetKeyDown(Jump) && isGround == true)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpPower);
            _anim.SetBool("isRun", false);
        }
        if (isGround)
        {
            _anim.SetBool("isJump", false);
        }
        else
        {
            _anim.SetBool("isJump", true);
            _anim.SetBool("isRun", false);
        }
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 offset = new Vector3(0.1f, 0, 0);
            GameObject bulletObj = PhotonNetwork.Instantiate("PhotonBullet", _transform.position + offset, Quaternion.identity);
            Rigidbody2D brb = bulletObj.GetComponent<Rigidbody2D>();
            brb.AddForce(new Vector2(bulletPower, 0));
        }*/
    }

    void PlayerLookDirectControl()
    {
        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // HeadObj
        Vector2 headLookDirection = (mousePos - (Vector2)HeadObj.transform.position).normalized;
        HeadObj.transform.right = headLookDirection;
        // WeaponObj
        Vector2 weaponLookDirection = (mousePos - (Vector2)WeaponContainer.transform.position).normalized;
        WeaponContainer.transform.right = weaponLookDirection;

        //«Ý¬ã¨s
        //float angle = Mathf.Atan2(weaponLookDirection.y, weaponLookDirection.x) * Mathf.Rad2Deg;

        if (mousePos.x < this.transform.position.x)
        {
            CharacterContainer.transform.rotation = Quaternion.Euler(0, 180, 0);
            WeaponContainer.transform.localScale = new Vector3(WeaponContainer.transform.localScale.x, -1f, WeaponContainer.transform.localScale.z);
            HeadObj.transform.localScale = new Vector3(HeadObj.transform.localScale.x, -1f, HeadObj.transform.localScale.z);
            HeadLight.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            CharacterContainer.transform.rotation = Quaternion.Euler(0, 0, 0);
            WeaponContainer.transform.localScale = new Vector3(WeaponContainer.transform.localScale.x, 1f, WeaponContainer.transform.localScale.z);
            HeadObj.transform.localScale = new Vector3(HeadObj.transform.localScale.x, 1f, HeadObj.transform.localScale.z);
            HeadLight.transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
    }

    private void HandleShooting()
    {
        WeaponController wc = _weaponObj.GetComponent<WeaponController>();
        if (wc.GetWeaponShootingMode() > 0)
        {
            if(Input.GetMouseButton(0))
                wc.OtherShootingMode();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                wc.Shooting();
            }
        }
    }

    private void PlayerJumpHandler()
    {
        if(_rb.velocity.y > 0)
        {
            BoxCollider.enabled = false;
        }
        else
        {
            if(BoxCollider.enabled == false)
                BoxCollider.enabled = true;
        }
    }

    public void TakeDamage(float damage)
    {
        if(_pv != null && _pv.IsMine)
        {
            hp -= damage;
            UpdateHpBar();
            /*HashTable table = new HashTable();
            table.Add("hp", hp);
            PhotonNetwork.LocalPlayer.SetCustomProperties(table);*/
            if (hp <= 0)
            {
                //GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>().GetPlayerList().Remove(this.transform);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_pv != null && _pv.IsMine)
        {
            /*if (other.gameObject.tag == "Bullet")
            {
                Bullet bullet = other.gameObject.GetComponent<Bullet>();
                if (!bullet.pv.IsMine)
                {
                    HashTable table = new HashTable();
                    hp -= 10;
                    table.Add("hp", hp);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(table);
                    if (hp <= 0)
                    {
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                }
            }*/
            if (other.gameObject.tag == "Ground")
            {
                isGround = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (_pv != null && _pv.IsMine)
        {
            if (other.gameObject.tag == "Ground")
            {
                isGround = false;
            }
        }
    }

    private void UpdateHpBar()
    {
        float percent = hp / hpMax;
        hpImage.transform.localScale = new Vector3(percent, hpImage.transform.localScale.y, hpImage.transform.localScale.z);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        /*if (targetPlayer == _pv.Owner)
        {
            if (changedProps.ContainsKey("hp"))
            {
                hp = (float)changedProps["hp"];
            }
            //print(targetPlayer.NickName + ":" + hp.ToString());
            UpdateHpBar();
        }*/
    }
}


