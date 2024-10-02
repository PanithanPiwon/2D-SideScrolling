using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public enum TimeOfDay
    {
        Morning,
        Afternoon,
        Evening
    }
    public enum WeekDay
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    [SerializeField] private TimeOfDay currentTimeOfDay;
    [SerializeField] private WeekDay currentWeekDay;
    [SerializeField] private int dayCounter;
    [SerializeField] private float timePerDay;
    [SerializeField] private Transform clockHand;
    [SerializeField] private GameObject morningFX;
    [SerializeField] private GameObject afternoonFX;
    [SerializeField] private GameObject eveningFX;

    private float elapsedTimeTotal;
    private float elapsedTimeForPeriod;
    private float timePerPeriod;
    private float degreesPerSecond;
    private float startRotationOffset = 60;

    private GameObject[] timeFX;
    private bool timeJump = false;

    public bool TimeJump { get { return timeJump; } set { timeJump = value; } }
    void Start()
    {
        timeFX = new GameObject[] { morningFX, afternoonFX, eveningFX };

        currentTimeOfDay = TimeOfDay.Morning;
        currentWeekDay = WeekDay.Monday;
        dayCounter = 0;

        timePerPeriod = timePerDay / 3f;
        elapsedTimeTotal = 0f;
        elapsedTimeForPeriod = 0f;

        degreesPerSecond = 360f / timePerDay;
    }

    void Update()
    {
        elapsedTimeTotal += Time.deltaTime;
        elapsedTimeForPeriod += Time.deltaTime;

        float rotationAmount = (elapsedTimeTotal * degreesPerSecond);
        clockHand.localRotation = Quaternion.Euler(0f, 0f, -(rotationAmount + startRotationOffset));

        if (elapsedTimeForPeriod >= timePerPeriod)
        {
            AdvanceTime();
            elapsedTimeForPeriod = 0f;
        }
    }

    public void AdvanceTime()
    {
        var part = (timePerDay / 3);

        switch (currentTimeOfDay)
        {
            case TimeOfDay.Morning:
                currentTimeOfDay = TimeOfDay.Afternoon; 
                elapsedTimeTotal = (part * 1) + .1f; 
                break;

            case TimeOfDay.Afternoon:
                currentTimeOfDay = TimeOfDay.Evening; 
                elapsedTimeTotal = (part * 2) + .1f; 
                break;

            case TimeOfDay.Evening:
                currentTimeOfDay = TimeOfDay.Morning; 
                elapsedTimeTotal = (part * 0) + 0.1f; 
                AdvanceDay(); 
                break;
        }

        ActivateFX(currentTimeOfDay);
       
        UpdateClockHandRotation(() => { timeJump = false; });

        Debug.Log($"Current Time: {currentTimeOfDay}, Current Day: {currentWeekDay}, Day Counter: {dayCounter}");
    }

    void AdvanceDay()
    {
        if (currentWeekDay == WeekDay.Sunday)
        {
            currentWeekDay = WeekDay.Monday;
        }
        else
        {
            currentWeekDay++;
        }

        dayCounter++;
    }

    public float GetDayDuration()
    {
        return timePerDay;
    }
    private void ActivateFX(TimeOfDay timeOfDay)
    {
        foreach (var fx in timeFX)
        {
            fx.SetActive(false);
        }

        timeFX[(int)timeOfDay].SetActive(true);
    }

    private void UpdateClockHandRotation(UnityAction callback = null)
    {
        float rotationAmount = (elapsedTimeTotal / timePerDay) * 360f + startRotationOffset;
        clockHand.localRotation = Quaternion.Euler(0f, 0f, rotationAmount); 
        elapsedTimeForPeriod = 0;
        callback?.Invoke();
    }
    public float GetElapsedTimeTotal() 
    {
        return elapsedTimeTotal;
    }
}
