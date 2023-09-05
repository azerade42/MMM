using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : Singleton<PlayerInput>
{
    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;
    private float playerXPos;
    private float playerYPos;

    private float elapsedTime;

    [Range(-1,1)][SerializeField] float xPosition;
    public Transform currentScreen;

    public void OnMoveMouse(InputAction.CallbackContext context)
    {
        Move(context.ReadValue<Vector2>());
    }
 
    private void Start()
    {
        // Calculate the boundaries of the screen, height, width of player sprite
        screenBounds = GameManager.Instance.ScreenBounds;
        playerHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        playerWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;

        // Use the screen boundaries to clamp the x position of the player sprite
        playerXPos = screenBounds.x * -xPosition;
        playerXPos = Mathf.Clamp(playerXPos, screenBounds.x + playerWidth, -screenBounds.x - playerWidth);
    }

    // Have the camera and player constantly move right while staying in bounds
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        playerXPos = screenBounds.x * -xPosition + elapsedTime;
        playerXPos = Mathf.Clamp(playerXPos, screenBounds.x + playerWidth + elapsedTime, -screenBounds.x - playerWidth + elapsedTime);
        transform.position = playerYPos * Vector3.up + Vector3.right * playerXPos;

        Camera.main.transform.position += Vector3.right * Time.deltaTime;
    }

    // Move the player up and down the screen using the mouse while clamping the player in the screen boundaries
    private void Move(Vector2 mousePos)
    {
        float newMouseYPos = Mathf.Lerp(-1f, 1f, mousePos.y / Screen.height);
        playerYPos = newMouseYPos * -screenBounds.y;
        playerYPos = Mathf.Clamp(playerYPos, screenBounds.y + playerHeight, -screenBounds.y - playerHeight);
    }
}
