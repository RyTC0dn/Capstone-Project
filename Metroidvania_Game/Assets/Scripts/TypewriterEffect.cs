using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    //private TMP_Text textBox;

    //[SerializeField] private string text;

    private int currentVisibleCharacterIndex;
    private Coroutine typewriterCoroutine;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay;

    [Header("Typewriter Settings")]
    [SerializeField] private float characterPerSecond = 20f;

    [SerializeField] private float interpunctuationDelay = 0.5f;

    public bool IsTyping;

    public bool CurrentlySkipping { get; private set; }
    private WaitForSeconds _skipDelay;

    [Header("Skip options")]
    [SerializeField] private bool quickSkip;

    [SerializeField][Min(1)] private int skipSpeedup = 5;

    private void Awake()
    {
        //textBox = GetComponent<TMP_Text>();

        _simpleDelay = new WaitForSeconds(1 / characterPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);

        _skipDelay = new WaitForSeconds(1 / (characterPerSecond * skipSpeedup));
    }

    public void SetText(string text, TextMeshProUGUI textBox)
    {
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        textBox.text = text;
        textBox.maxVisibleCharacters = 0;
        currentVisibleCharacterIndex = 0;

        typewriterCoroutine = StartCoroutine(routine: Typewriter(textBox));
    }

    private IEnumerator Typewriter(TextMeshProUGUI textBox)
    {
        TMP_TextInfo textInfo = textBox.textInfo;

        while (currentVisibleCharacterIndex < textInfo.characterCount + 1)
        {
            char character = textInfo.characterInfo[currentVisibleCharacterIndex].character;

            textBox.maxVisibleCharacters++;

            if ((character == '?' || character == '.' || character == ',' || character == ':' ||
                character == ';' || character == '!' || character == '-'))
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return _simpleDelay;
            }

            currentVisibleCharacterIndex++;
        }
    }
}