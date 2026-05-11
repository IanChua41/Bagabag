using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndingMonster : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private bool allowTriggerContactForEnding = true;

    [Header("Follow Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float followSpeed = 2f;
    [SerializeField] private float maxVisionDistance = 20f;
    [SerializeField, Range(1f, 180f)] private float viewAngle = 90f;
    [SerializeField] private float eyeHeight = 1.5f;
    [SerializeField] private LayerMask visionBlockingLayers = ~0;

    [Header("Sight Audio")]
    [SerializeField] private AudioSource sightAudioSource;
    [SerializeField] private AudioClip spottedPlayerClip;
    [SerializeField, Range(0f, 1f)] private float spottedPlayerVolume = 1f;
    [SerializeField] private float spottedSoundCooldown = 2f;
    [SerializeField] private bool force2DSightAudio = true;
    [SerializeField] private bool debugSightAudio = false;

    [Header("Cutscene")]
    [SerializeField] private VideoPlayer cutscenePlayer;
    [SerializeField] private VideoClip cutsceneClip;
    [SerializeField] private Canvas cutsceneCanvas;
    [SerializeField] private GameObject cutsceneCanvasObject;
    [SerializeField] private float fallbackCutsceneDuration = 5f;

    private bool hasTriggered;
    private bool hadLineOfSightLastFrame;
    private float lastSpottedSoundTime = -999f;

    private void Start()
    {
        if (sightAudioSource != null)
        {
            sightAudioSource.playOnAwake = false;
        }

        FindPlayerIfNeeded();
    }

    private void Update()
    {
        if (hasTriggered)
        {
            return;
        }

        if (CutsceneManager.instance != null && CutsceneManager.instance.IsPlaying)
        {
            return;
        }

        FindPlayerIfNeeded();
        FollowPlayerIfVisible();
    }

    private void OnCollisionEnter(Collision other)
    {
        TryStartEnding(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!allowTriggerContactForEnding)
        {
            return;
        }

        TryStartEnding(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!allowTriggerContactForEnding)
        {
            return;
        }

        if (hasTriggered)
        {
            return;
        }

        // Helps when player starts inside the monster trigger.
        TryStartEnding(other.gameObject);
    }

    private void TryStartEnding(GameObject otherObject)
    {
        if (otherObject == null)
        {
            return;
        }

        if (!otherObject.CompareTag(playerTag))
        {
            return;
        }

        if (triggerOnce && hasTriggered)
        {
            return;
        }

        if (CutsceneManager.instance != null && CutsceneManager.instance.IsPlaying)
        {
            return;
        }

        if (CutsceneManager.instance != null)
        {
            if (!CutsceneManager.instance.TryPlayCutscene(PlayEndingCutscene()))
            {
                return;
            }
        }
        else
        {
            StartCoroutine(PlayEndingCutscene());
        }

        hasTriggered = true;
    }

    private void FindPlayerIfNeeded()
    {
        if (player != null)
        {
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void FollowPlayerIfVisible()
    {
        if (player == null)
        {
            return;
        }

        bool hasLineOfSight = HasLineOfSightToPlayer();
        HandleSightAudio(hasLineOfSight);

        if (!hasLineOfSight)
        {
            return;
        }

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        if (toPlayer.sqrMagnitude < 0.0001f)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(toPlayer.normalized);
        transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
    }

    private void HandleSightAudio(bool hasLineOfSight)
    {
        if (!hasLineOfSight)
        {
            hadLineOfSightLastFrame = false;
            return;
        }

        if (hadLineOfSightLastFrame)
        {
            return;
        }

        if (spottedPlayerClip == null)
        {
            if (debugSightAudio)
            {
                Debug.LogWarning("EndingMonster: Spotted Player Clip is not assigned.");
            }
            hadLineOfSightLastFrame = true;
            return;
        }

        if (Time.time - lastSpottedSoundTime < spottedSoundCooldown)
        {
            hadLineOfSightLastFrame = true;
            return;
        }

        PlaySpottedSound();

        lastSpottedSoundTime = Time.time;
        hadLineOfSightLastFrame = true;
    }

    private void PlaySpottedSound()
    {
        if (spottedPlayerClip == null)
        {
            return;
        }

        if (sightAudioSource != null)
        {
            if (!sightAudioSource.enabled)
            {
                sightAudioSource.enabled = true;
            }

            if (force2DSightAudio)
            {
                sightAudioSource.spatialBlend = 0f;
            }

            sightAudioSource.PlayOneShot(spottedPlayerClip, spottedPlayerVolume);

            if (debugSightAudio)
            {
                Debug.Log("EndingMonster: Played spotted sound via assigned AudioSource.");
            }
            return;
        }

        if (force2DSightAudio)
        {
            GameObject tempAudioObject = new GameObject("EndingMonsterSightAudio");
            Vector3 audioPosition = Camera.main != null ? Camera.main.transform.position : transform.position;
            tempAudioObject.transform.position = audioPosition;

            AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
            tempAudioSource.spatialBlend = 0f;
            tempAudioSource.PlayOneShot(spottedPlayerClip, spottedPlayerVolume);
            Destroy(tempAudioObject, spottedPlayerClip.length + 0.2f);

            if (debugSightAudio)
            {
                Debug.Log("EndingMonster: Played spotted sound via temporary 2D AudioSource.");
            }
            return;
        }

        AudioSource.PlayClipAtPoint(spottedPlayerClip, transform.position, spottedPlayerVolume);

        if (debugSightAudio)
        {
            Debug.Log("EndingMonster: Played spotted sound via PlayClipAtPoint.");
        }
    }

    private bool HasLineOfSightToPlayer()
    {
        if (player == null)
        {
            return false;
        }

        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 target = player.position + Vector3.up * eyeHeight;
        Vector3 direction = target - origin;
        float distance = direction.magnitude;

        if (distance > maxVisionDistance)
        {
            return false;
        }

        Vector3 horizontalDirection = direction;
        horizontalDirection.y = 0f;
        if (horizontalDirection.sqrMagnitude > 0.0001f)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, horizontalDirection.normalized);
            if (angleToPlayer > viewAngle * 0.5f)
            {
                return false;
            }
        }

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distance, visionBlockingLayers, QueryTriggerInteraction.Ignore))
        {
            return hit.transform == player || hit.transform.IsChildOf(player);
        }

        return true;
    }

    private IEnumerator PlayEndingCutscene()
    {
        SetCutsceneCanvasVisible(true);

        if (cutscenePlayer != null)
        {
            if (cutsceneClip != null)
            {
                cutscenePlayer.clip = cutsceneClip;
            }

            cutscenePlayer.Play();

            while (cutscenePlayer.isPlaying)
            {
                yield return null;
            }
        }
        else if (fallbackCutsceneDuration > 0f)
        {
            yield return new WaitForSeconds(fallbackCutsceneDuration);
        }

        SetCutsceneCanvasVisible(false);

        SceneManager.LoadSceneAsync(7);
    }

    private void SetCutsceneCanvasVisible(bool isVisible)
    {
        if (cutsceneCanvas != null)
        {
            cutsceneCanvas.gameObject.SetActive(isVisible);
            return;
        }

        if (cutsceneCanvasObject != null)
        {
            cutsceneCanvasObject.SetActive(isVisible);
        }
    }

    private void QuitGame()
    {
        Debug.Log("Ending cutscene finished. Quitting game.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
