using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TriggerCutscene : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private string triggerTag = "Player";
    [SerializeField] private bool triggerOnce = true;

    [Header("Cutscene Content")]
    [SerializeField] private UnityEvent onCutsceneTriggered;
    [SerializeField] private AudioClip audioClip;
    [SerializeField, Range(0f, 1f)] private float audioVolume = 1f;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip videoClip;

    [Header("After Cutscene")]
    [SerializeField] private bool loadNextScene = true;
    [SerializeField] private string sceneToLoad;

    private bool hasTriggered;
    private bool isPlaying;

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaying)
        {
            return;
        }

        if (!other.CompareTag(triggerTag))
        {
            return;
        }

        if (triggerOnce && hasTriggered)
        {
            return;
        }

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        isPlaying = true;
        hasTriggered = true;

        onCutsceneTriggered?.Invoke();

        if (audioClip != null)
        {
            Vector3 audioPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(audioClip, audioPosition, audioVolume);
            yield return new WaitForSeconds(audioClip.length);
        }

        if (videoPlayer != null && videoClip != null)
        {
            videoPlayer.clip = videoClip;
            videoPlayer.Play();

            while (videoPlayer.isPlaying)
            {
                yield return null;
            }
        }

        if (loadNextScene)
        {
            if (!string.IsNullOrWhiteSpace(sceneToLoad))
            {
                SceneManager.LoadSceneAsync(sceneToLoad);
            }
            else
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        if (triggerOnce)
        {
            Destroy(gameObject);
            yield break;
        }

        isPlaying = false;
    }
}
