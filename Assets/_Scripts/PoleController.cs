using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleController : MonoBehaviour
{
    public GameObject polePrefab;

    Transform followTarget;

    float offset;

    #region Spawn Poles Variables
    public GameObject bottomOfPole;
    [HideInInspector] public float distanceFromBottom;
    private float nextSpawnPos;
    private GameObject lastCreatedPole;
    #endregion

    private void Awake()
    {
        followTarget = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position.y - followTarget.position.y;

        lastCreatedPole = gameObject;
        distanceFromBottom = Mathf.Abs(transform.position.y - bottomOfPole.transform.position.y);
        nextSpawnPos = transform.position.y + distanceFromBottom;
    }

    
    void Update()
    {
        transform.position = new Vector3(transform.position.x, (followTarget.position.y + offset), transform.position.z);
        SpawnPoles();
    }

    private void SpawnPoles()
    {
        if (transform.position.y > nextSpawnPos)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, lastCreatedPole.transform.position.y - (distanceFromBottom * 2), transform.position.z);
            lastCreatedPole = Instantiate(polePrefab, spawnPos, Quaternion.identity, transform);
            nextSpawnPos = transform.position.y + distanceFromBottom * 2;
        }
    }
}
