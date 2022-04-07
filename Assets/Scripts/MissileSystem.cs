using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    enum MyType
    {
        player,enemy,npc
    };
    [SerializeField]
    MyType myType=MyType.enemy;
    [SerializeField]
    Transform shootPoint;
    [SerializeField]
    float detectionRange, maxShootDelay;
    HelicopterController target;
    float distanceToPlayer;
    [SerializeField]
    bool playerInRange,checkDistance;
    GameObject instance;
    private void OnEnable()
    {
        // var temp;
        if(myType==MyType.enemy){
        target= GameObject.FindGameObjectWithTag("Player").GetComponent<HelicopterController>();
        }
        StartCoroutine(LaunchMissiles());
    }
/*  void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, detectionRange, transform.forward, out hit, 10))
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player In Zone");
                if(player==null)
                player=hit.collider.gameObject.GetComponent<HelicopterController>();
                checkDistance=true;
            }
        }
        if(checkDistance)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            Debug.Log("Distance till Player"+dist);
            if(dist>detectionRange+20)
            {
                Debug.Log("Player Out Of Detection Range");
                checkDistance=false;
                playerInRange=false;
            }
            else
            {
                Debug.Log("Player In Detection Range");
                playerInRange=true;
            }
        }
    }*/
    IEnumerator LaunchMissiles()
    {

        while (true)
        {
            Debug.Log("Missile System Active");
            yield return new WaitForSeconds(Random.Range(2,maxShootDelay));
            if(instance==null)
            {
                Debug.Log("Launching Missile");
                instance = Instantiate(Resources.Load("Missile", typeof(GameObject)),shootPoint.position,shootPoint.rotation) as GameObject;
                instance.GetComponent<Missile>().target=target;
            }
        }
    }
}
