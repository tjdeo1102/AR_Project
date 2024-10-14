using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    [Header("Base Settings")]
    public PlayerController Player;
    public GameDataModel GameDataModel;
    public StartPointTracking StartPointTracking;
    public ARSession Session;

    public bool isDebugMode;
    public static GameManager Instance;

    [SerializeField] List<MonsterSpawner> spawners;
    [SerializeField] List<GameObject> spawnMonsterPrefabs;
    [SerializeField] int maxPoolCount;

    [Header("Spawn Settings")]
    [SerializeField] float spawnDistance;
    [SerializeField] float spawnCycleTime;
    [SerializeField] bool isRandomCycle;
    [SerializeField] float minRandomTime;

    [Header("Game Settings")]
    [SerializeField] int hp;
    [SerializeField] int bullet;
    [SerializeField] float timer;
    public Vector3 EnemyTarget;
    public bool isPlaying;

    [Header("UI")]
    [SerializeField] GameObject RestartPanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (GameDataModel != null && isPlaying)
        {
            GameDataModel.TimerCount -= Time.deltaTime;

            if (GameDataModel.TimerCount <= 0 || GameDataModel.HP <= 0)
            {
                Pause();
                RestartPanel.SetActive(true);
            }
        }
    }
    public void PlayInit()
    {
        if (StartPointTracking != null)
        {
            for (int i = 0; i < spawners.Count; i++)
            {
                spawners[i].SetMonsters(spawnMonsterPrefabs, maxPoolCount);
            }

            if (isDebugMode)
            {
                Play(Player.transform.position);
            }
            else
            {
                StartPointTracking.isPlayInit = true;
            }
        }
    }
    public void Play(Vector3 startPos)
    {
        if (GameDataModel == null) return;
        GameDataModel.HP = hp;
        GameDataModel.BulletCount = bullet;
        GameDataModel.KillCount = 0;
        GameDataModel.TimerCount = timer;

        // 명시적으로 보기위해, 게임이 시작되어야 하는 요소들 (스포너, 플레이어 컨트롤러) 에 직접 Play 호풀 (이벤트로 구현 X)
        for (int i = 0; i < spawners.Count; i++)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            randomDirection.y = 0f;
            var dir = randomDirection.normalized;
            spawners[i].transform.position = startPos + dir * spawnDistance;
            spawners[i].SpawnStart(spawnCycleTime, isRandomCycle, minRandomTime);
        }
        var target = startPos;
        target.y = Player.transform.position.y;
        EnemyTarget = target;
        isPlaying = true;
        Continue();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Session.Reset();
        SceneManager.LoadScene(0);
    }
}
