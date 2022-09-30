using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_Story : MonoBehaviour
{
    public GameObject burrowPrefab;
    public TextMeshProUGUI timerText;
    public GameObject startScreen;

    private MoveBurrow burrow;
    private Story_PlayerController playerController;
    [SerializeField] float spawnRate = 1.0f;
    private float XRange = 12.5f;
    private float spawnZPos = 100;
    private int time = 0;

    public bool isTimeOver = false;
    public bool isStartGame = false;
    // Start is called before the first frame update
    IEnumerator Timer()
    {
        //60초 타이머
        for (int i = 0; i < 60; i++)
        {
            yield return new WaitForSeconds(1.0f);
            time++;
            timerText.text = "Time: " + (60 - time);
            if (playerController.isGameOver)
            {
                break;
            }
        }
        isTimeOver = true;
    }

    IEnumerator SpawnBurrow()
    {
        while (!playerController.isGameOver && !isTimeOver)
        {
            //시간에 따른 난이도 조정
            if (time == 20)
            {
                spawnRate = 0.25f;
                burrow.maxSpeed = 50;
            }
            if (time == 40)
            {
                spawnRate = 0.5f;
                burrow.maxSpeed = 40;
            }
            yield return new WaitForSeconds(spawnRate);
            Vector3 spawnPos = new Vector3(Random.Range(-XRange, XRange), 0, spawnZPos);
            Instantiate(burrowPrefab, spawnPos, burrowPrefab.transform.rotation); 
        }
    }

    public void StartGame()
    {
        burrow = burrowPrefab.GetComponent<MoveBurrow>();
        burrow.maxSpeed = 30;
        playerController = GameObject.Find("Player").GetComponent<Story_PlayerController>();
        playerController.isGameOver = false;
        isStartGame = true;        
        startScreen.SetActive(false);
        StartCoroutine(SpawnBurrow());
        timerText.text = "Time: " + 60;
        StartCoroutine(Timer());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
