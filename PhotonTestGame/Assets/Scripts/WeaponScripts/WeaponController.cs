using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponController : MonoBehaviourPunCallbacks
{
    public AudioSource GunAudioSource;
    private Animator _anime;
    
    [Header("Gun properties")]
    [Range(0, 2)]
    //Weapon shotting mode,0 is single,1 is 3-round burst,2 is full auto.
    [SerializeField] private int weaponShootingMode;

    [SerializeField] private float damage = 0.0f;

    //Time between two shot.
    [SerializeField] private float shootingRate;

    //Bullet trail.
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private float shootingRange = 10f;

    private float nextTimeToShoot;

    private PhotonView _pv;

    int layerMaskEnemy;
    int layerMaskShooting;

    private EnemyController enemy;

    private void Start()
    {
        _anime = GetComponent<Animator>();
        _pv = this.GetComponent<PhotonView>();

        layerMaskEnemy = 1 << LayerMask.NameToLayer("Enemy");
        layerMaskShooting = 1 << LayerMask.NameToLayer("Enemy") | 1 <<  LayerMask.NameToLayer("Wall");
    }
    private void Update()
    {
        AimOnEnemyHandler();
    }
    public void Shooting()
    {
        _pv.RPC("SyncShooting", RpcTarget.All);
    }
    public void OtherShootingMode()
    {
        if (weaponShootingMode == 2)
        {
            if (Time.timeSinceLevelLoad >= nextTimeToShoot)
            {
                _pv.RPC("SyncShooting", RpcTarget.All);
                nextTimeToShoot = Time.timeSinceLevelLoad + shootingRate;
            }
        }
    }

    private void AimOnEnemyHandler()
    {
        RaycastHit2D currentAimEnemy = Physics2D.Raycast(_shootingPoint.position, transform.right, shootingRange, layerMaskEnemy);

        if (currentAimEnemy.collider == null)
        {
            if(enemy != null)
                enemy.SetUIState(false);
        }
        else
        {
            if (enemy != null)
                enemy.SetUIState(false);
            enemy = currentAimEnemy.transform.gameObject.GetComponent<EnemyController>();
            enemy.SetUIState(true);
        }
    }

    [PunRPC]
    private void SyncShooting()
    {
        _anime.SetTrigger("Shoot");
        GunAudioSource.PlayOneShot(GunAudioSource.clip);

        RaycastHit2D hit = Physics2D.Raycast(_shootingPoint.position, transform.right,shootingRange, layerMaskShooting);
        //Test
        //Debug.DrawRay(_shootingPoint.position, transform.right*shootingRange, Color.red,5f);

        GameObject trail = Instantiate(_bulletTrail, _shootingPoint.position, transform.rotation);

        BulletTrailController bulletTrail = trail.GetComponent<BulletTrailController>();
        if (hit.collider == null)
        {
            Vector3 endPosition = _shootingPoint.position + transform.right * shootingRange;
            bulletTrail.SetTargetPosition(endPosition);
            return;
        }
        if(hit.collider != null)
        {
            HandleHitObj(hit,hit.collider.gameObject,trail,bulletTrail);
        }
    }
    private void HandleHitObj(RaycastHit2D hit,GameObject hitObj, GameObject trail, BulletTrailController bulletTrail)
    {
        Vector3 endPosition;
        switch (hitObj.tag)
        {
            case "Enemy":
                bulletTrail.SetTargetPosition(hit.point);
                EnemyController enemy = hitObj.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
                break;
            case "Player":
            case "Ground":
            case "Untagged":
                endPosition = _shootingPoint.position + transform.right * shootingRange;
                bulletTrail.SetTargetPosition(endPosition);
                break;
            case "Wall":
                bulletTrail.SetTargetPosition(hit.point);
                break;
        }
    }
    public int GetWeaponShootingMode()
    {
        return weaponShootingMode;
    }
}
