using GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class UnitDetailDisplay : SingletonBase<UnitDetailDisplay>
    {
        [SerializeField] private TMP_InputField unitName;
        [SerializeField] private TMP_Text unitDescription;
        [SerializeField] private Button upgradeButton, sellButton, cancelButton;
        [SerializeField] private Image image;

        public UnityEvent onDisplayClosed;

        private UnityAction onUpdate;

        public static void ShowUnitDetails(UnitBehaviour unit)
        {
            if (unit == null) return;
            if (unit.isAlly)
                instance.ShowUpgrade(unit);
            else
                instance.ShowEnemy(unit);
            // else
            //     instance.HideDisplay();
        }

        public static void ShowShopDetails(Unit unit)
        {
            if (unit != null)
                instance.ShowShop(unit);
            // else
            //     instance.HideDisplay();
        }

        public static void ShowTerrainDetail(MapTerritory territory)
        {
            if (territory != null)
                instance.ShowTerrain(territory);
        }

        private void ShowUpgrade(UnitBehaviour unit)
        {
            if (unit == null)
            {
                HideDisplay();
                return;
            }

            unitName.text = unit.name;
            unitName.onEndEdit.AddListener(n => unit.name = n);
            unitName.enabled = true;

            unitDescription.text = unit.upgradeDescription;
            image.sprite = unit.unit.allyImage;

            onUpdate = () => ShowUpgrade(unit);
            HideButtons();
            gameObject.SetActive(true);

            // upgrade button
            if (unit.level < unit.unit.maxLevel)
                EnableButton(upgradeButton, $"Upgrade {unit.upgradeCosts}s", unit.Upgrade);
            upgradeButton.enabled = unit.upgradeCosts < TimeAccount.GetTime();

            // sell button
            if (unit.sellCosts > 0)
                EnableButton(sellButton, $"Sell {unit.sellCosts}s", () =>
                {
                    unit.Sell();
                    HideDisplay();
                });
        }

        private void ShowEnemy(UnitBehaviour unit)
        {
            unitName.text = unit.name;
            unitName.enabled = false;
            unitDescription.text = unit.unit.description;
            image.sprite = unit.unit.enemyImage;

            onUpdate = () => ShowEnemy(unit);
            HideButtons();
            gameObject.SetActive(true);
        }

        private void ShowShop(Unit unit)
        {
            unitName.text = unit.name;
            unitName.enabled = false;
            unitDescription.text = unit.shopDescription;
            image.sprite = unit.allyImage;

            onUpdate = () => ShowShop(unit);
            HideButtons();
            gameObject.SetActive(true);
        }

        private void ShowTerrain(MapTerritory territory)
        {
            unitName.text = territory.name;
            unitDescription.text = territory.fullDescription;
            image.sprite = territory.image;

            onUpdate = () => ShowTerrain(territory);
            HideButtons();
            gameObject.SetActive(true);
        }

        public void HideDisplay()
        {
            gameObject.SetActive(false);
            onDisplayClosed?.Invoke();
            unitName.onEndEdit.RemoveAllListeners();
        }

        private void HideButtons()
        {
            upgradeButton.gameObject.SetActive(false);
            sellButton.gameObject.SetActive(false);
            // cancelButton.gameObject.SetActive(false);
        }

        private static void EnableButton(Button button, string text, UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);

            button.GetComponentInChildren<TMP_Text>().text = text;
            button.gameObject.SetActive(true);
        }

        private void Start()
        {
            EnableButton(cancelButton, "Close", HideDisplay);
            HideDisplay();
        }

        private void Update()
        {
            onUpdate?.Invoke();
        }
    }
}