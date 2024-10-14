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
            //���� ����Ŭ�� ���, minSpawnTime ~ cycletime�� ���̰��� �������� �����Ͽ� delay�ɱ�
            if (isRandomSpawnCycle) delay = new WaitForSeconds(Random.Range(minSpawnTime, cycleTime));

            yield return delay;

            //Ǯ ����Ʈ�� �� ���� ���� ����
            var monsterIndex = Random.Range(0, spawnMonsterPrefabs.Count);
            var monster = pool.GetObject(monsterIndex);
            var spawnPos = transform.position;
            // ���� ��ġ�� y���� �÷��̾��� y�� �����ϰ� ����
            spawnPos.y = GameManager.Instance.Player.transform.position.y;
            monster.transform.position = spawnPos;
            // ��ȯ�� Ǯ ����
            if (monster.TryGetComponent<MonsterController>(out var controller))
            {
                controller.poolIndex = monsterIndex;
                controller.returnPool = pool;
            }
        }
    }
}
