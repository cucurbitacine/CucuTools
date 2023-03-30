using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CucuTools.Terminal
{
    [DisallowMultipleComponent]
    public sealed class CucuTerminal : CucuBehaviour
    {
        private static CucuTerminal _singleton = null;

        public static CucuTerminal Singleton
        {
            get
            {
                if (_singleton != null) return _singleton;
                _singleton = FindObjectOfType<CucuTerminal>();
                return _singleton;
            }
            private set => _singleton = value;
        }

        public KeyCode callTerminal = KeyCode.BackQuote;
        
        [Space]
        public bool showOnStart = true;
        
        [Range(1, 1024)]
        public int logCountMax = 128;
        
        [Space]
        public Canvas terminalCanvas = null;

        [Space]
        public TMP_InputField inputField = null;
        
        [Space]
        public Button submitButton = null;
        public Button clearButton = null;
        public Button closeButton = null;
        
        [Space]
        public Transform content = null;
        
        [Space]
        public LogMessageView logMessageViewPrefab = null;

        private int _selectedCommand = 0; 
        
        private readonly Queue<LogMessage> _logQueue = new Queue<LogMessage>();
        private readonly List<LogMessageView> _logHistory = new List<LogMessageView>();
        private readonly List<string> _commandHistory = new List<string>();
        private readonly Dictionary<string, TerminalCommand> _commands = new Dictionary<string, TerminalCommand>();

        public const char CommandPrefix = '/';
        public const char CommandSeparator = ' ';
        public const char CommandWordMarker = '"';

        #region Public API

        public void Submit(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            
            Debug.Log(text);
            
            if (text.StartsWith(CommandPrefix))
            {
                ExecuteAsCommand(text.TrimStart(CommandPrefix));
            }
        }

        public void ClearHistory()
        {
            for (var i = 0; i < _logHistory.Count; i++)
            {
                Destroy(_logHistory[i].gameObject);
            }
            
            _logHistory.Clear();
        }
        
        public void Show()
        {
            terminalCanvas.enabled = true;

            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }

        public void Hide()
        {
            terminalCanvas.enabled = false;
            
            inputField.text = string.Empty;
            inputField.DeactivateInputField();
        }
        
        public bool RegisterCommand(TerminalCommand command)
        {
            if (_commands.TryAdd(command.name, command))
            {
                Debug.Log($"Command \"/{command.name}\" successfully Registered");
                return true;
            }
            
            Debug.LogError($"Register command \"/{command.name}\" failed");
            return false;
        }
        
        public bool UnregisterCommand(string commandName)
        {
            if (_commands.Remove(commandName))
            {
                Debug.Log($"Command \"/{commandName}\" successfully Unregistered");
                return true;
            }
            
            Debug.LogError($"Unregister command \"/{commandName}\" failed");
            return false;
        }
        
        public bool UnregisterCommand(TerminalCommand command)
        {
            return UnregisterCommand(command.name);
        }

        #endregion

        #region Private API

        private void HandleLog(string message, string stackTrace, LogType logType)
        {
            var log = new LogMessage()
            {
                time = DateTime.Now,
                message = message,
                stackTrace = stackTrace,
                type = logType,
            };

            _logQueue.Enqueue(log);
        }

        private void AddLogTerminal(LogMessage logMessage)
        {
            LogMessageView logView;
            
            if (logCountMax <= _logHistory.Count)
            {
                logView = _logHistory[0];
                _logHistory.RemoveAt(0);
                logView.transform.SetSiblingIndex(_logHistory.Count);
            }
            else
            {
                logView = Instantiate(logMessageViewPrefab, content);
            }
            
            logView.SetLogMessage(logMessage);
            
            _logHistory.Add(logView);
        }

        private void GetCommandAndArgs(string text, out string commandName, out string[] commandArgs)
        {
            commandName = string.Empty;
            
            var parts = text.Split(CommandSeparator, StringSplitOptions.RemoveEmptyEntries);

            var listArgs = new List<string>();

            for (var i = 0; i < parts.Length; i++)
            {
                if (i == 0)
                {
                    commandName = parts[i];
                    continue;
                }

                if (parts[i].StartsWith(CommandWordMarker))
                {
                    var indexStop = parts.Length - 1;
                    
                    for (var j = i + 1; j < parts.Length; j++)
                    {
                        if (parts[j].EndsWith(CommandWordMarker))
                        {
                            indexStop = j;
                            break;
                        }
                    }

                    var word = parts.Where((p, ind) => i <= ind && ind <= indexStop);
                    var oneWord = string.Join(CommandSeparator, word).Trim(CommandWordMarker);
                    
                    listArgs.Add(oneWord);
                    i = indexStop;
                }
                else
                {
                    listArgs.Add(parts[i]);
                }
            }
            
            commandArgs = listArgs.ToArray();
        }
        
        private void ExecuteAsCommand(string text)
        {
            GetCommandAndArgs(text, out var cmdName, out var cmdArgs);

            if (_commands.TryGetValue(cmdName, out var cmd))
            {
                if (_commandHistory.Count == 0 || !string.Equals(_commandHistory[_commandHistory.Count - 1], text))
                {
                    _commandHistory.Add(text);
                }

                _selectedCommand = _commandHistory.Count;

                cmd.Execute(cmdArgs);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(cmdName))
                {
                    Debug.LogError($"Empty command");
                }
                else
                {
                    Debug.LogError($"Command \"{cmdName}\" was not found");
                }
            }
        }

        private void SelectPreviousCommand()
        {
            if (_commandHistory.Count == 0) return;
            
            _selectedCommand--;

            if (_selectedCommand < 0) _selectedCommand = 0;
                        
            if (0 <= _selectedCommand && _selectedCommand < _commandHistory.Count)
            {
                inputField.text = $"{CommandPrefix}{_commandHistory[_selectedCommand]}";

                inputField.caretPosition = inputField.text.Length;
            }
        }

        private void SelectNextCommand()
        {
            if (_commandHistory.Count == 0) return;
            
            if (_selectedCommand < _commandHistory.Count) _selectedCommand++;

            if (0 <= _selectedCommand && _selectedCommand < _commandHistory.Count)
            {
                inputField.text = $"{CommandPrefix}{_commandHistory[_selectedCommand]}";
                inputField.caretPosition = inputField.text.Length;
            }
            else
            {
                inputField.text = string.Empty;
            }
        }
        
        private void UpdateInputField()
        {
            if (!terminalCanvas.enabled || !inputField.isFocused) return;
            
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SelectPreviousCommand();
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SelectNextCommand();
            }
        }

        private void UpdateInput()
        {
            if (!inputField.isFocused && Input.GetKeyDown(callTerminal))
            {
                if (terminalCanvas.enabled)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }
            
            UpdateInputField();
        }
        
        private void UpdateLogViews()
        {
            if (!terminalCanvas.enabled) return;
            
            while (_logQueue.TryDequeue(out var logMessage))
            {
                AddLogTerminal(logMessage);
            }
        }
        
        private void CheckEventSystem()
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            
            if (eventSystem == null)
            {
                Debug.LogWarning("Event System was not found");
                eventSystem = new GameObject(nameof(EventSystem)).AddComponent<EventSystem>();
                Debug.Log("Event System - Created");
            }
            
            var inputModule = eventSystem.GetComponent<StandaloneInputModule>();

            if (inputModule == null)
            {
                Debug.LogWarning("Standalone Input Module was not found");
                inputModule = eventSystem.gameObject.AddComponent<StandaloneInputModule>();
                Debug.Log("Standalone Input Module - Created");
            }
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            CheckEventSystem();
        }
        
        #endregion

        #region UI Methods

        private void InputFieldSubmit(string text)
        {
            inputField.text = string.Empty;
            inputField.ActivateInputField();
            
            Submit(text);
        }
        
        private void SubmitButtonClick()
        {
            Submit(inputField.text);
        }

        private void ClearButtonClick()
        {
            ClearHistory();
        }
        
        private void CloseButtonClick()
        {
            Hide();
        }

        #endregion

        #region Static API

        public static bool TryGetBool(out bool boolValue, int index, params string[] args)
        {
            if (0 <= index && index < args.Length)
            {
                return bool.TryParse(args[index], out boolValue);
            }

            boolValue = false;
            return false;
        }
        
        public static bool TryGetInt(out int intValue, int index, params string[] args)
        {
            if (0 <= index && index < args.Length)
            {
                return int.TryParse(args[index], out intValue);
            }

            intValue = 0;
            return false;
        }
        
        public static bool TryGetFloat(out float floatValue, int index, params string[] args)
        {
            if (0 <= index && index < args.Length)
            {
                return float.TryParse(args[index], out floatValue);
            }

            floatValue = 0f;
            return false;
        }

        public static bool TryGetString(out string stringValue, int index, params string[] args)
        {
            if (0 <= index && index < args.Length)
            {
                stringValue = args[index];
                return true;
            }

            stringValue = string.Empty;
            return false;
        }
        
        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (Singleton == this)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
            SceneManager.sceneLoaded += SceneLoaded;
            
            inputField.onSubmit.AddListener(InputFieldSubmit);
            
            submitButton.onClick.AddListener(SubmitButtonClick);
            clearButton.onClick.AddListener(ClearButtonClick);
            closeButton.onClick.AddListener(CloseButtonClick);
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
            SceneManager.sceneLoaded -= SceneLoaded;
            
            inputField.onSubmit.RemoveListener(InputFieldSubmit);
            
            submitButton.onClick.RemoveListener(SubmitButtonClick);
            clearButton.onClick.RemoveListener(ClearButtonClick);
            closeButton.onClick.RemoveListener(CloseButtonClick);
        }

        private void Start()
        {
            if (showOnStart) Show();
            else Hide();

            var helpCommand = new ActionCommand("help", args =>
            {
                foreach (var command in _commands)
                {
                    Debug.Log($"{CommandPrefix}{command.Key}");
                }
            });

            var clearCommand = new ActionCommand("clear", args =>
            {
                ClearHistory();
            });
            
            RegisterCommand(helpCommand);
            RegisterCommand(clearCommand);
        }

        private void Update()
        {
            UpdateInput();

            UpdateLogViews();
        }

        #endregion
    }

    [Serializable]
    public class LogMessage
    {
        public DateTime time = default;
        public string message = string.Empty;
        [Multiline] public string stackTrace = string.Empty;
        public LogType type = LogType.Log;
    }
}