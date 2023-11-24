using UnityEngine;

public class UIMainController : MonoBehaviour
{
    [SerializeField] private GameMenuController _gameMenuController;

    private void Start()
    {
        InitControllers();
    }

    private void InitControllers()
    {
        _gameMenuController.Init();
    }
}
