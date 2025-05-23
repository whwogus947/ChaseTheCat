using System.Text.RegularExpressions;
using System.Threading;
using Com2usGameDev;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Com2usGameDev
{
    [System.Serializable]
    public class Dialogue
    {
        public DialogueHandler handler;
        public DialogueDataSO data;
        public UnityAction onLastDialogueEnd = delegate { };
        public NPCTypeSO NPC => data.NextNpc;

        private bool isFirstMessage = true;
        private bool isEventInvoked = false;

        public void StartNewMessage()
        {
            data.Reset();
            handler.Reset();
        }

        public DialogueText NextMessage(TMP_Text label, bool interruption = false)
        {
            if (isFirstMessage)
            {
                isFirstMessage = false;
                data.Reset();
            }

            if (handler.IsCompleted)
            {
                if (data.TryNext())
                {
                    handler.StartDialogue(label, data.CurrentMessage);
                    return data.CurrentDialogue;
                }
                if (!isEventInvoked)
                    onLastDialogueEnd?.Invoke();
            }
            else if (interruption)
            {
                handler.StopDialogue();
            }
            return data.CurrentDialogue;
        }
    }

    [System.Serializable]
    public class DialogueHandler
    {
        public bool IsCompleted => isCompleted;

        [SerializeField] private int letterDelay = 100;
        private CancellationTokenSource _cancellationTokenSource;
        private bool isCompleted = true;

        public void StartDialogue(TMP_Text print, string text, bool checkBefore = true)
        {
            if (checkBefore && !isCompleted)
                return;
            isCompleted = false;
            _cancellationTokenSource = new();
            DialogueRoutine(print, text, _cancellationTokenSource.Token).Forget();
        }

        public void StopDialogue()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource?.Cancel();
                isCompleted = true;
            }
        }

        public void Reset()
        {
            isCompleted = true;
        }

        public async UniTaskVoid DialogueRoutine(TMP_Text label, string text, CancellationToken cancellationToken)
        {
            bool isInsideTag = false;
            string printText = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    label.text = text;
                    return;
                }

                char currentChar = text[i];

                if (currentChar == '<')
                {
                    isInsideTag = true;
                    continue;
                }
                if (currentChar == '>')
                {
                    isInsideTag = false;
                    continue;
                }
                if (isInsideTag)
                {
                    continue;
                }

                await UniTask.Delay(letterDelay, delayType: DelayType.UnscaledDeltaTime);
                printText = text[0..i];
                label.text = printText;
            }
            label.text = text;
            isCompleted = true;
        }

        // public async UniTaskVoid DialogueRoutine(TMP_Text label, string text, CancellationToken cancellationToken)
        // {
        //     string openingColorTag = "";
        //     string pureText = text;

        //     Regex colorTagRegex = new Regex(@"<color=.*?>");
        //     Match colorMatch = colorTagRegex.Match(text);

        //     if (colorMatch.Success)
        //     {
        //         openingColorTag = colorMatch.Value;
        //         int closeTagIndex = text.LastIndexOf("</color>");

        //         pureText = text.Substring(openingColorTag.Length,
        //                                 closeTagIndex - openingColorTag.Length);
        //     }

        //     string printText = "";
        //     for (int i = 0; i < pureText.Length; i++)
        //     {
        //         if (cancellationToken.IsCancellationRequested)
        //         {
        //             label.text = text;
        //             return;
        //         }

        //         await UniTask.Delay(letterDelay, delayType: DelayType.UnscaledDeltaTime);
        //         printText += pureText[i];

        //         label.text = colorMatch.Success
        //             ? openingColorTag + printText + "</color>"
        //             : printText;
        //     }

        //     label.text = text;
        //     isCompleted = true;
        // }
    }

    [System.Serializable]
    public class DialogueText
    {
        public string subtitle;
        public NPCTypeSO npc;
        [TextArea(5, 10)]
        public string textMessage;
        public Sprite illust;
    }
}