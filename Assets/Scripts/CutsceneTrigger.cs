using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float audioVolume = 1f;

    [Header("Video Settings")]
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Trigger Settings")]
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private string triggerTag = "Car";

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering has the correct tag and hasn't already triggered
        if (other.CompareTag(triggerTag) && (!triggerOnce || !hasTriggered))
        {
            Debug.Log($"Cutscene trigger activated by: {other.gameObject.name}");
            hasTriggered = true;
            StartCoroutine(PlayCutscene());
        }
    }

    private IEnumerator PlayCutscene()
    {
        // Play audio
        if (audioClip != null)
        {
            Debug.Log("Playing audio: " + audioClip.name);
            Vector3 playPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(audioClip, playPosition, audioVolume);
            
            // Wait for audio to finish
            yield return new WaitForSeconds(audioClip.length);
            Debug.Log("Audio finished");
        }

        // Play video
        if (videoPlayer != null && videoClip != null)
        {
            Debug.Log("Playing video: " + videoClip.name);
            videoPlayer.clip = videoClip;
            videoPlayer.Play();
            
            // Wait for video to finish
            while (videoPlayer.isPlaying)
            {
                yield return null;
            }
            Debug.Log("Video finished");
        }

        // Load Level2
        Debug.Log("Loading Level2...");
        SceneManager.LoadScene("Level2");
    }
}
