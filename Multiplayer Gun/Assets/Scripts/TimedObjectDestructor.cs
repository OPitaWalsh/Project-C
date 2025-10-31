using UnityEngine;
using Photon.Pun;
using System.Collections;

public class TimedObjectDestructor : MonoBehaviourPun
{
    public float lifetime = 3f;


    void Start()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(DelayedDestroy());
        }
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(lifetime);
        PhotonNetwork.Destroy(gameObject);
    }
}
