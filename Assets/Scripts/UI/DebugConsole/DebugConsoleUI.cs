using DebugConsole.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.DebugConsole
{
    public class DebugConsoleUI : MonoBehaviour
    {
        private const int CHARACTER_LIMIT = 13000;
        
        [Header("UI References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text consoleBody;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button openCloseButton;
        [SerializeField] private Button submit;
        [SerializeField] private ConsoleWrapper consoleWrapper; 
        private InputSystem_Actions inputActions;

        private void Awake()
        {
            inputActions = new InputSystem_Actions();
            
            inputActions.DebugConsole.ToggleConsole.performed += _ =>
            {
                panel.SetActive(!panel.activeSelf);
                if (panel.activeSelf) inputField.ActivateInputField();
            };
            
            inputActions.DebugConsole.SubmitConsole.performed += _ =>
            {
                HandleSubmitClick();
            };

            if (openCloseButton != null)
                openCloseButton.onClick.AddListener(ToggleConsole);
            
            if (panel != null) panel.SetActive(false);
        }

        private void OnEnable()
        {
            inputActions.Enable();
            
            submit?.onClick.AddListener(HandleSubmitClick);
            inputField?.onSubmit.AddListener(SubmitInput);
            if (consoleWrapper != null)
                consoleWrapper.Log += WriteToOutput;
        }

        private void OnDisable()
        {
            inputActions.Disable();

            submit?.onClick.RemoveListener(HandleSubmitClick);
            inputField?.onSubmit.RemoveListener(SubmitInput);
            if (consoleWrapper != null)
                consoleWrapper.Log -= WriteToOutput;
        }
        private void HandleSubmitClick() => SubmitInput(inputField.text);

        private void SubmitInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return;
            if (consoleWrapper == null)
            {
                Debug.LogError($"{nameof(consoleWrapper)} is null!");
                return;
            }

            consoleWrapper.TryUseInput(input);
            inputField.SetTextWithoutNotify(string.Empty);
        }
        public void ToggleConsole()
        {
            panel.SetActive(!panel.activeSelf);
            if (panel.activeSelf)
                inputField.ActivateInputField();
        }

        private void WriteToOutput(string newFeedback)
        {
            if (consoleBody == null)
            {
                Debug.LogError($"{nameof(consoleBody)} is null!");
                return;
            }

            consoleBody.text += "\n" + newFeedback;
            var watchdog = 10;
            var bodyText = consoleBody.text;
            while (watchdog-- > 0 && bodyText.Length >= CHARACTER_LIMIT)
            {
                var newBody = bodyText[(bodyText.IndexOf('\n') + 1)..];
                consoleBody.text = newBody;
                bodyText = newBody;
            }

            inputField.ActivateInputField();
        }
    }
}