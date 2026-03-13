using UnityEngine;

public class ErrorLogger : MonoBehaviour
{
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error)
        {
            Debug.Log($"=== ПЕРЕХВАЧЕНА ОШИБКА ===\n{logString}\n{stackTrace}");
        }
    }
}