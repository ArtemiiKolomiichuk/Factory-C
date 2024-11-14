using System;
using UnityEngine;

public class DayController : MonoBehaviour
{

    public static DayController Instance { get; private set; }
    private uint currentDay;
    private uint currentDayTime;

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public uint GetCurrentDay() {
        throw new NotImplementedException();
    }

    public uint GetCurrentDayTime() {
        throw new NotImplementedException();
    }

    private void ChangeCurrentDayTime() {
        throw new NotImplementedException();
    }

    private void NextDay() {
        throw new NotImplementedException();
    }

}
