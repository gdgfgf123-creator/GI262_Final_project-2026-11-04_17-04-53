using UnityEngine;

public abstract class Dialogue : MonoBehaviour
{
    [Header("Shared Dialogue UI")]
    [SerializeField] protected GameObject dialoguePanel;

    private float previousTimeScale = 1f;
    protected bool IsOpen { get; private set; }

    protected virtual void Awake()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public virtual void ShowDialogue()
    {
        if (dialoguePanel == null)
        {
            Debug.LogError("Dialogue Panel is not assigned.", this);
            return;
        }

        if (IsOpen)
            return;

        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        IsOpen = true;
        dialoguePanel.SetActive(true);
    }

    public virtual void CloseDialogue()
    {
        if (!IsOpen)
            return;

        IsOpen = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        Time.timeScale = previousTimeScale;
    }

    protected virtual void OnDestroy()
    {
        if (IsOpen)
            Time.timeScale = previousTimeScale;
    }
}
