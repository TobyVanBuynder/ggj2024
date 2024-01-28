using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum TickleButtonType {
    A,
    B,
    X,
    Y,
    LEFT,
    RIGHT,
    UP,
    DOWN,
    MAX
}

public class TickleButtonPrompt : MonoBehaviour
{
    public TickleButtonType button;
    public Sprite keyboardSprite;
    public Sprite gamepadSprite;

    private Image _sprite;

    void Awake()
    {
        _sprite = GetComponent<Image>();
    }

    void Start()
    {
        // if is kb then take the right spriterino
        // is joystick question Mark?
        _sprite.sprite = Input.GetJoystickNames().Length > 0 ? gamepadSprite : keyboardSprite;
    }

    public void Show()
    {
        _sprite.color = Color.white;
        transform.localScale = new Vector3(1.5f, 1.5f, 1);
    }

    public void Complete()
    {
        _sprite.color = new Color(1,1,1, 0.5f);
        transform.localScale = Vector3.one;
    }
}
