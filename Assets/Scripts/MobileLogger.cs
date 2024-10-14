using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobileLogger : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro;
    private string logMessages = "";
    private void OnEnable()
    {
        Application.logMessageReceived += OnReceiveLog;
    }

    void OnReceiveLog(string log, string stackTrace, LogType type)
    {
        // 새로운 로그 메시지를 추가하고 UI 텍스트에 반영
        logMessages += log + "\n";
        textMeshPro.text = logMessages;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= OnReceiveLog;
    }
}
