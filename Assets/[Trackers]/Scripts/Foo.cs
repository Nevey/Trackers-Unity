using System;
using UnityEngine;

// File "Foo.cs"
public class Foo
{
    public event Action DoSomethingEvent;

    // Fire the even from this method, it's not happening in this example because we need a reason to fire it!
    private void DispatchDoSomething()
    {
        // Option 1
        DoSomethingEvent?.Invoke();
        
        // Option 2
        if (DoSomethingEvent != null)
        {
            DoSomethingEvent();
        }
    }
}

// File "Bar.cs"
public class Bar : MonoBehaviour
{
    // Assign a reference to "Foo" via the editor
    [SerializeField] private Foo foo;

    private void Awake()
    {
        foo = new Foo();
        
        // Start listening to the event, if the event is fired before we're listening, nothing will happen
        foo.DoSomethingEvent += OnDoSomething;
    }

    private void OnDestroy()
    {
        // Stop listening to the event, clean up your stuff!
        foo.DoSomethingEvent -= OnDoSomething;
    }
    
    private void OnDoSomething()
    {
        // This will now be shown every time the event is fired
        Debug.Log("Foo.DoSomthingEvent was fired!");
    }
}