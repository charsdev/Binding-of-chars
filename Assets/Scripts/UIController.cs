using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsCounterText;
    [SerializeField] private TextMeshProUGUI _bombsCounterText;

    private void Start()
    {
        EventBus.Instance.Subscribe<PropertyChangeArg<int>>("UPDATE_COINS", OnCoinsPropertyChanged);
        EventBus.Instance.Subscribe<PropertyChangeArg<int>>("UPDATE_BOMBS", OnBombsPropertyChanged);
    }

    private void OnCoinsPropertyChanged(PropertyChangeArg<int> data)
    {
        UpdateCounter(_coinsCounterText, data.value);
    }

    private void OnBombsPropertyChanged(PropertyChangeArg<int> data)
    {
        UpdateCounter(_bombsCounterText, data.value);
    }


    public void UpdateCounter(TextMeshProUGUI counter, int value)
    {
        counter.text = value.ToString();
    }
}

