using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject[] locations;
    [SerializeField] GameObject[] searchObjects;
    [SerializeField] GameObject player;
    [SerializeField] Camera playerHead;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

        //Spawn random location of player

        int randIndexSpawn = Random.Range(0, locations.Length);
        int randIndexSearch = Random.Range(0, searchObjects.Length);
        float rotationAngleY = locations[randIndexSpawn].transform.rotation.eulerAngles.y -
            playerHead.transform.rotation.eulerAngles.y;
        player.transform.Rotate(0, rotationAngleY, 0);

        Vector3 distanceDiff = locations[randIndexSpawn].transform.position -
            playerHead.transform.position;
        Vector3 itemStart = player.transform.position + distanceDiff;

        //Make all locations invisible

        for (int i = 0; i < locations.Length; i++)
        {
            locations[i].SetActive(false);
        }

        //Spawn 1 random object.

        float radius = Random.Range(-3, 3);
        Vector3 originPoint = itemStart;
        originPoint.x += Random.Range(-radius, radius);
        originPoint.z += Random.Range(-radius, radius);
        Instantiate(searchObjects[randIndexSearch], originPoint, transform.rotation);
        player.transform.position = itemStart;
    }



    // Update is called once per frame
}
