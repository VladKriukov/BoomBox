using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] float startDelay = 2.0f;
    [SerializeField] float cinematicDelay = 0.5f;

    public delegate void OnLevelStart();
    public static OnLevelStart levelStart;

    public UnityEngine.Events.UnityEvent OnCinematicStarted;

    void OnEnable()
    {
        LevelController.levelLoaded += StartCinematicCamera;
    }

    void OnDisable()
    {
        LevelController.levelLoaded -= StartCinematicCamera;
    }

    void StartCinematicCamera()
    {
        Invoke("StartCamera", cinematicDelay);
    }

    void StartCamera()
    {
        OnCinematicStarted.Invoke();
        Invoke("StartLevel", startDelay);
    }

    void StartLevel()
    {
        levelStart?.Invoke();
    }
}
