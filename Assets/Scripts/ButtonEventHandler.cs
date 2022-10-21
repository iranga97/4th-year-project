using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEventHandler : MonoBehaviour
{
    private MultiSelectStore multiSelectStore;
    private IsolationController isolationController;
    private void Awake()
    {
        multiSelectStore = MultiSelectStore.Instance;
        isolationController = new IsolationController();
    }
    public void onIsolateBtnClick()
    {
        isolationController.IsolateObjects(multiSelectStore.getSelectedObjects());
    }

    public void onUndoBtnClick()
    {
        isolationController.undo();
    }

    public void onRedoBtnClick()
    {
        isolationController.redo();
    }
}
