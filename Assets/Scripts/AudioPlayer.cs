using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Cabinet")]
    [SerializeField]
    AudioClip shootingClip;

    [SerializeField]
    [Range(0f, 1f)]
    float cabinetVolume = 1f;

    public void PlayCabinetClip()
    {
        PlayClip(shootingClip, cabinetVolume);
    }

    void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
}
