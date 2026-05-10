using System.Collections;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Try to play the provided cutscene coroutine. Returns true if accepted.
    /// </summary>
    public bool TryPlayCutscene(IEnumerator cutsceneRoutine)
    {
        if (IsPlaying || cutsceneRoutine == null)
            return false;

        StartCoroutine(RunCutscene(cutsceneRoutine));
        return true;
    }

    private IEnumerator RunCutscene(IEnumerator routine)
    {
        IsPlaying = true;
        yield return StartCoroutine(routine);
        IsPlaying = false;
    }
}
