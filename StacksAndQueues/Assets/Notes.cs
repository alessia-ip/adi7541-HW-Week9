using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notes : MonoBehaviour
{
    //all the audio used in the game
    public AudioClip audOne;
    public AudioClip audTwo;
    public AudioClip audThree;
    public AudioClip audFour;
    public AudioClip audFive;
    public AudioClip audSix;

    //the note buttons
    public GameObject btnOne;
    public GameObject btnTwo;
    public GameObject btnThree;
    public GameObject btnFour;
    public GameObject btnFive;
    public GameObject btnSix;

    //the three data structures
    private List<AudioClip> noteList = new List<AudioClip>(); //this is the master list
    private Stack<AudioClip> noteBackup = new Stack<AudioClip>(); //this is the stack we play the music from
    private Queue<AudioClip> playerQueue = new Queue<AudioClip>(); //this is the player's inputs

    //the audio source
    public AudioSource aud;
    
    //whether or not the player is able to input notes
    public bool playerTurn = false;
    
    //the win/loss text
    public TextMeshProUGUI instructText;

    //the current difficulty
    public TextMeshProUGUI difficulty;

    //a grab bag - since I was having trouble with the notes repeating too many times
    public List<int> grabBag;
    
    // Start is called before the first frame update
    void Start()
    {
        //change the seed!
        Random.InitState(System.DateTime.Now.Second);
        
        //set up the grab bag
        setGrabBag();
        
        //add the very first note to the list. this is the only note that's the same every time
        noteList.Add(audOne);
        noteBackup.Push(noteList[0]);
        
        //the first instructions
        instructText.text = "Listen. Then, play it back.";
        
        //play the music notes
        Invoke("PlayNotes", 1f);
        
            //print the current difficulty 
        difficulty.text = "Current difficulty: " + noteList.Count;
    }

    //every time we re-set the grab bag, we invoke this!
    //we want to limit the number of repeats in a row, but not prevent them entirely, hence two copies of each number
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
    
    //plays the audio of the music ntoes from the list
    void PlayNotes()
    {
        //all the key/button colors start off white
        settowhite();
        
        //switch based on the name - we peek because we don't want to destroy it yet. 
        //the note that's about to play should turn green, for visual reference!
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
           case "NoteSix_v2":
                    btnSix.GetComponent<Image>().color =  Color.green;
                    break;
           default:
                    break;
        }
        //here we remove it from the stack, and play that note!
        aud.PlayOneShot(noteBackup.Pop());
        
        //so long as there are notes in the stack, we call this again to play the next note
        if (noteBackup.Count > 0) 
        {
            Invoke("PlayNotes", 0.8f);
        }
        else //otherwise, it becomes the player's turn again!
        {
            playerTurn = true;
            AddNotesToPlaylist();
            Invoke("settowhite", 1f);
        }
    }

    //sets all the key colors to white
    void settowhite()
    {
        btnOne.GetComponent<Image>().color =  Color.white;
        btnTwo.GetComponent<Image>().color =  Color.white;
        btnThree.GetComponent<Image>().color =  Color.white;
        btnFour.GetComponent<Image>().color =  Color.white;
        btnFive.GetComponent<Image>().color =  Color.white;
        btnSix.GetComponent<Image>().color =  Color.white;
    }
    
    //this increases the difficulty by adding a new note!
    void increaseDifficulty()
    {

        Debug.Log(grabBag.Count + " numbers left");
        
        //if the grab bag is empty, we re-fill it!
        if (grabBag.Count == 0)
        {
            setGrabBag();
        }

        //we get a random number between 0 and the number of items in the bag
        var rand = Random.Range(0, grabBag.Count - 1);
        
        //then we get the int at that index
        var newNote = grabBag[rand];
        
        Debug.Log(grabBag[rand] + " is the number we wanted at place " + rand);
        
        //then we remove that index from the grab bag
        grabBag.RemoveAt(rand);
        
        Debug.Log(newNote + " is the new note");
        
        //this switch is based on the int we just got
        //a new note is added to the master list based on the int we just got
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
        //add one to the difficulty - which is always based on the count, so is always synced up
        difficulty.text = "Current difficulty: " + noteList.Count;
        
        //these notes are added back to the playing stack
        AddNotesToPlaylist();
        
        //then we play the notes
        Invoke("PlayNotes", 0.1f);
    }
    
    //adds notes from the master list to the playing stack
    void AddNotesToPlaylist()
    {
        noteBackup.Clear();
        foreach (var audClip in noteList) { 
            noteBackup.Push(audClip);    
        }
    }
    
    
    //this compares the player's selections to the stack (which is based on the master list)
    void CompareStacks()
    {

        //if the count is 0, all the notes have been correct and removed!
        //after we advance to the next difficulty
        if (playerQueue.Count == 0)
        {
            instructText.text = "You did it! Here's something harder.";
            increaseDifficulty();
        } else if (playerQueue.Peek() == noteBackup.Peek())
        { //if there are notes left, but the queue and stack match up, we know it's a correct note!
            //both notes get removed from their places
            playerQueue.Dequeue();
            noteBackup.Pop();
            //then  we run this again until we hit zero
            CompareStacks();
        }
        else
        {//otherwise, if any of the notes are WRONG, we restart everything for the player
            instructText.text = "Try again";
          
            //both of these are reset before continuing
            playerQueue.Clear();
            noteBackup.Clear();
            AddNotesToPlaylist();
            Invoke("PlayNotes", 0.5f);
        }
    }

    //this function is attached to the buttons in the scene
    public void NoteButton(AudioClip audClip) //each button has been assigned an audio clip in the editor
    {
        //the player has to be on their turn for the buttons to do anything
        if (playerTurn == true)
        { //if it is true, when clicked on, we do the following:
            //1. play the clip from the button
            //2. add that clicp to the Queue
            //3. compare the length of the queue and the list to see if we've inputted all the possible notes for this round
            aud.PlayOneShot(audClip);
            playerQueue.Enqueue(audClip);
            
            //if the player has reached the number of notes that were originally played, we then check if the player got them correct!
            if (playerQueue.Count == noteList.Count)
            {
                playerTurn = false;
                Invoke("CompareStacks", 2f);
            }
        }
    }
}
