using UnityEngine;
using System.Collections;

public class CoRoutines : MonoBehaviour {

    void Start()
    {
        Example1();
        //StartCoroutine(Example2());
    }

    IEnumerator Print10Lines()
    {
        Debug.Log("CoRoutine Started");
        for (int i = 0; i < 10; i++)
        {
            print("Line: " + i.ToString() + " processed");
            yield return new WaitForSeconds(2);
        }
    }

    void Example1()
    {
        StartCoroutine(Print10Lines());
        print("I started printing lines");
    }

    IEnumerator Example2()
    {
        yield return StartCoroutine(Print10Lines());
        print("I have finished printing lines");
    }

}
