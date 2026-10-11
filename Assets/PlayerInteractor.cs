using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    // Увеличим дальность для отладки
    public float interactionRange = 10f;

    void Update()
    {
        // Проверяем нажатие клавиши E (новый Input System)
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("Нажата клавиша E!");

            // Создаем луч из центра камеры
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // --- ДОБАВЬТЕ ЭТУ СТРОКУ ДЛЯ ВИЗУАЛИЗАЦИИ ЛУЧА ---
            // Луч будет виден только в окне Scene в течение 3 секунд красным цветом.
            Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red, 3f);
            // ----------------------------------------------------

            // Испускаем луч
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionRange))
            {
                Debug.Log("Луч попал в объект: " + hit.collider.name);

                // Проверяем, есть ли на объекте скрипт взаимодействия
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
                else
                {
                    Debug.Log("У объекта '" + hit.collider.name + "' нет компонента IInteractable");
                }
            }
            else
            {
                Debug.Log("Луч никуда не попал");
            }
        }
    }
}