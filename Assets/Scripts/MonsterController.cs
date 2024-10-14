using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] Vector3 target;
    [SerializeField] float speed;

    public GameObjectPool returnPool;
    public int poolIndex;

    private Rigidbody rb;
    private GameDataModel gameDataModel;
    void Start()
    {
        TryGetComponent<Rigidbody>(out rb);
        target = GameManager.Instance.EnemyTarget;
        gameDataModel = GameManager.Instance.GameDataModel;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            var targetPos = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            transform.LookAt(targetPos);
            rb.MovePosition(targetPos);
        }
    }

    public void RemoveMonster()
    {
        if (returnPool != null)
        {
            if (gameDataModel != null) gameDataModel.KillCount++;

            returnPool.ReturnObject(poolIndex, gameObject);
        }
    }
}
