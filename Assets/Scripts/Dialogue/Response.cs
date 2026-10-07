using UnityEngine;

[System.Serializable]
public class Response
{
    [SerializeField] private string reponseTxt;
    [SerializeField] private DialogueObject dialogueObject;

    public string ReponseText => reponseTxt;

    public DialogueObject DialogueObject => dialogueObject;
}
