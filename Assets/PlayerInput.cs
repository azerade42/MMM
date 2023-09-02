using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private float objectXPos;
    private float objectYPos;

    private float elapsedTime;

    [Range(-1,1)][SerializeField] float xPosition;

    public void OnMoveMouse(InputAction.CallbackContext context)
    {
        Move(context.ReadValue<Vector2>());
    }
 
    private void Start()
    {
        // Calculate the boundaries of the screen, height, width of player sprite
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;

        // Use the screen boundaries to clamp the x position of the player sprite
        objectXPos = screenBounds.x * -xPosition;
        objectXPos = Mathf.Clamp(objectXPos, screenBounds.x + objectWidth, -screenBounds.x - objectWidth);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        objectXPos = screenBounds.x * -xPosition + elapsedTime;
        objectXPos = Mathf.Clamp(objectXPos, screenBounds.x + objectWidth + elapsedTime, -screenBounds.x - objectWidth + elapsedTime);
        transform.position = objectYPos * Vector3.up + Vector3.right * objectXPos;

        Camera.main.transform.position += Vector3.right * Time.deltaTime;
    }

    // Move the player up and down the screen using the mouse while clamping the player in the screen boundaries
    private void Move(Vector2 mousePos)
    {
        float newMouseYPos = Mathf.Lerp(-1f, 1f, mousePos.y / Screen.height);
        objectYPos = newMouseYPos * -screenBounds.y;
        objectYPos = Mathf.Clamp(objectYPos, screenBounds.y + objectHeight, -screenBounds.y - objectHeight);
    }
}
