using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    private Transform player;

    [SerializeField] float interactDistance;

    public bool interacted = false;

    private Vector3 turnVector;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        turnVector = transform.up + transform.forward / 2;
    }

    private void Update()
    {
        transform.Rotate(turnVector * 60 * Time.deltaTime, Space.Self);

        if (!interacted)
        {
            float distanceFromPlayer = Mathf.Abs(transform.position.y - player.transform.position.y);

            if (distanceFromPlayer < interactDistance)
            {
                // Interact here....
                StartCoroutine(GoBig());
                FindObjectOfType<UIManager>().OpenWaterOption();
                interacted = true;
            }
        }
    }

    private IEnumerator GoBig()
    {
        Vector3 bigSize = transform.localScale * 2;
        float offSet = transform.localScale.x / 20;
       
        while (true)
        {
            yield return null;
            transform.localScale = Vector3.Slerp(transform.localScale, bigSize, Time.deltaTime * 2);
            if (transform.localScale.x + offSet >= bigSize.x)
            {
                StartCoroutine(GoSmall());
                yield break;
            }
        }
    }

    private IEnumerator GoSmall()
    {
        Vector3 smallSize = transform.localScale / 2;
        float offSet = transform.localScale.x / 20;
        while (true)
        {
            yield return null;
            transform.localScale = Vector3.Slerp(transform.localScale, smallSize, Time.deltaTime  * 2);
            if (transform.localScale.x - offSet<= smallSize.x)
            {
                StartCoroutine(GoBig());
                yield break;
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
