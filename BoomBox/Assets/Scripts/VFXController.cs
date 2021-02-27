using UnityEngine;

public class VFXController : MonoBehaviour
{
    
    Animator animator;
    [SerializeField] Animator cameraAnimator;

    private void OnEnable() {
        BoxBehaviour.deathFx += PlayerDied;
        Bomb.onMove += Explosion;
    }

    private void OnDisable() {
        BoxBehaviour.deathFx -= PlayerDied;
        Bomb.onMove += Explosion;
    }

    private void Start() {
        animator = GetComponent<Animator>();
    }

    void PlayerDied()
    {
        if (GameSettings.vfx == true)
            animator.SetTrigger("PlayerDied");

        if (GameSettings.shakeOn)
            cameraAnimator.SetTrigger("Death");
    }

    void Explosion()
    {
        if (GameSettings.vfx == true)
            animator.SetTrigger("Explosion");

        if (GameSettings.shakeOn)
            cameraAnimator.SetTrigger("Explosion");
    }

}
