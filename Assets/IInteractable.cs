public interface IInteractable
{
    void Interact();
    void OnFocus();   // Вызывается, когда игрок навел прицел
    void OnLoseFocus(); // Вызывается, когда игрок отвел прицел
}