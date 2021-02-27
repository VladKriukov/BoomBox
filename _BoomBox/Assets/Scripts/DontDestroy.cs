using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroy[] others = FindObjectsOfType<DontDestroy>();
        if (others.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
