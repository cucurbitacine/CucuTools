using TMPro;
using UnityEngine;

namespace CucuTools.Terminal
{
    public class LogMessageView : MonoBehaviour
    {
        public LogMessage log = default;

        [Space]
        public Color logColor = new Color(0.75f, 0.75f, 0.75f);
        public Color warningColor = new Color(0.75f, 0.75f, 0.0f);
        public Color errorColor = new Color(0.75f, 0.0f, 0.0f);
        
        [Space]
        public TextMeshProUGUI gui = null;
        
        public void RefreshLogMessage()
        {
            gui.text = $"[{log.time:HH:mm:ss}] {log.message}";

            switch (log.type)
            {
                case LogType.Log:
                    gui.color = logColor;
                    break;
                case LogType.Warning:
                    gui.color = warningColor;
                    break;
                case LogType.Assert:
                case LogType.Error:
                case LogType.Exception:
                    gui.color = errorColor;
                    break;
            }
        }
        
        public void SetLogMessage(LogMessage log)
        {
            this.log = log;

            RefreshLogMessage();
        }
    }
}