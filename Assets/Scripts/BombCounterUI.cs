using BindingOfChars;
using TMPro;
using UnityEngine;

public class BombCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bombCounterText;
    [SerializeField] private PlayerController _playerController;

    private void Start()
    {
        //_playerController.AddUpdateBombEvent(OnUpdateBombCounter);
    }

    private void OnUpdateBombCounter()
    {
        if (_bombCounterText != null && _playerController != null)
        {
            //_bombCounterText.text = _playerController.GetBombs().ToString();
        }
    }
}

