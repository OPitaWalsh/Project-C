using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    public GameObject fpCamera;
    public Movement movement;
    [Space]
    public TextMeshProUGUI nameText;


    private void Start()
    {
        fpCamera.SetActive(photonView.IsMine);
        movement.enabled = photonView.IsMine;

        nameText.gameObject.SetActive(!photonView.IsMine);

        nameText.text = photonView.Owner.NickName;
    }

}
