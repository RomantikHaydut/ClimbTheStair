using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator anim;

    private UIManager uiManager;

    private PowerupController powerupController;

    private int level;

    public float goalScore;

    private float score;

    private float scoreFactor = 10f;

    #region Rest and Tired
    [Header("Rest and Tired")]
    public float percentOfStartTiring; // Material starts chancing at this percent.
    public float maxStamina;
    public float minStamina;
    public float stamina;
    public float restSpeed;
    [SerializeField] Transform head; // For sweat.
    [SerializeField] GameObject sweat;
    [SerializeField] float sweatRepeatTime;
    #endregion

    #region Movement Variables
    [Header("Movement")]
    public GameObject referance; // Player's referance.

    private Vector3 centerPoint = Vector3.zero; // Point which is we must turn around.

    [SerializeField] [Range(0.2f, 0.8f)] float movementBlend; // Clamp blend vlaue.

    [SerializeField] float verticalSpeed = 1f;

    [SerializeField] float horizontalSpeed = 1f;

    private float radius; // Distance from center.

    public float timeForOneRound = 6f; // default value is 6 seconds for 1 round.

    private const float animationSpeedMultiplier = 1.0f;

    [SerializeField] float stopDelayTime = 0.3f;

    private bool isStopping;

    private bool isRespwaning;

    #endregion

    #region Materials And Colors
    [Header("Colors and Materials")]
    private SkinnedMeshRenderer renderer;
    private Material material;
    [SerializeField] Color startColor;
    [SerializeField] Color tiredColor;
    #endregion

    #region Features
    [Header("Features")]
    public float income;
    public float staminaDecreaseMultiplier;
    public float speed;

    // Const start values
    private const float incomeStart = 0.3f;
    private const float staminaDecreaseMultiplierStart = 10;
    private const float speedStart = 1;
    #endregion


    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("LevelFinished", false);

        powerupController = GetComponent<PowerupController>();

        renderer = referance.GetComponent<SkinnedMeshRenderer>();
        material = renderer.material;
        renderer.material.color = startColor;

        level = SceneManager.GetActiveScene().buildIndex;

        uiManager = FindObjectOfType<UIManager>();

        radius = Vector3.Distance(new Vector3(referance.transform.position.x, 0, referance.transform.position.z), new Vector3(centerPoint.x, 0, centerPoint.z)); // Distance from center.

        stamina = maxStamina;

        goalScore = GameManager.Instance.GoalScore();

        isRespwaning = false;

        StartCoroutine(DissolveReverse());

    }

    private void Update()
    {
        // Starting level.
        if (!GameManager.Instance.isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.GameStarted();
            }
        }

        // Movement.
        else
        {
            if (!GameManager.Instance.isGameOver && !GameManager.Instance.isLevelFinished && !isRespwaning)
            {
                MovementBlend();
                SetFeatures();
                LevelWinCheck();
                Movement();
                GetScore();
            }
        }

    }

    private void SetFeatures() 
    {
        income = incomeStart + ((float)powerupController.GetIncomeLevel() / 2);
        staminaDecreaseMultiplier = (staminaDecreaseMultiplierStart * level / powerupController.GetStaminaLevel());
        speed = speedStart  + ((float)powerupController.GetSpeedLevel() / 5);
    }
    private void Movement()
    {

        if (Input.GetMouseButton(0))
        {
            if (!anim.GetBool("Moving"))
            {
                anim.SetBool("Moving", true);
            }
            else
            {
                Move();
                GetTired();
            }
            isStopping = false;
        }
        else
        {
            if (anim.GetBool("Moving"))
            {
                Move();

                if (!isStopping)
                {
                    StartCoroutine(StopDelay());
                    isStopping = true;
                }

            }

            GetRest();
        }

    }

    private void Move()
    {
        // Rotation
        transform.RotateAround(centerPoint, Vector3.up, (360 / timeForOneRound) * Time.deltaTime * AnimationSpeed() * horizontalSpeed * Speed());

        // Vertical movement.
        transform.position += Vector3.up * Time.deltaTime * verticalSpeed * Speed();
    }

    private float Speed()
    {
        anim.SetFloat("SpeedMultiplier", animationSpeedMultiplier * speed);
        return speed;
    }

    private float AnimationSpeed()
    {
        return anim.GetFloat("SpeedMultiplier");
    }

    private void MovementBlend()
    {
        anim.SetFloat("Blend" , movementBlend);
    } 

    private IEnumerator StopDelay() // For prevent short moves.
    {
        yield return new WaitForSecondsRealtime(stopDelayTime);
        if (!isStopping)
        {
            yield break;
        }
        anim.SetBool("Moving", false);
    }

    public float GetScore()
    {
        score = referance.transform.position.y;
        return score * scoreFactor;
    }

    private void LevelWinCheck()
    {
        // level win
        if (GetScore() >= goalScore)
        {
            anim.SetBool("LevelFinished", true);
            anim.ResetTrigger("Happy");
            anim.SetTrigger("Happy");
            GameManager.Instance.GameWin();
        }
    }

    // Tired and rest events...
    private void GetTired()
    {
        if (stamina > minStamina)
        {
            stamina -= (staminaDecreaseMultiplier * Time.deltaTime);

            StaminaForAnimation();

            if (stamina < (maxStamina * (percentOfStartTiring / 100f))) // Materials chancing to tired color here.
            {
                Sweat();
                float colorChangeTime = (1f / (stamina / staminaDecreaseMultiplier));
                Color activeColor = renderer.material.color;
                renderer.material.color = Color.Lerp(activeColor, tiredColor, colorChangeTime * Time.deltaTime);
            }
            
            
        }
        else
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                anim.SetBool("Death",true);
            }
            
        }
        
    }

    private void Sweat()
    {
        bool isThereSweat = GameObject.FindGameObjectsWithTag("Sweat").Length >= 1 ? true : false; // If there is a sweat in scene returns true.
        if (!isThereSweat)
        {
            sweatRepeatTime = Mathf.Clamp(sweatRepeatTime * (stamina / maxStamina), 0.3f, 3f);  // Sweats are repeat themselves faster if we are tired.

            GameObject sweatClone = Instantiate(sweat, head.position, transform.rotation);

            Destroy(sweatClone, sweatRepeatTime);
        }
    }

    private void GetRest()
    {
        if (stamina < maxStamina)
        {
            stamina += (restSpeed * Time.deltaTime);
            StaminaForAnimation();
            float colorChangeTime = (stamina / maxStamina);
            Color activeColor = renderer.material.color;
            renderer.material.color = Color.Lerp(activeColor, startColor, colorChangeTime * Time.deltaTime);
        }
        else
        {
            stamina = maxStamina;
        }
        
    }

    public void TakeWater()
    {
        StartCoroutine(MakeBlue());
        DestroyWater();     
        stamina = maxStamina;
        renderer.material.color = startColor;
        anim.SetFloat("Stamina", stamina / maxStamina);
        uiManager.CloseWaterOption();
    }
    private IEnumerator MakeBlue()// Taking water.
    {
        float makeBlueTimer = 0f;
        float makeWhiteTimer = 0f;
        while (true)
        {
            makeBlueTimer += Time.deltaTime;
            yield return null;
            if (makeBlueTimer <= 0.5f)
            {
                renderer.material.color = Color.Lerp(renderer.material.color, Color.blue, makeBlueTimer);
            }
            else
            {
                if (makeWhiteTimer <= 0.97f)
                {
                    makeWhiteTimer += Time.deltaTime;
                    renderer.material.color = Color.Lerp(renderer.material.color, startColor, makeWhiteTimer);
                }
                else
                {
                    renderer.material.color = startColor;
                    yield break;
                }

            }


        }
    } 

    public void DestroyWater()
    {
        GameObject[] waters = GameObject.FindGameObjectsWithTag("Water");
        for (int i = 0; i < waters.Length; i++) // Here we find which water we take.
        {
            if (waters[i].GetComponent<WaterController>().interacted)
            {
                Destroy(waters[i], 0.25f);
            }
        }
    }
    private void StaminaForAnimation() // 
    {
        anim.SetFloat("Stamina", stamina / maxStamina);
        float breathSpeed = Mathf.Clamp((maxStamina / stamina), 1f, 5f);
        anim.SetFloat("BreathMultiplier", breathSpeed);
    }

    // Game over events...
    private IEnumerator Dissolve()
    {
        material.SetFloat("_DissolveAmount", 0.2f);
        material.SetColor("_DissolveColor", renderer.material.color);
        SoundManager.Instance.PlayDeathSound();
        while (true)
        {
            if (material.GetFloat("_DissolveAmount") <= 0.7f)
            {
                yield return null;
                float amount = material.GetFloat("_DissolveAmount") + Time.deltaTime / 4;
                material.SetFloat("_DissolveAmount", amount);
            }
            else
            {
                GameManager.Instance.GameOver();
                yield break;
            }
        }
        
    }

    public IEnumerator DissolveReverse() // Start game and continiue game.
    {
        isRespwaning = true;
        stamina = maxStamina;
        renderer.material.color = startColor;
        anim.SetBool("Death", false);
        material.SetFloat("_DissolveAmount", 0.8f);
        material.SetColor("_DissolveColor", startColor);
        while (true)
        {
            if (material.GetFloat("_DissolveAmount") >= 0.1f)
            {
                yield return null;
                float amount = material.GetFloat("_DissolveAmount") - Time.deltaTime / 4;
                material.SetFloat("_DissolveAmount", amount);
            }
            else
            {
                isRespwaning = false;
                material.SetFloat("_DissolveAmount", 0);
                yield break;
            }
        }
    } 

  

    public void Death() // Calling in death animation.
    {
        SaveBestScore();
        anim.SetBool("Moving", false);
        StartCoroutine(Dissolve());
        GameManager.Instance.isGameOver = true;
        
    }

    private void SaveBestScore()
    {
        if (GetScore() > DataManager.Instance.GetBestScore())
        {
            DataManager.Instance.SetBestScore(GetScore());
            DataManager.Instance.SetPoleCount(GameObject.FindGameObjectsWithTag("Pole").Length);
        }
    }

    public void ContinueLevel()
    {
        StartCoroutine(DissolveReverse());
    }

    // Save in quit
    private void OnApplicationQuit()
    {
        SaveBestScore();
        DataManager.Instance.SetLastLevel(level);
    }
}
