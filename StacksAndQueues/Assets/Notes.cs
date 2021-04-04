using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notes : MonoBehaviour
{
    public AudioClip audOne;
    public AudioClip audTwo;
    public AudioClip audThree;
    public AudioClip audFour;
    public AudioClip audFive;
    public AudioClip audSix;
    
    private List<AudioClip> noteList = new List<AudioClip>();
    private Stack<AudioClip> noteBackup = new Stack<AudioClip>();
    private Queue<AudioClip> playerQueue = new Queue<AudioClip>();
    
    public AudioSource aud;
    
    public bool playerTurn = false;
    public TextMeshProUGUI instructText;
    
    // Start is called before the first frame update
    void Start()
    {
        noteList.Add(audOne);
        noteBackup.Push(noteList[0]);
        instructText.text = "Listen. Then, play it back.";
        Invoke("PlayNotes", 1f);
    }
    
    void PlayNotes()
    {
        aud.PlayOneShot(noteBackup.Pop());
        if (noteBackup.Count > 0) 
        {
            Invoke("PlayNotes", 1f);
        }
        else
        {
            playerTurn = true;
            AddNotesToPlaylist();
        }
    }

    void increaseDifficulty()
    {
        var newNote = Random.Range(1, 6);
        Debug.Log(newNote);
        switch (newNote)
        {
            case 1:
                noteList.Add(audOne);
                break;
            case 2:
                noteList.Add(audTwo);
                break;
            case 3:
                noteList.Add(audThree);
                break;
            case 4:
                noteList.Add(audFour);
                break;
            case 5:
                noteList.Add(audFive);
                break;
            case 6:
                noteList.Add(audSix);
                break;
            default:
                noteList.Add(audOne);
                break;
        }
        AddNotesToPlaylist();
        Invoke("PlayNotes", 1f);
    }
    
    void AddNotesToPlaylist()
    {
        noteBackup.Clear();
        foreach (var audClip in noteList) { 
            noteBackup.Push(audClip);    
        }
    }
    
    
    void CompareStacks()
    {

        if (playerQueue.Count == 0)
        {
            instructText.text = "You did it! Here's something harder.";
            increaseDifficulty();
        } else if (playerQueue.Peek() == noteBackup.Peek())
        {
            playerQueue.Dequeue();
            noteBackup.Pop();
            CompareStacks();
            Debug.Log("Correct!");
        }
        else
        {
            instructText.text = "Try again";
            Debug.Log("Fail");
            Debug.Log(playerQueue.Peek().name+ " Player note");
            Debug.Log(noteBackup.Peek().name + " Real note");
            playerQueue.Clear();
            noteBackup.Clear();
            AddNotesToPlaylist();
            Invoke("PlayNotes", 0.5f);
        }
    }

    public void NoteButton(AudioClip audClip)
    {
        if (playerTurn == true)
        {
            aud.PlayOneShot(audClip);
            playerQueue.Enqueue(audClip);
            if (playerQueue.Count == noteList.Count)
            {
                playerTurn = false;
                Invoke("CompareStacks", 2f);
            }
        }
    }
}
