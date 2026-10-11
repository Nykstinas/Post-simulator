using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    private Transform cameraTransform;

    void Start()
    {
        // Автоматически ищем CharacterController на этом же объекте
        controller = GetComponent<CharacterController>();

        // Если его почему-то нет, автоматически добавляем
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (cameraTransform == null) return;

        // --- Вращение камеры мышей ---
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * 0.1f;

            verticalRotation -= mouseDelta.y;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

            transform.Rotate(Vector3.up * mouseDelta.x);
        }

        // --- Движение WASD ---
        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) moveX = -1f;
            if (Keyboard.current.dKey.isPressed) moveX = 1f;
            if (Keyboard.current.wKey.isPressed) moveZ = 1f;
            if (Keyboard.current.sKey.isPressed) moveZ = -1f;
        }

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // Гравитация
        controller.Move(Vector3.down * 9.81f * Time.deltaTime);
    }
}