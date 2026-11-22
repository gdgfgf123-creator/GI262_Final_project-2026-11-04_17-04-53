using System.Collections.Generic;
using UnityEngine;

public class BossChoiceDialogue : Dialogue
{
    [SerializeField] private List<BossChoice> choices = new List<BossChoice>();
    [SerializeField] private int triggerHPThreshold;

    public override void ShowDialogue()
    {
        base.ShowDialogue();
    }

    public override void CloseDialogue()
    {
        base.CloseDialogue();
    }

    public void StartDialogue()
    {
        ShowDialogue();
    }

    public void PauseBossCombat()
    {
        Time.timeScale = 0f;
    }

    public void ShowChoices()
    {
        // Connect the choice buttons here when the boss choice UI is added.
    }

    public void ResolveChoice()
    {
        // Resolve the selected BossChoice here.
    }

    public void ShowVictoryDialogue()
    {
        ShowDialogue();
    }
}
