using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float fireRate = 10f;
    public int damagePerShot = 25;
    public float hitscanDistance = 500f;

    [Header("Animation Set Up")]
    public Animation anim;
    public AnimationClip shootClip;

    [Header("Hit Particle Set Up")]
    public GameObject concreteHitParticle;
    public GameObject playerHitParticle;

    [Header("Muzzle Flash Set Up")]
    public Transform muzzleFlashSpawnPoint;
    public GameObject muzzleFlashPrefab;

    [Header("Shoot SFX")]
    public PhotonPlayerSoundManager photonPlayerSoundManager;
    public byte shootSoundIndex = 0;

    [Header("Camera Reference")]
    public Transform cameraTramsform;

    private float timeUntilAllowNextShot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilAllowNextShot = Mathf.Max(0, timeUntilAllowNextShot - Time.deltaTime);

        if (Input.GetButton("Fire1") && timeUntilAllowNextShot <= 0)
        {
            HitscanShoot();
            timeUntilAllowNextShot = 1 / fireRate;
        }
    }


    void HitscanShoot()
    {
        anim.clip = shootClip;
        anim.Stop();
        anim.Play();

        photonPlayerSoundManager.photonView.RPC("RPC_PlayShootSound", RpcTarget.All, shootSoundIndex);

        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, muzzleFlashSpawnPoint.position, muzzleFlashSpawnPoint.rotation);
        muzzleFlash.transform.parent = muzzleFlashSpawnPoint;
        Destroy(muzzleFlash, 0.5f);

        if (Physics.Raycast(cameraTramsform.position, cameraTramsform.forward, out RaycastHit hit, hitscanDistance))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                hit.transform.GetComponent<PhotonView>().RPC("RPC_TakeDamage", RpcTarget.All, damagePerShot);
                PhotonNetwork.Instantiate(playerHitParticle.name, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                PhotonNetwork.Instantiate(concreteHitParticle.name, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
