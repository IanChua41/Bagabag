using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Totem : MonoBehaviour, IInteractable
{
    [Header("Video Settings")]
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private VideoPlayer videoPlayer;

    public void Interact()
    {
        Debug.Log("Playing cutscene...");
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
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
        Debug.Log("Level finished.");
        SceneManager.LoadScene(4);
    }
}
