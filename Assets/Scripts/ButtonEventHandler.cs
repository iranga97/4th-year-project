using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEventHandler : MonoBehaviour
{
    private MultiSelectStore multiSelectStore;
    private IsolationController isolationController;

    private GameObject viewPopup;
    private void Awake()
    {
        multiSelectStore = MultiSelectStore.Instance;
        isolationController = new IsolationController();
        viewPopup = GameObject.Find("UIDocument_views");
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

    public void onViewPopupOpenClick(){
        viewPopup.SetActive(true);
    }

    public void onViewPopupCloseClick(){
        viewPopup.SetActive(false);
    }
}
