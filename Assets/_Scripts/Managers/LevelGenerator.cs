using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject stairPrefab;

    [SerializeField] LayerMask stairLayer;

    public Transform spawnCenter;

    [SerializeField] Vector3 spawnArea;

    [SerializeField] Transform stairsParent;

    // Best score pole variables...
    [Header("Best Score Pole")]
    public GameObject polePrefab;
    public GameObject bestScorePos;
    public GameObject bestScoreSign;
    private int poleCount;

    //Game Over Variables.
    [SerializeField] float forceFactor;
    [SerializeField] float torqueFactor;


    private void Start()
    {
        CreateBestScorePole();
    }
    private void Update()
    {
        if (!GameManager.Instance.isGameOver && GameManager.Instance.isGameStarted)
        {
            CreateStairs();
        }

    }



    public void GameOver() // Using. force all stairs.
    {
        GameObject[] stairList = GameObject.FindGameObjectsWithTag("Stair"); // All stairs in scene.
        foreach (GameObject stair in stairList)
        {
            stair.transform.parent = null;
            Rigidbody rb = stair.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            Vector3 forceDir = (stair.transform.position - new Vector3(0, stair.transform.position.y, 0)).normalized;
            rb.AddForce(forceDir * forceFactor, ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-360f, 360f), Random.Range(-360f, 360f), Random.Range(-360f, 360f)) * torqueFactor, ForceMode.Impulse);
        }
    }

    public void CreateStairs()
    {
        bool canSpawn = !Physics.CheckBox(spawnCenter.position, spawnArea, spawnCenter.rotation, stairLayer);
        if (canSpawn)
        {
            GameObject stairClone = Instantiate(stairPrefab, spawnCenter.position, spawnCenter.rotation, stairsParent);
            stairClone.transform.Rotate(Vector3.up * 90);
            SoundManager.Instance.PlayStepSound();
        }
    }

    public void CreateBestScorePole()
    {
        poleCount = DataManager.Instance.GetPoleCount() - 1;
        if (poleCount > 0)
        {
            float distanceFromBottom = FindObjectOfType<PoleController>().distanceFromBottom;
            Vector3 spawnPos = new Vector3(bestScorePos.transform.position.x, bestScorePos.transform.position.y - distanceFromBottom, bestScorePos.transform.position.z);

            for (int i = 0; i < poleCount; i++)
            {
                Instantiate(polePrefab, spawnPos, bestScorePos.transform.rotation, bestScorePos.transform);

                bestScorePos.transform.position += new Vector3(0, distanceFromBottom * 2, 0);

                if (i == (poleCount - 1)) // We instantiate score sign with canvas after last pole.
                {
                    GameObject bestScoreObject = Instantiate(bestScoreSign, bestScorePos.transform.position, Quaternion.identity, bestScorePos.transform);
                    FindObjectOfType<UIManager>().DisplayBestScore(bestScoreObject);
                }

            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(spawnCenter.position, spawnArea * 2f);
    }
}
