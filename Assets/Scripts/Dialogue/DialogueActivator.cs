using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, Interractable
{
    [SerializeField] private DialogueObject lines;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.lines = dialogueObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && other.TryGetComponent(out Player player))
        {
            player.Interractable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && other.TryGetComponent(out Player player))
        {
            if(player.Interractable is DialogueActivator activator && activator == this)
            {
                player.Interractable = null;
            }
        }
    }

    public void Interact(Player player)
    {
        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents == lines)
            {
                DialogueManager.instance.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        DialogueManager.instance.ShowDialogue(lines);
    }
}
