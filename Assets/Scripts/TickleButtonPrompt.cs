using UnityEngine;
using UnityEngine.InputSystem;

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

    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // if is kb then take the right spriterino
        if (InputSystem.devices.Count > 1)
        {
            // is joystick question Mark?
        }

        _spriteRenderer.sprite = keyboardSprite;
    }

    public void Show()
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1);
    }

    public void Complete()
    {
        _spriteRenderer.material.color = new Color(1,1,1, 0.5f);
        transform.localScale = Vector3.one;
    }
}
