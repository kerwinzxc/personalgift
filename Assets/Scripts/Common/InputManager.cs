using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    #region Variables

    private static InputManager _instance;
	public GameObject gameView;
	public Camera mainCamera;
	private int _pressedLayer = -1;
	private GameObject _selectedObject;

    #endregion


    #region Accessors
    public int pressedLayer
    {
        get
        {
            return _pressedLayer;
        }
        set
        {
            _pressedLayer = value;
        }
    }

	public GameObject selectedObject
	{
        get
        {
            return _selectedObject;
        }
        set
        {
            _selectedObject = value;
        }
    }

    #endregion


    public static InputManager GetInstance()
    {
        if (!_instance)
        {
            _instance = (InputManager)FindObjectOfType(typeof(InputManager));

            if (!_instance)
            {
				Debug.LogError("You need to have an object of Input Manager");
            }
        }

        return _instance;
    }

	public void Initialize() 
    {
        UICamera.genericEventHandler = this.gameObject;
	}

	private void Awake()
	{
		InputManager.GetInstance().Initialize();
		MainCamera.GetInstance().Initialize();
	}

	private void Update () 
	{
		float deltaTime = Time.deltaTime;
		InputManager.GetInstance().Tick(deltaTime);
		MainCamera.GetInstance().Tick(deltaTime);
	}

    public void Tick(float deltaTime)
    {

    }

    public void SetInputLayers(params int[] layers)
    {
        if (layers.Length > 0)
        {
            int layerMask = 0;

            for (int i = 0; i < layers.Length; i++)
            {
                layerMask = layerMask | (1 << layers[i]);
            }

 //FIXME           MainCamera.GetInstance().uiCamera.eventReceiverMask = layerMask;
        }
    }

    private void OnPress(bool isDown)
    {
        if (isDown)
        {
            _pressedLayer = UICamera.lastHit.collider != null ? UICamera.lastHit.collider.gameObject.layer : -1;

            if (_pressedLayer != LayerMask.NameToLayer("GUI"))
            {
//				if (_pressedLayer == BuildingManager3D.SELECTION_LAYER)
	//				selectedObject = UICamera.lastHit.collider.gameObject;

				MainCamera.GetInstance().StartDrag();
            }
        }
        else
        {
            _pressedLayer = -1;
			selectedObject = null;
            MainCamera.GetInstance().StopDrag();
        }
    }

    private void OnClick()
    {
        //if we hit something with the click
/*        if (UICamera.lastHit.collider != null)
        {
            //find out what type of object it was based on its layer
            int cl = UICamera.lastHit.collider.gameObject.layer;

			if (cl == LayerMask.NameToLayer("UI"))
            {
                //ignore the gui layer
                return;
            }
			else if (cl == BuildingManager3D.BUILDING_LAYER)
            {
                //we only want to select a new object if we dont have a selected object, or we're not doing anything with the currently selected object
               // if (selectedObject == null)
              //  {
					selectedObject = UICamera.lastHit.collider.gameObject;//SelectObject(UICamera.lastHit.collider.gameObject.GetComponent<BaseObject>());
			//		selectedObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			//	}
                
            }
            else
            {
                //if we didnt click an object, and we have a currently selected object that is idle, then deselect it
               // if (_selectedObject == null || _selectedObject.objectState == BaseObject.ObjectState.IDLE)
             //   {
				selectedObject = null;//SelectObject(null);
             //   }
            }
        }*/
    }

    private void OnDrag(Vector2 delta)
    {
        if (UICamera.touchCount == 3)
        {
            UICamera.MouseOrTouch touch1 = UICamera.GetTouch(0);
            UICamera.MouseOrTouch touch2 = UICamera.GetTouch(1);
            UICamera.MouseOrTouch touch3 = UICamera.GetTouch(2);

            MainCamera.GetInstance().Rotate((touch1.delta.x + touch2.delta.x + touch3.delta.x) / 3f);
        }
        else if (UICamera.touchCount == 2) 
        {
            UICamera.MouseOrTouch touch1 = UICamera.GetTouch(0);
            UICamera.MouseOrTouch touch2 = UICamera.GetTouch(1);

            if (Vector2.Dot(touch1.delta.normalized, touch2.delta.normalized) <= 0f) // < 0 means the 2 touches are moving in different directions (a pinch gesture)
            {
                Vector2 curDist = touch1.pos - touch2.pos; //current distance between finger touches
                Vector2 prevDist = ((touch1.pos - touch1.delta) - (touch2.pos - touch2.delta)); //difference in previous locations using delta positions
                float touchDelta = curDist.magnitude - prevDist.magnitude;

                MainCamera.GetInstance().Zoom(-touchDelta);
            }
        }
        else if (UICamera.touchCount == 1)
        {
            if (_pressedLayer == LayerMask.NameToLayer("GUI"))
            {
                //ignore the GUI layer
                return;
            }
		/*	else if (_pressedLayer == BuildingManager3D.SELECTION_LAYER)
            {
                if (selectedObject != null)
                {
					Ray ray = mainCamera.ScreenPointToRay(UICamera.lastTouchPosition);
					RaycastHit hit;
					if(Physics.Raycast (ray, out hit, 10000, 1 << BuildingManager3D.TERRAIN_LAYER))
							selectedObject.SendMessage("OnDragEvent", hit.point - gameView.transform.position, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    MainCamera.GetInstance().Drag(delta);
                }
            } */
            else
            {
                MainCamera.GetInstance().Drag(delta);
            }
        }
    }

    private void OnRest()
    {
		if (_pressedLayer == LayerMask.NameToLayer("GUI"))
        {
            //ignore the GUI layer
            return;
        }
/*		else if (_pressedLayer == BuildingManager3D.SELECTION_LAYER)
        {
            if (selectedObject != null)
			{
				selectedObject.SendMessage("OnDragEvent", Vector3.zero, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                MainCamera.GetInstance().Drag(Vector2.zero);
            }
        }
  */      else
        {
            MainCamera.GetInstance().Drag(Vector2.zero);
        }
    }

    private void OnScroll(float delta)
    {
        MainCamera.GetInstance().Zoom(-delta * 30f);
    }
}
