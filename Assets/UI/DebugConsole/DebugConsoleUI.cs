using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DebugConsole.Core;

namespace UI.DebugConsole
{
    public class DebugConsoleUI : MonoBehaviour
    {
        private const int CHARACTER_LIMIT = 13000;
        
        [Header("UI References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button openCloseButton;
        [SerializeField] private TMP_Text consoleBody;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button submit;
        [SerializeField] private ConsoleWrapper consoleWrapper; 
        
        private void Awake()
        {
            if (panel != null) panel.SetActive(false);

            if (openCloseButton != null)
                openCloseButton.onClick.AddListener(() =>
                    panel.SetActive(!panel.activeSelf));
        }

        private void OnEnable()
        {
            submit?.onClick.AddListener(HandleSubmitClick);
            inputField?.onSubmit.AddListener(SubmitInput);
            if (consoleWrapper != null)
                consoleWrapper.Log += WriteToOutput;
        }

        private void OnDisable()
        {
            submit?.onClick.RemoveListener(HandleSubmitClick);
            inputField?.onSubmit.RemoveListener(SubmitInput);
            if (consoleWrapper != null)
                consoleWrapper.Log -= WriteToOutput;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                panel.SetActive(!panel.activeSelf);
            }
        }

        private void HandleSubmitClick() => SubmitInput(inputField.text);

        private void SubmitInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;

            if (consoleWrapper == null)
            {
                Debug.LogError($"{nameof(consoleWrapper)} is null!");
                return;
            }

            consoleWrapper.TryUseInput(input);
            inputField.SetTextWithoutNotify(string.Empty);
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