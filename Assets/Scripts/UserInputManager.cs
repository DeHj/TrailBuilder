using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    public float maxSlowDownRatio;

    private void Update()
    {
        HandleSlowdown(Input.GetAxis("Slowdown"));
    }

    private void HandleSlowdown(float ratio)
    {
        Time.timeScale = 1 + ratio * (1 / maxSlowDownRatio - 1);
    }
}
