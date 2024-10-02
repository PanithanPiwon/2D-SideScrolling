using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private Gradient lightColorGradient;
    [SerializeField] private TimeManager timeManager;
    private float timeElapsed;

    void Start()
    {
        if (directionalLight == null)
        {
            directionalLight = GetComponent<Light>();
        }
    }
    void Update()
    {
        UpdateAfterTimeJump();

        timeElapsed += Time.deltaTime;

        float timeNormalized = Mathf.Clamp01(timeElapsed / timeManager.GetDayDuration());
        directionalLight.color = lightColorGradient.Evaluate(timeNormalized);

        if (timeElapsed >= timeManager.GetDayDuration())
        {
            timeElapsed = 0f;
        }
    }
    private void UpdateAfterTimeJump()
    {
        timeElapsed = timeManager.GetElapsedTimeTotal();

        float timeNormalized = Mathf.Clamp01(timeElapsed / timeManager.GetDayDuration());
        directionalLight.color = lightColorGradient.Evaluate(timeNormalized);
    }
}
