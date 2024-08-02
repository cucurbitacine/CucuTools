using System;
using TMPro;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts.UI
{
    [DisallowMultipleComponent]
    public class TextLogger : MonoBehaviour
    {
        [SerializeField] private TMP_Text logger;

        private void Log(string message)
        {
            if (logger)
            {
                logger.text = message;
            }
        }
        
        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Log)
            {
                Log(logString);
            }
            else
            {
                Log($"[{type}] {logString}");
            }
        }
        
        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
            
            Log("");
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }
    }
}
