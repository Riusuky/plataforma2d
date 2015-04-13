using System;
using UnityEngine;
public class Events {
    //Delegate method definition
    public delegate void ClickAction();

    //Event hook using delegate method signature
    public static event ClickAction OnClicked;

    //Safe method for calling the event
    void Clicked()
    {
        //Trigger the event delegate if there is a subscriber
        if (OnClicked != null)
        {
            OnClicked();
        }
    }

    void Update()
    {
        //if the space bar is pressed, this item has been clicked
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Clicked();
        }
    }

    void Start()
    {
        //Hook on to the functions onClicked event and run the Events_OnClicked method when it occurs
        OnClicked += Events_OnClicked;
    }

    void OnDestroy()
    {
        //Unsubscribe from the event to clean up
        OnClicked -= Events_OnClicked;
    }

    //Subordinate method
    void Events_OnClicked()
    {
        Debug.Log("The button was clicked");
    }

    #region event template
    //Logging template to send a string / report everytime something happens
    public delegate void LogMessage(string message);
    public static event LogMessage Log;

    void OnLog(string message)
    {
        if (Log != null)
        {
            Log(message);
        }
    }
    #endregion

}
