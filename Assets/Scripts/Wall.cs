using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private GameDataModel gameDataModel;

    private PlayerController playerController;
    void Start()
    {
        gameDataModel = GameManager.Instance.GameDataModel;
        playerController = GameManager.Instance.Player;
    }

    private void Update()
    {
        // 벽의 y축 값은 플레이어의 y축과 동일선상이 되도록 조정

        if (playerController != null) {
            var pos = transform.position;
            pos.y = playerController.transform.position.y;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            // 적인 경우에만, 자신의 피가 깍이면서 생명력을 잃는다 (벽이 점차 부숴짐)
            if (other.TryGetComponent<MonsterController>(out var controller))
            {
                if (gameDataModel != null)
                {
                    gameDataModel.HP--;
                }
                controller.RemoveMonster();
            }
        }
    }
}
