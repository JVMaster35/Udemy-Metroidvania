using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class TypeWriterEffect : MonoBehaviour
{
    [SerializeField] private float typeWriterSpeed = 25f;

    public bool isRunning { get; private set; }

    private readonly List<Puncatuation> punctuations = new List<Puncatuation>()
    {
        new Puncatuation(new HashSet<char>() {'.', '!', '?'}, 0.6f ),
        new Puncatuation(new HashSet<char>() {',', ';', ':'}, 0.3f )
    };

    private Coroutine typingCoroutine;

    public void Run(string txtToType, TMP_Text textLabel)
    {
        typingCoroutine = StartCoroutine(TypeText(txtToType, textLabel));
    }

    public void Stop()
    {
        StopCoroutine(typingCoroutine);
        isRunning = false;
    }

    private IEnumerator TypeText(string txtToType, TMP_Text textLabel)
    {
        isRunning = true;

        textLabel.text = string.Empty;

        float t = 0;
        int charIndex = 0;

        while (charIndex < txtToType.Length)
        {
            int lastCharIndex = charIndex;

            t += Time.deltaTime * typeWriterSpeed;

            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, txtToType.Length);

            for(int i = lastCharIndex; i < charIndex; i++)
            {
                bool isLast = i >= txtToType.Length - 1;

                textLabel.text = txtToType.Substring(0, i + 1);

                if (IsPunctuation(txtToType[i], out float waitTime) && !isLast && !IsPunctuation(txtToType[i + 1], out float timeWait))
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }

            yield return null;
        }

        isRunning = false;
    }

    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach(Puncatuation punctuationCategory in punctuations)
        {
            if (punctuationCategory.Puncuations.Contains(character))
            {
                waitTime = punctuationCategory.waitTime;
                return true;
            }
        }

        waitTime = default;
        return false;
    }

    private readonly struct Puncatuation
    {
        public readonly HashSet<char> Puncuations;
        public readonly float waitTime;

        public Puncatuation(HashSet<char> puncuation, float waitTime)
        {
            Puncuations = puncuation;
            this.waitTime = waitTime;
        }
    }

}
