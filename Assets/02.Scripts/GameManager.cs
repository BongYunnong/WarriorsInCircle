using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public static class GameData
{
    public static Color hairColor;
    public static int[] customIndex = new int[11];
    public static Color[] bulletColors = new Color[2];
    public static int weaponIndex;
    public static int skillIndex;
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        if (instance==null)
        {
            instance = FindObjectOfType<GameManager>();
        }
        return instance;
    }
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gameClearPanel;
    [SerializeField] Text gameTimeTxt;

    [SerializeField] Slider[] healthSliders;
    [SerializeField] Slider[] staminaSliders;
    [SerializeField] Image SkillCoolTimeImg;
    [SerializeField] Image SkillImg;
    [SerializeField] Sprite[] skillSprites;
    Player player;
    Enemy enemy;

    [SerializeField] float GameTime;

    [SerializeField] AudioSource backgroundAudioSource;
    [SerializeField] AudioSource ExecutionAudioSource;
    public bool GameEnded { get; private set; }

    float currConcentrate = 0f;
    [SerializeField] float maxConcentrate = 5f;

    [SerializeField] float camShakeMultiplier = 1f;
    [SerializeField] float maxCamShakeAmplitude = 20;
    private float camShakeTime;
    private float camShakeAmplitude;

    [SerializeField] private CinemachineVirtualCamera VC;

    [SerializeField]private Text LogTxt;

    // Balance
    public Vector2 enemyHealthMixMax;
    public Vector2 enemyDamageMinMax;
    public Vector2 enemySpeedMinMax;
    public Vector2 enemyStaminaMinMax;
    public Vector2 enemyAttackRateMinMax;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        Invoke("UpdateCharacterHealth",0.5f);

        GameEnded = false;
        
        SkillImg.sprite = skillSprites[GameData.skillIndex];
    }
    private void Update()
    {
        if (GameEnded == false)
        {
            GameTime -= Time.deltaTime;
            gameTimeTxt.text = ((int)(GameTime / 60f) % 60f).ToString("00") + ":" + ((int)(GameTime % 60f)).ToString("00");
            UpdateCharacterStamina();

            if (GameTime <= 0)
            {
                GameEnded = true;
                bool cleared = enemy.health <= player.health;
                gameOverPanel.SetActive(!cleared);
                gameClearPanel.SetActive(cleared);
            }
            else if (enemy.health <= 0)
            {
                GameEnded = true;
                gameOverPanel.SetActive(false);
                gameClearPanel.SetActive(true);
            }
            else if (player.health <=0)
            {
                GameEnded = true;
                gameOverPanel.SetActive(true);
                gameClearPanel.SetActive(false);
            }

            if(enemy && player && ((enemy.health/enemy.maxHealth)<0.2f || (player.health / player.maxHealth) < 0.2f))
            {
                backgroundAudioSource.volume = Mathf.Lerp(backgroundAudioSource.volume, 0, Time.deltaTime);
                ExecutionAudioSource.volume = Mathf.Lerp(ExecutionAudioSource.volume, 0.6f, Time.deltaTime);
            }
        }

        if (VC.m_Follow)
        {
            if (camShakeTime > 0)
            {
                camShakeTime -= Time.deltaTime;
                VC.m_Follow.localPosition = Vector3.Lerp(VC.m_Follow.localPosition, Vector3.zero, Time.deltaTime * 2f);
                camShakeAmplitude = Mathf.Lerp(camShakeAmplitude, 0, Time.deltaTime);
            }
            else
            {
                VC.m_Follow.localPosition = Vector3.zero;
                camShakeAmplitude = 0;
            }

            if (currConcentrate>0)
            {
                currConcentrate -= Time.deltaTime;
                VC.m_Lens.OrthographicSize = Mathf.Lerp(VC.m_Lens.OrthographicSize, maxConcentrate, Time.deltaTime*0.5f);
            }
            else
            {
                VC.m_Lens.OrthographicSize = Mathf.Lerp(VC.m_Lens.OrthographicSize, 10f, Time.deltaTime * 3f);
            }
        }
    }
    public void UpdateCharacterHealth()
    {
        if (player)
            healthSliders[0].value = (float)player.health / player.maxHealth;
        if (enemy)
            healthSliders[1].value = (float)enemy.health / enemy.maxHealth;
    }
    public void UpdateCharacterStamina()
    {
        if (player)
            staminaSliders[0].value = (float)player.stamina / player.maxStamina;
        if (enemy)
            staminaSliders[1].value = (float)enemy.stamina / enemy.maxStamina;
    }
    public void UpdateSkillCoolTime(float _ratio)
    {
        if (player)
            SkillCoolTimeImg.fillAmount = _ratio;
    }

    public void TriggerOverHeatedAnim(int _teamIndex)
    {
        staminaSliders[_teamIndex].GetComponent<Animator>().SetTrigger("Overheated");

        if (_teamIndex == 0)
        {
            LogTxt.text = "지친 상태입니다.";
            LogTxt.GetComponent<Animator>().SetTrigger("Appear");
        }
    }
    public void TriggerGotStaminaAnim(int _teamIndex,bool _active)
    {
        staminaSliders[_teamIndex].GetComponent<Animator>().SetBool("GotStamina",_active);
    }

    public void CameraShake(float _force,float _time) {
        camShakeTime = _time;
        camShakeAmplitude += _force;
        camShakeAmplitude = Mathf.Clamp(camShakeAmplitude, 0, maxCamShakeAmplitude);
        if(VC.m_Follow)
            VC.m_Follow.localPosition = new Vector3(Random.Range(-1f, 1f) * camShakeAmplitude * camShakeMultiplier, Random.Range(-1f, 1f) * camShakeAmplitude* camShakeMultiplier, VC.m_Follow.position.z);
    }

    public void CameraConcentrate(float _time)
    {
        currConcentrate += _time;
    }


    public static Vector3 CalculateMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 spawnPos = Vector3.zero;

        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(mousePos);
        spawnPos = new Vector3(screenPoint.x, screenPoint.y, 0f);
        return spawnPos;
    }
}
