using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviourPun
{
    [Header("Health Set Up")]
    public int maxHealth;
    private int health;

    [Header("UI Set Up")]
    public TextMeshProUGUI healthText;
    public Image healthFillImage;


    private void Start()
    {
        health = maxHealth;
        UpdateUI();
    }


    private void UpdateUI()
    {
        healthText.text = $"{health}/{maxHealth}";
        healthFillImage.fillAmount = (float)health / maxHealth;
    }


    [PunRPC]
    public void RPC_TakeDamage(int _damage)
    {
        health = Mathf.Max(0, health -= _damage);

        if (photonView.IsMine)
        {
            UpdateUI();

            /*
            if (health <= 0)
            {
                RoomManager.Instance.RespawnPlayer();
                PhotonNetwork.Destroy(gameObject);
            }
            */ //// NOT PROVIDED FOR
        }
        
    }
}
