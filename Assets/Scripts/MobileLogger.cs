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
        // ���ο� �α� �޽����� �߰��ϰ� UI �ؽ�Ʈ�� �ݿ�
        logMessages += log + "\n";
        textMeshPro.text = logMessages;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= OnReceiveLog;
    }
}
