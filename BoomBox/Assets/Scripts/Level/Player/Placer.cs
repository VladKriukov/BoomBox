using UnityEngine;

public class Placer : MonoBehaviour
{
    [SerializeField] Bomb itemToPlace;
    [SerializeField] float rechargeRate = 1.4f;
    Ray ray;
    RaycastHit hit;
    Camera cam;
    //MeshRenderer bombRenderer;

    float localRechargeRate;
    int count;

    float timer;

    bool inGame;

    void OnEnable()
    {
        CameraController.levelStart += LevelBegin;
        BoxBehaviour.levelFinish += LevelFinished;
        BoxBehaviour.bombBonus += OnBombBonus;
    }

    void OnDisable()
    {
        CameraController.levelStart -= LevelBegin;
        BoxBehaviour.levelFinish -= LevelFinished;
    }

    void OnBombBonus(Effect effect)
    {
        count += effect.effectAmount;
        localRechargeRate = 0;
    }

    void Awake()
    {
        cam = Camera.main;
        if (GameSettings.bombDelay == false)
        {
            rechargeRate = 0;
        }
        localRechargeRate = rechargeRate;
        //bombRenderer = itemToPlace.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (inGame == true)
        {
            PlaceBomb();
        }
    }

    void PlaceBomb()
    {
        int layerMask = 1 << 8;
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            //MakeBombVisible(true);
            Vector3 pos = new Vector3(0, hit.point.y, hit.point.z);
            itemToPlace.transform.position = pos;
            if (Input.GetMouseButtonUp(0) && timer <= 0)
            {
                itemToPlace.BombPlaced();
                timer = localRechargeRate;
                if (count > 0)
                {
                    count--;
                }
                else
                {
                    localRechargeRate = rechargeRate;
                }
                //MakeBombVisible(false);
            }
            //print(hit.collider.name);
        }
        else
        {
            //MakeBombVisible(false);
        }
    }

    /*
    void MakeBombVisible(bool visibility)
    {
        //itemToPlace.gameObject.SetActive(visibility);
        bombRenderer.enabled = visibility;
    }
    */

    void LevelBegin()
    {
        inGame = true;
    }

    void LevelFinished()
    {
        inGame = false;
        //MakeBombVisible(false);
    }

}
