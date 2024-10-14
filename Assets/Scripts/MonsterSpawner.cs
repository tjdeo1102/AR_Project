using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private List<GameObject> spawnMonsterPrefabs;
    private float cycleTime;
    private Coroutine spawnCoroutine;

    // object pool
    private GameObjectPool pool;

    private void Awake()
    {
        if(TryGetComponent<GameObjectPool>(out pool) == false)
        {
            pool = gameObject.AddComponent<GameObjectPool>();
        }
    }
    public void SetMonsters(List<GameObject> monsters, int maxPoolCount)
    {
        spawnMonsterPrefabs = monsters;
        pool.Initialize(spawnMonsterPrefabs, maxPoolCount);
    }

    public void SpawnStart(float CycleTime, bool isRandomSpawnCycle, float minSpawnTime = 0.5f)
    {
        if (pool.canSpawnObject)
        {
            cycleTime = CycleTime;
            spawnCoroutine = StartCoroutine(SpawnRoutine(isRandomSpawnCycle,minSpawnTime));
        }
    }

    public void SpawnStop()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }
    IEnumerator SpawnRoutine(bool isRandomSpawnCycle, float minSpawnTime)
    {
        var delay = new WaitForSeconds(cycleTime);
        while (true)
        {
            //랜덤 사이클인 경우, minSpawnTime ~ cycletime의 사이값울 랜덤으로 선택하여 delay걸기
            if (isRandomSpawnCycle) delay = new WaitForSeconds(Random.Range(minSpawnTime, cycleTime));

            yield return delay;

            //풀 리스트들 중 랜덤 몬스터 생성
            var monsterIndex = Random.Range(0, spawnMonsterPrefabs.Count);
            var monster = pool.GetObject(monsterIndex);
            var spawnPos = transform.position;
            // 스폰 위치의 y값은 플레이어의 y와 동일하게 설정
            spawnPos.y = GameManager.Instance.Player.transform.position.y;
            monster.transform.position = spawnPos;
            // 반환될 풀 지정
            if (monster.TryGetComponent<MonsterController>(out var controller))
            {
                controller.poolIndex = monsterIndex;
                controller.returnPool = pool;
            }
        }
    }
}
