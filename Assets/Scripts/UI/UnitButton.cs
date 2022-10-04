using GameLogic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    public Unit unit;
    private Button button;
    [SerializeField] private Image buttonImage, background;

    private void Start()
    {
        background = GetComponent<Image>();
    }

    public void Initialize(Unit unit, ShopController shop)
    {
        this.unit = unit;
        name = unit.name;
        button = GetComponent<Button>();
        button.onClick.AddListener(() => shop.SelectUnitItem(unit));
        buttonImage.sprite = unit.allyImage;
    }

    public void ColorizeBackground(Color color) => background.color = color;
}