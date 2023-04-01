using System;
using System.Collections.Generic;
using System.Linq;
using CucuTools.Terminal.Commands;
using CucuTools.Terminal.View;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CucuTools.Terminal
{
    [DisallowMultipleComponent]
    public sealed class CucuTerminal : TerminalCommandRegistrator
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

        [Header("General Settings")]
        public KeyCode callTerminal = KeyCode.BackQuote;
        public bool showOnEnable = false;
        public bool stopTime = false;
        [Range(0.0001f, 1f)]
        public float stopTimeScale = 0.0001f;
        public bool debugLog = false;
        [Min(1)]
        public int logCountMax = 128;
        [Space]
        public Canvas terminalCanvas = null;
        
        [Header("Console Panel")]
        public GameObject consolePanel = null;
        public Transform contentAnchor = null;
        public LogMessageView logViewPrefab = null;
        
        [Header("Control Block")]
        public TMP_InputField inputField = null;
        public Button submitButton = null;
        public Button clearButton = null;
        public Button closeButton = null;

        [Header("Commands Block")]
        public GameObject commandsPanel = null;
        public TextMeshProUGUI commandsGUI = null;

        private int _selectedCommand = 0; 
        private string _lastInput = string.Empty;
        private float _lastTimeScale = 1f;
        
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
                ExecuteAsCommand(text);
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

            _lastTimeScale = Time.timeScale;
            if (stopTime) Time.timeScale = stopTimeScale;
        }

        public void Hide()
        {
            terminalCanvas.enabled = false;
            
            inputField.text = string.Empty;
            inputField.DeactivateInputField();
            
            Time.timeScale = _lastTimeScale;
        }

        public string[] GetPossibleCommands(string text)
        {
            GetCommandAndArgs(text, out var commandName, out _);
            
            return _commands.Keys.Where(k => k.StartsWith(commandName)).ToArray();
        }
        
        public bool RegisterCommand(TerminalCommand command)
        {
            if (_commands.TryAdd(command.name, command))
            {
                if (debugLog) Debug.Log($"Command \"/{command.name}\" successfully Registered");
                return true;
            }
            
            if (debugLog) Debug.LogError($"Register command \"/{command.name}\" failed");
            return false;
        }
        
        public bool UnregisterCommand(string commandName)
        {
            if (_commands.Remove(commandName))
            {
                if (debugLog) Debug.Log($"Command \"/{commandName}\" successfully Unregistered");
                return true;
            }
            
            if (debugLog) Debug.LogError($"Unregister command \"/{commandName}\" failed");
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
                logView = Instantiate(logViewPrefab, contentAnchor);
            }
            
            logView.SetLogMessage(logMessage);
            
            _logHistory.Add(logView);
        }

        private void GetCommandAndArgs(string text, out string commandName, out string[] commandArgs)
        {
            text = text.TrimStart(CommandPrefix);
            
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
                    if (debugLog) Debug.LogError($"Empty command");
                }
                else
                {
                    if (debugLog) Debug.LogError($"Command \"{cmdName}\" was not found");
                }
            }
        }

        private void SelectPreviousCommand()
        {
            if (_commandHistory.Count == 0) return;

            if (_selectedCommand == _commandHistory.Count) _lastInput = inputField.text;
            
            _selectedCommand--;

            if (_selectedCommand < 0) _selectedCommand = 0;
                        
            if (0 <= _selectedCommand && _selectedCommand < _commandHistory.Count)
            {
                inputField.text = _commandHistory[_selectedCommand];

                inputField.caretPosition = inputField.text.Length;
            }
        }

        private void SelectNextCommand()
        {
            if (_commandHistory.Count == 0) return;
            
            if (_selectedCommand < _commandHistory.Count) _selectedCommand++;

            if (0 <= _selectedCommand && _selectedCommand < _commandHistory.Count)
            {
                inputField.text = _commandHistory[_selectedCommand];
                inputField.caretPosition = inputField.text.Length;
            }
            else
            {
                if (_selectedCommand == _commandHistory.Count) inputField.text = _lastInput;
            }
        }

        private void ShowPossibleCommands(string text)
        {
            var possibleCommands = GetPossibleCommands(text);

            if (0 < possibleCommands.Length)
            {
                commandsGUI.text = string.Join("\n", possibleCommands.Select(c => $"/{c}"));
                
                consolePanel.SetActive(false);
                commandsPanel.SetActive(true);
            }
            else
            {
                commandsGUI.text = string.Empty;
            }
        }

        private void HidePossibleCommands()
        {
            consolePanel.SetActive(true);
            commandsPanel.SetActive(false);
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

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                var cmds = GetPossibleCommands(inputField.text.TrimStart(CommandPrefix));
                if (cmds.Length == 1)
                {
                    inputField.text = $"{CommandPrefix}{cmds[0]} ";
                    inputField.caretPosition = inputField.text.Length;
                }
                else if (cmds.Length > 1)
                {
                    var general = cmds[0];
                    var length = general.Length;

                    for (var i = length - 1; i >= 0; i--)
                    {
                        general = general.Substring(0, i + 1);
                        if (cmds.All(c => c.StartsWith(general)))
                        {
                            inputField.text = $"{CommandPrefix}{general}";
                            inputField.caretPosition = inputField.text.Length;
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateInput()
        {
            if (Input.GetKeyDown(callTerminal))
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
                if (debugLog) Debug.LogWarning("Event System was not found");
                eventSystem = new GameObject(nameof(EventSystem)).AddComponent<EventSystem>();
                if (debugLog) Debug.Log("Event System - Created");
            }
            
            var inputModule = eventSystem.GetComponent<StandaloneInputModule>();

            if (inputModule == null)
            {
                if (debugLog) Debug.LogWarning("Standalone Input Module was not found");
                inputModule = eventSystem.gameObject.AddComponent<StandaloneInputModule>();
                if (debugLog) Debug.Log("Standalone Input Module - Created");
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

        private void InputFieldChange(string text)
        {
            if (text.StartsWith(CommandPrefix))
            {
                ShowPossibleCommands(text);
            }
            else
            {
                HidePossibleCommands();
            }
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

        #region Commands

        [TerminalCommand("quit")]
        private void QuitCommand(int exitCode = 0)
        {
            Application.Quit(exitCode);
        }
        
        [TerminalCommand("terminal.clear")]
        private void ClearCommand()
        {
            ClearHistory();
        }

        [TerminalCommand("terminal.close")]
        private void CloseCommand()
        {
            Hide();
        }
        
        [TerminalCommand("terminal.debug")]
        private void DebugCommand(bool value)
        {
            debugLog = value;
            
            Debug.Log($"Debug Mode = {debugLog}");
        }

        [TerminalCommand("terminal.timestop.mode")]
        private void TimeStopCommand(bool value)
        {
            stopTime = value;
            
            Debug.Log($"Time Stop Mode = {stopTime}");
        }
        
        [TerminalCommand("terminal.timestop.scale")]
        private void TimeStopCommand(float value)
        {
            stopTimeScale = value;
            
            Debug.Log($"Time Stop = {stopTimeScale:f2}");
        }
        
        [TerminalCommand("scene.load.single")]
        private void LoadSingleCommand(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        
        [TerminalCommand("scene.load.additive")]
        private void LoadAdditiveCommand(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        [TerminalCommand("transform.translate")]
        private void MoveCommand(string objectName, float x, float y, float z, string spaceName)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                var move = new Vector3(x, y, z);
                var space = spaceName != null && spaceName.ToLower().Contains("self") ? Space.Self : Space.World;
                go.transform.Translate(move, space);
                
                Debug.Log($"\"{go.name}\" moved by {move} : {space}");
            }
        }
        
        [TerminalCommand("transform.rotate")]
        private void RotateCommand(string objectName, float x, float y, float z, string spaceName)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                var eulers = new Vector3(x, y, z);
                var space = spaceName != null && spaceName.ToLower().Contains("self") ? Space.Self : Space.World;
                go.transform.Rotate(eulers, space);
                
                Debug.Log($"\"{go.name}\" rotated by {eulers} : {space}");
            }
        }
        
        [TerminalCommand("transform.scale")]
        private void ScaleCommand(string objectName, float x, float y, float z)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                var scale = new Vector3(x, y, z);
                go.transform.localScale = Vector3.Scale(go.transform.localScale, scale);
                
                Debug.Log($"\"{go.name}\" scaled by {scale}");
            }
        }
        
        [TerminalCommand("set.position")]
        private void SetPositionCommand(string objectName, float x, float y, float z)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                var position = new Vector3(x, y, z);
                go.transform.position = position;
                
                Debug.Log($"\"{go.name}\" set position {position}");
            }
        }
        
        [TerminalCommand("set.rotation")]
        private void SetRotationCommand(string objectName, float x, float y, float z)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                var euler = new Vector3(x, y, z);
                go.transform.rotation = Quaternion.Euler(euler);
                
                Debug.Log($"\"{go.name}\" set rotation {euler}");
            }
        }
        
        [TerminalCommand("set.scale")]
        private void SetScaleCommand(string objectName, float x, float y, float z)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                var scale = new Vector3(x, y, z);
                go.transform.localScale = new Vector3(x, y, z);
                
                Debug.Log($"\"{go.name}\" set scale {scale}");
            }
        }
        
        [TerminalCommand("get.position")]
        private void GetPositionCommand(string objectName)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                Debug.Log(go.transform.position);
            }
        }
        
        [TerminalCommand("get.rotation")]
        private void GetRotationCommand(string objectName)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                Debug.Log(go.transform.rotation);
            }
        }
        
        [TerminalCommand("get.scale")]
        private void GetScaleCommand(string objectName)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                Debug.Log(go.transform.localScale);
            }
        }
        
        [TerminalCommand("log.warning")]
        private void LogWarningCommand(params string[] args)
        {
            Debug.LogWarning(string.Join(CommandSeparator, args));
        }
        
        [TerminalCommand("log.error")]
        private void LogErrorCommand(params string[] args)
        {
            Debug.LogError(string.Join(CommandSeparator, args));
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

        protected override void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
            SceneManager.sceneLoaded += SceneLoaded;
            
            inputField.onSubmit.AddListener(InputFieldSubmit);
            inputField.onValueChanged.AddListener(InputFieldChange);
            
            submitButton.onClick.AddListener(SubmitButtonClick);
            clearButton.onClick.AddListener(ClearButtonClick);
            closeButton.onClick.AddListener(CloseButtonClick);
            
            base.OnEnable();
            
            if (showOnEnable)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            Application.logMessageReceived -= HandleLog;
            SceneManager.sceneLoaded -= SceneLoaded;
            
            inputField.onSubmit.RemoveListener(InputFieldSubmit);
            inputField.onValueChanged.RemoveListener(InputFieldChange);
            
            submitButton.onClick.RemoveListener(SubmitButtonClick);
            clearButton.onClick.RemoveListener(ClearButtonClick);
            closeButton.onClick.RemoveListener(CloseButtonClick);
        }

        private void Update()
        {
            UpdateInput();

            UpdateLogViews();
        }

        #endregion
    }
}