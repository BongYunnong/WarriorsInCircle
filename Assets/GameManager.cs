using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] GameObject gameEndPanel;
    [SerializeField] Text gameTimeTxt;

    [SerializeField] Slider[] healthSliders;
    [SerializeField] Slider[] staminaSliders;
    Player player;
    Enemy enemy;

    [SerializeField] float GameTime;
    public bool GameEnded { get; private set; }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        Invoke("UpdateCharacterHealth",0.5f);

        GameEnded = false;
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
                gameEndPanel.SetActive(true);
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
}
