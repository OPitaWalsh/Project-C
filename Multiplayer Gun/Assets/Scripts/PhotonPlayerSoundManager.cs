using Photon.Pun;
using UnityEngine;

public class PhotonPlayerSoundManager : MonoBehaviourPun
{
    public AudioClip[] shootSFX;
    public AudioSource shootSource;

    [PunRPC]
    public void RPC_PlayShootSound(byte _index)
    {
        shootSource.clip = shootSFX[_index];
        shootSource.Stop();
        shootSource.Play();
    }
}
