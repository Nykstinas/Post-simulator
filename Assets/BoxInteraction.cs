using UnityEngine;
using UnityEngine.InputSystem;

public class BoxInteraction : MonoBehaviour, IInteractable
{
    private bool isHeld = false;
    private Transform holdPoint;
    private Rigidbody rb;
    private Renderer boxRenderer;
    private Color originalColor;
    public Color highlightColor = Color.cyan;

    // Сила броска
    public float throwForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxRenderer = GetComponent<Renderer>();
        if (boxRenderer != null)
        {
            originalColor = boxRenderer.material.color;
        }

        GameObject hp = GameObject.Find("HoldPoint");
        if (hp != null)
        {
            holdPoint = hp.transform;
        }
    }

    void Update()
    {
        if (isHeld)
        {
            // Если предмет в руках, проверяем нажатие клавиши Q для броска
            if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
            {
                Throw();
                return;
            }

            // Следуем за точкой удержания
            if (holdPoint != null)
            {
                transform.position = holdPoint.position;
                transform.rotation = holdPoint.rotation;
            }
        }
    }

    public void Interact()
    {
        if (!isHeld)
        {
            PickUp();
        }
        else
        {
            Drop();
        }
    }

    public void OnFocus()
    {
        if (boxRenderer != null && !isHeld)
        {
            boxRenderer.material.color = highlightColor;
        }
    }

    public void OnLoseFocus()
    {
        if (boxRenderer != null)
        {
            boxRenderer.material.color = originalColor;
        }
    }

    void PickUp()
    {
        if (holdPoint == null)
        {
            GameObject hp = GameObject.Find("HoldPoint");
            if (hp != null) holdPoint = hp.transform;
            else return;
        }

        isHeld = true;
        rb.isKinematic = true;
        rb.detectCollisions = false;
        rb.useGravity = false;

        OnLoseFocus();
    }

    void Drop()
    {
        isHeld = false;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;
    }

    void Throw()
    {
        isHeld = false;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;

        // Придаем импульс в направлении взгляда камеры
        if (Camera.main != null)
        {
            Vector3 throwDirection = Camera.main.transform.forward;
            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }

        Debug.Log("Предмет брошен!");
    }
}