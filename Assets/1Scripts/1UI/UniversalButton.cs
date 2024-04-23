using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UniversalButton : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _buttonText;
    [SerializeField] private TMPro.TextMeshProUGUI _valueText;
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;



    public void Setup(Enums.UniversalButtonType openType, Action callback, int value = 1)
    {

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => callback?.Invoke());


        if (openType == Enums.UniversalButtonType.available)
        {
            _button.interactable = true;
            _buttonText.text = "Choose";

            _icon.gameObject.SetActive(false);
            _valueText.gameObject.SetActive(false);
        }
        else if (openType == Enums.UniversalButtonType.unavailable)
        {
            _button.interactable = false;
            _buttonText.text = "Unavailable";

            _icon.gameObject.SetActive(false);
            _valueText.gameObject.SetActive(false);
        }
        else if (openType == Enums.UniversalButtonType.choosen)
        {
            _button.interactable = false;
            _buttonText.text = "Choosen";

            _icon.gameObject.SetActive(false);
            _valueText.gameObject.SetActive(false);
        }


        else if (openType == Enums.UniversalButtonType.canBuy)
        {
            _button.interactable = true;
            _buttonText.text = "Buy";

            _icon.gameObject.SetActive(true);
            _valueText.gameObject.SetActive(true);
            _valueText.text = value.ToString();
        }
        else if (openType == Enums.UniversalButtonType.canSell)
        {
            _button.interactable = true;
            _buttonText.text = "Sell";

            _icon.gameObject.SetActive(true);
            _valueText.gameObject.SetActive(true);
            _valueText.text = Mathf.RoundToInt(value).ToString();
        }
        else if (openType == Enums.UniversalButtonType.notEnoughMoney)
        {
            _button.interactable = false;
            _buttonText.text = "Not Enough Money";

            _icon.gameObject.SetActive(false);
            _valueText.gameObject.SetActive(false);
        }



        else if (openType == Enums.UniversalButtonType.backButton)
        {
            _button.interactable = true;
            _buttonText.text = "Back";

            _icon.gameObject.SetActive(false);
            _valueText.gameObject.SetActive(false);
        }
    }

}

