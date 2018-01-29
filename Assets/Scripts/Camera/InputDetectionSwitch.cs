using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hack to make our touch and mouse solution work.
// Problem:
// The event trigger doesn't seem to register all mouse inputs when also having a standard layer (of TouchScript) registering Screen Space UI.
// So this class will detect both mouse activity and TUIO activity to activate a standard layer, registering SS-UI, on when getting mouse input
// and off when getting TUIO activity.

public class InputDetectionSwitch : MonoBehaviour {

    [SerializeField]
    TouchScript.Layers.StandardLayer layerUI;

    [SerializeField]
    TouchScript.Layers.StandardLayer layerCamera;

    TouchScript.ILayerManager layerManager;
    TouchScript.ITouchManager touchManager;

    private Vector3 mousePositionLast;
    public float mouseMoveThreshold = 0f;

	// Use this for initialization
	void Start () {
        mousePositionLast = Input.mousePosition;
        layerManager = TouchScript.LayerManager.Instance;
        layerManager.RemoveLayer(layerUI);
        touchManager = TouchScript.TouchManager.Instance;
	}

    void OnEnable()
    {
        layerCamera.PointerBegan += DetectedLayerInput;
    }

    void OnDisable()
    {
        layerCamera.PointerBegan -= DetectedLayerInput;
    }

    bool ContainsLayer(TouchScript.Layers.TouchLayer layer)
    {
        for (int i = 0; i < layerManager.LayerCount; i++)
        {
            if (layerManager.Layers[i].GetInstanceID() == layerUI.GetInstanceID())
            {
                return true;
            }
        }

        return false;
    }

    void DetectedLayerInput(object sender, System.EventArgs e)
    {
        //Debug.Log("DetectedLayerInput");
        //if (!layerUI.isActiveAndEnabled)
        if (!ContainsLayer(layerUI))
        {
            Debug.Log("Enable UI Layer");
            layerManager.AddLayer(layerUI);
            //layerUI.enabled = true;
        }
    }

    void DetectedMouseAction()
    {
        //Debug.Log("DetectedMouseMovement");
        //if (layerUI.isActiveAndEnabled)
        if (ContainsLayer(layerUI))
        {
            Debug.Log("Disable UI Layer");
            layerManager.RemoveLayer(layerUI);
            //layerUI.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("Mouse movement: " + Vector3.SqrMagnitude(Input.mousePosition - mousePositionLast));
        if (Vector3.SqrMagnitude(Input.mousePosition - mousePositionLast) > mouseMoveThreshold)
        {
            DetectedMouseAction();
        }

        mousePositionLast = Input.mousePosition;
	}
}
