using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    [SerializeField] Transform[] destinations;
    [SerializeField] float speed;
    [SerializeField] float waitingTime;
    [SerializeField] bool pingPong;
    [SerializeField] bool reverse;
    bool movingForward = true;
    float timeToWait;
    int index;
    Vector3 destination;
    Rigidbody rb;

    void Start() {
        transform.position = destinations[0].position;
        rb = GetComponent<Rigidbody>();
        SwitchDestination();
    }

    void FixedUpdate() {
        if (timeToWait <= 0)
        {
            rb.velocity = Vector3.Normalize((destination - transform.position)) * speed * Time.deltaTime;
            //Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector3.Distance(destination, transform.position) <= (0.0005f * speed))
            {
                timeToWait = waitingTime;
                SwitchDestination();
            }
        }
        else
        {
            timeToWait -= Time.deltaTime;
        }
    }

    void SwitchDestination()
    {
        if (pingPong == false)
        {
            AddToIndexer(1);

            if(index >= destinations.Length)
                index = 0;
            if(index < 0)
                index = destinations.Length - 1;
        }
        else
        {
            if(reverse)
                reverse = false;
            
            if (movingForward)
            {
                AddToIndexer(1);
                if (index >= destinations.Length)
                {
                    movingForward = false;
                    AddToIndexer(-2);
                }
                    
            }
            else
            {
                AddToIndexer(-1);
                if (index == 0)
                    movingForward = true;
                    
            }
        }
        
        destination = destinations[index].position;
    }

    void AddToIndexer(int amount)
    {
        if (reverse)
        {
            index -= amount;
        }
        else
        {
            index += amount;
        }
    }

}
