using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIViewController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] TextMeshProUGUI BulletText;
    [SerializeField] List<TextMeshProUGUI> KillTexts;
    [SerializeField] TextMeshProUGUI KillMenuText;
    [SerializeField] TextMeshProUGUI TimerText;

    private GameDataModel gameDataModel;
    private void Start()
    {
        gameDataModel = GameManager.Instance.GameDataModel;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameDataModel != null)
        {
            if (HealthText != null)
            {
                HealthText.text = gameDataModel.HP.ToString();
            }

            if (BulletText != null)
            {
                BulletText.text = gameDataModel.BulletCount.ToString();
            }

            if (KillTexts.Count > 0)
            {
                foreach (var item in KillTexts)
                {
                    item.text = gameDataModel.KillCount.ToString();
                }
            }

            if (TimerText != null)
            {
                TimerText.text = gameDataModel.TimerCount.ToString("0.00");
            }
        }
    }
}
