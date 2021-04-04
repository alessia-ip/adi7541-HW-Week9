using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notes : MonoBehaviour
{
    public AudioClip audOne;
    public AudioClip audTwo;
    public AudioClip audThree;
    public AudioClip audFour;
    public AudioClip audFive;
    public AudioClip audSix;

    public GameObject btnOne;
    public GameObject btnTwo;
    public GameObject btnThree;
    public GameObject btnFour;
    public GameObject btnFive;
    public GameObject btnSix;

    private List<AudioClip> noteList = new List<AudioClip>();
    private Stack<AudioClip> noteBackup = new Stack<AudioClip>();
    private Queue<AudioClip> playerQueue = new Queue<AudioClip>();
    
    public AudioSource aud;
    
    public bool playerTurn = false;
    public TextMeshProUGUI instructText;

    public TextMeshProUGUI difficulty;

    public List<int> grabBag;
    
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Second);
        
        setGrabBag();
        
        noteList.Add(audOne);
        noteBackup.Push(noteList[0]);
        instructText.text = "Listen. Then, play it back.";
        Invoke("PlayNotes", 1f);
        difficulty.text = "Current difficulty: " + noteList.Count;
    }

    void setGrabBag()
    {
        grabBag.Add(1);
        grabBag.Add(2);        
        grabBag.Add(3);
        grabBag.Add(4);        
        grabBag.Add(5);
        grabBag.Add(6);
        grabBag.Add(1);
        grabBag.Add(2);        
        grabBag.Add(3);
        grabBag.Add(4);        
        grabBag.Add(5);
        grabBag.Add(6);
    }
    
    void PlayNotes()
    {

        settowhite();
        
        switch (noteBackup.Peek().name)
        {
            case "NoteOne":
                btnOne.GetComponent<Image>().color =  Color.green;
                break;
           case "NoteTwo":
                    btnTwo.GetComponent<Image>().color =  Color.green;
                    break;
           case "NoteThree":
                    btnThree.GetComponent<Image>().color =  Color.green;
                    break;
           case "NoteFour":
                    btnFour.GetComponent<Image>().color =  Color.green;
                    break;
           case "NoteFive":
                    btnFive.GetComponent<Image>().color =  Color.green;
                    break;
           case "NoteSix":
                    btnSix.GetComponent<Image>().color =  Color.green;
                    break;
           default:
                    break;
        }
        aud.PlayOneShot(noteBackup.Pop());
        if (noteBackup.Count > 0) 
        {
            Invoke("PlayNotes", 1f);
        }
        else
        {
            playerTurn = true;
            AddNotesToPlaylist();
            Invoke("settowhite", 1f);
        }
    }

    void settowhite()
    {
        btnOne.GetComponent<Image>().color =  Color.white;
        btnTwo.GetComponent<Image>().color =  Color.white;
        btnThree.GetComponent<Image>().color =  Color.white;
        btnFour.GetComponent<Image>().color =  Color.white;
        btnFive.GetComponent<Image>().color =  Color.white;
        btnSix.GetComponent<Image>().color =  Color.white;
    }
    
    void increaseDifficulty()
    {

        Debug.Log(grabBag.Count + " numbers left");
        
        if (grabBag.Count == 0)
        {
            setGrabBag();
        }

        var rand = Random.Range(0, grabBag.Count - 1);
        var newNote = grabBag[rand];
        
        Debug.Log(grabBag[rand] + " is the number we wanted at place " + rand);
        
        grabBag.RemoveAt(rand);
        
        Debug.Log(newNote + " is the new note");
        
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
        difficulty.text = "Current difficulty: " + noteList.Count;
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
        }
        else
        {
            instructText.text = "Try again";
          
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
