using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private TypeWriterEffect typeWriter;
    private ResponseHandler responseHandler;

    public GameObject dialoguePanel;
    public TMP_Text dialogueBox;

    public bool IsOpen { get; private set; }


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        typeWriter = GetComponent<TypeWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        StartCoroutine(StepThroughDialogue(dialogueObject));
        dialoguePanel.SetActive(true);

    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            yield return RunTypingEffect(dialogue);

            dialogueBox.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.Responses != null && dialogueObject.Responses.Length > 0)
            {
                break;
            }

            yield return null;
            yield return new WaitUntil(() => Input.GetMouseButtonUp(1));
        }

        if (dialogueObject.HasReponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typeWriter.Run(dialogue, dialogueBox);

        while(typeWriter.isRunning)
        {
            yield return null;

            if(Input.GetMouseButtonDown(1))
            {
                typeWriter.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false;
        dialoguePanel.SetActive(false);
        dialogueBox.text = string.Empty;
    }
}
