using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewsUIController : MonoBehaviour
{
    public GameObject obj; 
    //public GameObject rotationPoint;

    private ViewController viewController;
    

    private void OnEnable() {
        GameObject doc = GameObject.Find("UIDocument_views");
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        viewController = ViewController.Instance;
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        Button center = root.Q<Button>("Center");
        Button anterior = root.Q<Button>("Anterior");
        Button posterior = root.Q<Button>("Posterior");
        Button lateral = root.Q<Button>("Lateral");
        Button medial = root.Q<Button>("Medial");
        Button superior = root.Q<Button>("Superior");
        Button inferior = root.Q<Button>("Inferior");
        Button close = root.Q<Button>("Close");

        center.clicked += () => {
            viewController.centerPos(obj);
        };
        anterior.clicked += () => {
            viewController.antRotation(obj);
        };
        posterior.clicked += () => viewController.posRotation(obj);
        lateral.clicked += () => viewController.latRotation(obj);
        medial.clicked += () => viewController.medRotation(obj);
        superior.clicked += () => viewController.supRotation(obj);
        inferior.clicked += () => viewController.infRotation(obj);
        close.clicked += () => {
            doc.SetActive(false);
        };
    }
}
