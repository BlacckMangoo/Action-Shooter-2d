using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] AudioClip shootSfx;
    [SerializeField] float fireRate;
    [SerializeField] Vector2 shootHorRecoil;
    [SerializeField] Vector2 shootUpRecoil;
    [SerializeField] Vector2 shootDiagonalRecoil;

    public bool canshoot;
    Quaternion bulletRotation;
    [SerializeField] AudioClip gunEquipSfx;
    [SerializeField] ParticleSystem gunfx;
    [SerializeField] ParticleSystem gunfxup;
    float time;
    [SerializeField] Transform bulletInstantiatePos;
    [SerializeField] Transform bulletInstantiatePosUp;
    PlayerMovement playerMovement;
    [SerializeField] Animator anim;
    PlayerAnimator playerAnimator;
    float xinput;

    WeaponManager weaponManager;


   

    private void Start()
    {

       
        playerMovement = GetComponent<PlayerMovement>();
        weaponManager  = FindObjectOfType<WeaponManager>();

       playerAnimator = FindAnyObjectByType<PlayerAnimator>();
    }

  

    
    private void Update()
    {
  
        xinput = Input.GetAxis("Horizontal");
        bulletRotation = GetComponent<Transform>().rotation;
        time += Time.deltaTime;

        if(Input.GetKey(KeyCode.C)  && !playerMovement.isRolling && !playerAnimator.isWallTouching  && !playerMovement.IsWallJumping && !playerMovement.IsDashing ) 
        {
            if( time > weaponManager.bulletPrefabs[weaponManager.currentBulletType].GetComponent<Bullet>().fireRate)
            {
               
                if (Input.GetKey(KeyCode.UpArrow) && xinput != 0 && !playerMovement.isRolling && !playerAnimator.isWallTouching && !playerMovement.IsWallJumping && !playerMovement.IsDashing)
                {
                    gunfx.Play();
                    ShootDiagonal();
                }
                else if (Input.GetKey(KeyCode.UpArrow) && xinput == 0 && !playerMovement.isRolling && !playerAnimator.isWallTouching && !playerMovement.IsWallJumping && !playerMovement.IsDashing)
                {
                    ShootUp(); gunfxup.Play();
                }
                else
                {
                    
                    ShootHor(); gunfx.Play();
                }
               

                time = 0;
            }

            anim.SetLayerWeight(1, 100);
            anim.SetLayerWeight(0, 0);
        }
        else
        {
            anim.SetLayerWeight(1, 0);
            anim.SetLayerWeight(0, 100);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            canshoot = !canshoot;
            SoundManager.instance.PlaySoundFX(0.5f,gunEquipSfx,transform);

        }
            
    }
    private void ShootHor()
    {
        GameObject bulletPrefab =  weaponManager.bulletPrefabs[ weaponManager.currentBulletType];
        SoundManager.instance.PlaySoundFX(0.1f, shootSfx, transform);
            anim.SetTrigger("shootHor");

        Recoil(shootHorRecoil);
        Instantiate(bulletPrefab, bulletInstantiatePos.position, bulletRotation*Quaternion.Euler(0,0,Random.Range(0,4)));
    }

    private void ShootUp()
    {
        GameObject bulletPrefab = weaponManager.bulletPrefabs[weaponManager.currentBulletType];
        SoundManager.instance.PlaySoundFX(0.1f, shootSfx, transform);
        anim.SetTrigger("shootUp");
        Instantiate(bulletPrefab, bulletInstantiatePosUp.position,bulletRotation*Quaternion.Euler(0,0,Random.Range(87,93)));
        Recoil(shootUpRecoil);
    }
  
    private void ShootDiagonal()
    {
        GameObject bulletPrefab = weaponManager.bulletPrefabs[weaponManager.currentBulletType];
        SoundManager.instance.PlaySoundFX(0.1f, shootSfx, transform);
        anim.SetTrigger("shootDiagonal");

        Recoil(shootHorRecoil);
        Instantiate(bulletPrefab, bulletInstantiatePos.position, bulletRotation*Quaternion.Euler(0,0,Random.Range(43,47)));
    }
    private void Recoil( Vector2 force )
    {
        if(transform.rotation.y == 180)
        {
            playerMovement.RB.AddForce(force*xinput, ForceMode2D.Impulse);
        }
       
    }


}
