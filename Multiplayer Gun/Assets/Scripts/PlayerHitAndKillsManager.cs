using UnityEngine;

public class PlayerHitAndKillsManager : MonoBehaviour
{
    [Header("UI")]
    public Animation hitMarkerAnimation;
    public AudioSource hitMarkerAudioSource;
    [Space]
    public Animation killMarkerAnimation;
    public AudioSource killMarkerAudioSource;


    public void GetHit()
    {
        hitMarkerAnimation.Stop();
        hitMarkerAnimation.Play();

        hitMarkerAudioSource.Stop();
        hitMarkerAudioSource.Play();
    }


    public void GetKill()
    {
        killMarkerAnimation.Stop();
        killMarkerAnimation.Play();

        killMarkerAudioSource.Stop();
        killMarkerAudioSource.Play();
    }
}
