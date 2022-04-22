using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputData inputData;

    void Update()
    {
        WriteInputData();
    }

    void WriteInputData()
    {
        inputData.isPressed = Input.GetMouseButtonDown(0);
        inputData.isHeld = Input.GetMouseButton(0);
        inputData.isReleased = Input.GetMouseButtonUp(0);
    }
}
