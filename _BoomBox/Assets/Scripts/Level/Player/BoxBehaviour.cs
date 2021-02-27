using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{

    [SerializeField] TimerNode levitationTimer;
    [SerializeField] TimerNode noDangerTimer;
    [SerializeField] Color dangerNoDangerColour;
    [SerializeField] Color dangerDefaultColor;
    [SerializeField] TimerNode magnetTimer;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] GameObject deathSound;
    Vector3 initialPos;
    Rigidbody myRigidbody;
    BoxCollider myCollider;
    public static bool inGame = true;
    bool noDangers;
    Transform visuals;

    public delegate void OnLevelFinish();
    public static OnLevelFinish levelFinish;

    public delegate void OnDeath();
    public static OnDeath onDeath;

    public delegate void OnDeathFx();
    public static OnDeathFx deathFx;

    public delegate void OnBombBonus(Effect effect);
    public static OnBombBonus bombBonus;

    public delegate void OnHit(Vector3 position);
    public static OnHit hit;

    void Start()
    {
        initialPos = transform.position;
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<BoxCollider>();
        visuals = transform.GetChild(0);
        deathParticles.transform.parent = null;
        inGame = true;
    }

    void Update()
    {
        if (transform.position.y <= -5)
        {
            ResetBox();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (inGame)
        {
            if (other.gameObject.tag == "Danger" && noDangers == false)
            {
                visuals.gameObject.SetActive(false);
                myRigidbody.isKinematic = true;
                myCollider.enabled = false;
                deathParticles.transform.position = other.contacts[0].point;
                //deathParticles.transform.rotation = new Quaternion(other.contacts[0].normal.x - 90, other.contacts[0].normal.y, other.contacts[0].normal.z, 0);
                deathParticles.Play();
                deathFx?.Invoke();
                if (GameSettings.sfxOn) deathSound.SetActive(true);
                Invoke("ResetBox", 1.2f);
            }
            if (other.gameObject.tag == "Finish")
            {
                Debug.Log("Level complete!");
                levelFinish?.Invoke();
                inGame = false;
                if (GameSettings.sfxOn) other.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (other.gameObject.tag == "CheckPoint" && other.gameObject.GetComponent<CheckPoint>().flagged == false)
            {
                initialPos = other.transform.position;
                initialPos.y = other.transform.position.y + 1;
                other.gameObject.GetComponent<CheckPoint>().flagged = true;
            }
            if (other.gameObject.tag == "AntiBonus")
            {
                RemoveAllBonuses();
                hit?.Invoke(other.contacts[0].point);
            }
            if (other.gameObject.tag == "Untagged")
            {
                hit?.Invoke(other.contacts[0].point);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (inGame)
        {
            if (other.gameObject.tag == "Bonus")
            {
                if (other.gameObject.GetComponent<Bonus>())
                {
                    Bonus bonus = other.gameObject.GetComponent<Bonus>();
                    if (bonus.bonusType == Bonus.BonusType.levitation)
                    {
                        levitationTimer.StartTimer(bonus.effect.effectTime);
                    }
                    if (bonus.bonusType == Bonus.BonusType.bombCountBonus)
                    {
                        bombBonus?.Invoke(bonus.effect);
                    }
                    if (bonus.bonusType == Bonus.BonusType.removeAllDangers)
                    {
                        noDangerTimer.StartTimer(bonus.effect.effectTime);
                    }
                    if (bonus.bonusType == Bonus.BonusType.magnet)
                    {
                        magnetTimer.StartTimer(bonus.effect.effectTime);
                    }
                    if (bonus.bonusType == Bonus.BonusType.antiBonus)
                    {
                        RemoveAllBonuses();
                    }
                    bonus.StartTimer();
                }
            }
        }
    }

    void ResetBox()
    {
        if (inGame)
        {
            visuals.gameObject.SetActive(true);
            myRigidbody.isKinematic = false;
            myCollider.enabled = true;
            transform.position = initialPos;
            transform.rotation = Quaternion.identity;
            myRigidbody.Sleep();
            deathSound.SetActive(false);
            onDeath?.Invoke();
            
            RemoveAllBonuses();
        }
    }

    void RemoveAllBonuses()
    {
        foreach (Transform item in transform.GetChild(1))
        {
            item.GetComponent<TimerNode>().ResetTImer();
        }
    }

    public void ShowAllDangers(bool b)
    {
        noDangers = b;
        GameObject[] dangers = GameObject.FindGameObjectsWithTag("Danger");
        foreach (var item in dangers)
        {
            if (b)
            {
                item.GetComponent<MeshRenderer>().material.color = dangerNoDangerColour;
            }
            else
            {
                item.GetComponent<MeshRenderer>().material.color = dangerDefaultColor;
            }
            
        }
    }

    private void OnApplicationQuit()
    {
        if (inGame)
        {
            Debug.Log("Left before completing the level. Ragequit?");
        }
    }

}
