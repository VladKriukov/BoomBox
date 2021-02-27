using UnityEngine;

public class ParticleDisabler : MonoBehaviour
{
    float disableTime = 0.8f;
    
    void OnEnable() {
        Invoke(nameof(DisableSelf), 0.8f);
    }

    void DisableSelf()
    {
        gameObject.SetActive(false);
    }

}
