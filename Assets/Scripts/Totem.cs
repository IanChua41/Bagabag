using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Totem : MonoBehaviour, IInteractable
{
    [Header("Video Settings")]
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Canvas videoCanvas;

    private void Start()
    {
        // Deactivate the canvas initially
        if (videoCanvas != null)
        {
            videoCanvas.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        Debug.Log("Playing cutscene...");
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // Activate the canvas
        if (videoCanvas != null)
        {
            videoCanvas.gameObject.SetActive(true);
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

        // Deactivate the canvas
        if (videoCanvas != null)
        {
            videoCanvas.gameObject.SetActive(false);
        }

        // Load Level2
        Debug.Log("Level finished.");
        SceneManager.LoadScene(6);
    }
}