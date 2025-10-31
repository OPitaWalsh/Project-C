using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float fireRate = 10f;
    public int damagePerShot = 25;
    public float hitscanDistance = 500f;

    [Header("Ammo Set Up")]
    public int magSize = 30;
    public int currentAmmoInMag = 30;
    public TextMeshProUGUI ammoText;
    public Image ammoIndicator;

    [Header("Hit and Kills Manager")]
    public PlayerHitAndKillsManager hitKillManager;

    [Header("Animation Set Up")]
    public Animation anim;
    public AnimationClip shootClip;
    public AnimationClip reloadClip;

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
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilAllowNextShot = Mathf.Max(0, timeUntilAllowNextShot - Time.deltaTime);

        if (Input.GetButton("Fire1") && timeUntilAllowNextShot <= 0 && currentAmmoInMag > 0 && !isReloading())
        {
            HitscanShoot();
            timeUntilAllowNextShot = 1 / fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }


    private void UpdateUI()
    {
        ammoText.text = $"{currentAmmoInMag}/{magSize}";
        ammoIndicator.fillAmount = (float)currentAmmoInMag / magSize;
    }


    void HitscanShoot()
    {
        currentAmmoInMag--;
        UpdateUI();

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
                hit.transform.GetComponent<PhotonView>().RPC("RPC_TakeDamage", RpcTarget.AllBuffered, damagePerShot);
                PhotonNetwork.Instantiate(playerHitParticle.name, hit.point, Quaternion.LookRotation(hit.normal));

                if (hit.transform.GetComponent<PlayerHealth>().health <= 0)
                {
                    hitKillManager.GetKill();
                }
                else
                {
                    hitKillManager.GetHit();
                }
            }
            else
            {
                PhotonNetwork.Instantiate(concreteHitParticle.name, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }


    private void Reload()
    {
        anim.clip = reloadClip;
        anim.Stop();
        anim.Play();

        currentAmmoInMag = magSize;
        UpdateUI();
    }

    private bool isReloading()
    {
        return anim.isPlaying && anim.clip == reloadClip;
    }
}
