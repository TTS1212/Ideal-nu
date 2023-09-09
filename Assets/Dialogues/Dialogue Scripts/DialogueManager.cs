using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")] 
    
    [SerializeField] 
    private GameObject dialoguePanel;

    [SerializeField]
    private TextMeshProUGUI _dialogueText, _nameText;

    [SerializeField] private GameObject ctc;

    private Story _story;

    [HideInInspector] public bool _dialogueIsPlaying;

    private static DialogueManager instance;

    [Header("Audio")] 
    [SerializeField] private AudioClip[] clips;
    private AudioClip dialogueTypingClip()
    {
        AudioClip result = null;
        
        switch (Random.Range(0, 4))
        {
            case 0:
                result = clips[0];
                break;
            
            case 1:
                result = clips[1];
                break;
            
            case 2:
                result = clips[2];
                break;
            
            case 3:
                result = clips[3];
                break;
        }

        return result;
    }

    [SerializeField] private bool stopAudioSource;

    private AudioSource _source;

    [Header("Choices UI")] 
    
    [SerializeField]
    private GameObject[] choices;

    private TextMeshProUGUI[] choicesText;

    [Header("Parameters")] 
    [SerializeField] private float _typingSpeed = 0.04f;
    private Coroutine displayLineCoroutine;
    private string textToDisplay;
    private bool _canContinueToNextLine = false;
    private bool _typing;
    private float _originalValue;
    
    public static event Action finishedDialogue;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Something Happened.\n There is more than one instance of Dialogue Manager (scripts) in this scene. Please delete one Manager so the game can continue smoothly.");
        }

        instance = this;

        _source = this.gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayerMovement._confirmed += HandleConfirm;
    }

    private void OnDisable()
    {
        PlayerMovement._confirmed -= HandleConfirm;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        _dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        _originalValue = _typingSpeed;
        
        // Gathering choices' texts.

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!_dialogueIsPlaying)
        {
            return;
        }
    }

    private void HandleConfirm()
    {
        if (_canContinueToNextLine 
            && _dialogueIsPlaying 
            && _story.currentChoices.Count == 0)
        {
            ContinueStory();
            
        }

        if (_typing && !_canContinueToNextLine)
        {
            _typingSpeed -= 0.02f;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        _story = new Story(inkJSON.text);
        _dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    public void ExitDialogueMode()
    {
        _dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        _dialogueText.text = "";
        finishedDialogue?.Invoke();
    }

    private void ContinueStory()
    {
        if (_story.canContinue)
        {
            
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            displayLineCoroutine = StartCoroutine(DisplayLine(_story.Continue()));
            
            HandleNames();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        if (line.Contains(":"))
        {
            string[] newline = line.Split(':');
            line = newline[1];
        }
        
        _dialogueText.text = "";

        _canContinueToNextLine = false;
        
        HideChoices();
        
        ctc.SetActive(false);
        
        _typing = true;

        foreach (char letter in line)
        {
            if (stopAudioSource)
            {
                _source.Stop();
            }

            if (letter != ' ')
            {
                _source.PlayOneShot(dialogueTypingClip());
            }

            _dialogueText.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }

        _typing = false;

        _canContinueToNextLine = true;
        
        DisplayChoices();

        ctc.SetActive(true);

        _typingSpeed = _originalValue;
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void HandleNames()
    {
        if (_story.currentText.Contains(":"))
        {
            string[] rawName = _story.currentText.Split(':');

            _nameText.text = rawName[0];
        }
        else
        {
            _nameText.text = "";
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _story.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("Something Happened.\n More choices were given than the manager can support.");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int ChoiceIndex)
    {
        if (_canContinueToNextLine)
        {
            _story.ChooseChoiceIndex(ChoiceIndex);
            ContinueStory();
        }
    }
}
