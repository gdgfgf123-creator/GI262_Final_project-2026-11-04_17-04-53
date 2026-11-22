using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualNovelDialogue : Dialogue
{
    [Serializable]
    public class DialogueLine
    {
        public string characterName;

        [TextArea(2, 4)]
        public string text;
    }

    [Header("Dialogue UI - Text (Legacy)")]
    [SerializeField] private Text characterNameText;
    [SerializeField] private Text dialogueBodyText;
    [SerializeField] private Button nextButton;

    [Header("Dialogue Lines")]
    [SerializeField] private List<DialogueLine> lines = new List<DialogueLine>
    {
        new DialogueLine
        {
            characterName = "DATURA",
            text = "คืนนี้ เลือดของเจ้าจะเป็นของข้า"
        }
    };

    private int currentLineIndex;

    protected override void Awake()
    {
        base.Awake();

        if (nextButton != null)
            nextButton.onClick.AddListener(NextLine);
    }

    public override void ShowDialogue()
    {
        if (lines == null || lines.Count == 0)
        {
            Debug.LogWarning("VisualNovelDialogue has no dialogue lines.", this);
            return;
        }

        if (dialoguePanel == null ||
            characterNameText == null ||
            dialogueBodyText == null)
        {
            Debug.LogError(
                "Assign Dialogue Panel and both Text (Legacy) fields in the Inspector.",
                this
            );
            return;
        }

        base.ShowDialogue();

        if (!IsOpen)
            return;

        currentLineIndex = 0;
        DisplayCurrentLine();
    }

    public void PlayStory()
    {
        ShowDialogue();
    }

    public void NextLine()
    {
        if (!IsOpen)
            return;

        currentLineIndex++;

        if (currentLineIndex >= lines.Count)
        {
            CloseDialogue();
            return;
        }

        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        DialogueLine line = lines[currentLineIndex];

        characterNameText.text = line.characterName;
        dialogueBodyText.text = line.text;
    }

    protected override void OnDestroy()
    {
        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextLine);

        base.OnDestroy();
    }
}
