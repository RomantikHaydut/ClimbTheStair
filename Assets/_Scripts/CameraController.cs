using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    private float offset;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        offset =  transform.position.y - player.transform.position.y;

    }
    void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.position = new Vector3(transform.position.x , player.transform.position.y + offset,transform.position.z);
    }
}
