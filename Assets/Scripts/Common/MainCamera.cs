using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    #region Variables

	public static bool IS_PANNING_ENABLED = true;
	public static bool IS_ZOOMING_ENABLED = true;
	public static bool IS_ROTATION_ENABLED = true;

    private static MainCamera _instance;
    private static Camera _cachedCamera;
    private static UICamera _cachedUicamera;

    private const float CAMERA_SPEED = 0.075f;
    private const float CAMERA_DRAG = 8f;
    private const float CAMERA_MOMENTUM_COEFFICIENT = 2f;

    private const float CAMERA_ROTATE_SPEED = 0.1f;

    private const float CAMERA_ZOOM_COEFFICIENT = 0.1f;

    public const float CAMERA_MIN_ZOOM_PERSP = 1f;
    public const float CAMERA_MAX_ZOOM_PERSP = 50f;

    private static bool _isRotating;
    private static bool _useMomentum;
    private static float _xMomentum;
    private static float _zMomentum;

    private static float _zoomDistance = CAMERA_MAX_ZOOM_PERSP;
    
    #endregion


    #region Accessors

    public UICamera uiCamera
    {
        get
        {
            if (!_cachedUicamera)
            {
                _cachedUicamera = this.GetComponent<UICamera>();
            }

            return _cachedUicamera;
        }
    }

    public Camera cachedCamera
    {
        get
        {
            if (!_cachedCamera)
            {
                _cachedCamera = this.GetComponent<Camera>();
            }

            return _cachedCamera;
        }
    }

    public bool isRotating
    {
        get { return _isRotating; }
        set { _isRotating = value; }
    }

    public float zoomDistance
    {
        get
        {
            return _zoomDistance;
        }
    }

    #endregion

	public Transform cameraBounds;
	
	public static float screenDamping_x
	{
		get
		{
			return 854f / (float)Screen.width;
		}
	}
	
	public static float screenDamping_y
	{
		get
		{
			return 480f / (float)Screen.height;
		}
	}
	
	public static float screenDamping_realWorld_x
	{
		get
		{
			//            if (Application.platform == RuntimePlatform.Android)
			//            {
			//                return ((float)Screen.width / DisplayMetricsAndroid.XDPI) / (1280f / 195.3846f);
			//            }
			//            else
			//            {
			return screenDamping_x;
			//            }
		}
	}
	
	public static float screenDamping_realWorld_y
	{
		get
		{
			//            if (Application.platform == RuntimePlatform.Android)
			//            {
			//                return ((float)Screen.height / DisplayMetricsAndroid.YDPI) / (800f / 200.6914f);
			//            }
			//            else
			//            {
			return screenDamping_y;
			//            }
		}
	}
	
	public float cameraBounds_minX
	{
		get
		{
			return cameraBounds.position.x - cameraBounds.localScale.x / 2f;
		}
	}
	
	public float cameraBounds_maxX
	{
		get
		{
			return cameraBounds.position.x + cameraBounds.localScale.x / 2f;
		}
	}
	
	public float cameraBounds_minY
	{
		get
		{
			return cameraBounds.position.z - cameraBounds.localScale.z / 2f;
		}
	}
	
	public float cameraBounds_maxY
	{
		get
		{
			return cameraBounds.position.z + cameraBounds.localScale.z / 2f;
		}
	}

    public static MainCamera GetInstance()
    {
        if (!_instance)
        {
            _instance = (MainCamera)FindObjectOfType(typeof(MainCamera));

            if (!_instance)
            {
				Debug.LogError("There should be Main Camera attached");//_instance = Constants.GetInstance().mainCamera.gameObject.AddComponent<MainCamera>();
            }
        }

        return _instance;
    }

    public void Initialize()
    {
        _cachedCamera = this.GetComponent<Camera>();
    }

    public void StartDrag()
    {
		if(!IS_PANNING_ENABLED){
			return;
		}

        _useMomentum = false;
    }

    public void StopDrag()
    {
		if(!IS_PANNING_ENABLED){
			return;
		}

        _useMomentum = true;
    }

    public void Drag(Vector2 delta)
    {
		////TODO : FIXME : MANJEET
		return;

		if(!IS_PANNING_ENABLED){
			return;
		}
		float xDamping = screenDamping_realWorld_x;
		float yDamping = screenDamping_realWorld_y;

        _xMomentum = -delta.x * CAMERA_SPEED * xDamping * CAMERA_MOMENTUM_COEFFICIENT;
        _zMomentum = -delta.y * CAMERA_SPEED * yDamping * CAMERA_MOMENTUM_COEFFICIENT;

        Vector3 pos = new Vector3(-delta.x * CAMERA_SPEED * xDamping, 0f, -delta.y * CAMERA_SPEED * yDamping);
        pos = (pos.x * this.transform.parent.right) + (pos.z * this.transform.parent.forward);
        this.transform.parent.position += pos;

        ClampToBounds();
    }

    public void Rotate(float delta)
    {
		if(!IS_ROTATION_ENABLED){
			return;
		}
	    float xDamping = screenDamping_realWorld_x;
	    Vector3 euler = this.transform.parent.rotation.eulerAngles;
	    euler += new Vector3(0f, delta * CAMERA_ROTATE_SPEED * xDamping, 0f);
	    this.transform.parent.rotation = Quaternion.Euler(euler);
    }

    public void Zoom(float delta)
    {
		////TODO : FIXME : MANJEET
		return;
		if (!IS_ZOOMING_ENABLED) {
			return;
		}

		delta = delta * screenDamping_realWorld_x * CAMERA_ZOOM_COEFFICIENT;
		_zoomDistance = Mathf.Clamp(_zoomDistance + delta, CAMERA_MIN_ZOOM_PERSP, CAMERA_MAX_ZOOM_PERSP);
		this.transform.position = this.transform.position - this.transform.forward * _zoomDistance;
    }

    private void AddMomentum(float deltaTime)
    {
        Vector3 momentum = new Vector3(_xMomentum, 0f, _zMomentum);
        momentum = (momentum.x * this.transform.parent.right) + (momentum.z * this.transform.parent.forward);
        this.transform.parent.position += momentum;
        ClampToBounds();
    }

    private void ClampToBounds()
    {
        Vector3 pos = this.transform.parent.position;

        pos.x = Mathf.Clamp(pos.x, cameraBounds_minX, cameraBounds_maxX);
        pos.z = Mathf.Clamp(pos.z, cameraBounds_minY, cameraBounds_maxY);

        this.transform.parent.position = pos;
    }

    public void Tick(float deltaTime)
    {
		////TODO : FIXME : MANJEET
		return;

        if (_useMomentum && (Mathf.Abs(_xMomentum) > 0f || Mathf.Abs(_zMomentum) > 0f))
        {
            AddMomentum(deltaTime);
            _xMomentum = Mathf.Lerp(_xMomentum, 0f, CAMERA_DRAG * deltaTime);
            _zMomentum = Mathf.Lerp(_zMomentum, 0f, CAMERA_DRAG * deltaTime);
        }
    }

    //private void OnGUI()
    //{
    //    GUILayout.Label("");
    //    GUILayout.Label("Density: " + DisplayMetricsAndroid.Density);
    //    GUILayout.Label("DensityDPI: " + DisplayMetricsAndroid.DensityDPI);
    //    GUILayout.Label("ScaledDenisty: " + DisplayMetricsAndroid.ScaledDensity);
    //    GUILayout.Label("XDPI: " + DisplayMetricsAndroid.XDPI);
    //    GUILayout.Label("YDPI: " + DisplayMetricsAndroid.YDPI);
    //    GUILayout.Label("Width: " + Screen.width);
    //    GUILayout.Label("Height: " + Screen.height);
    //    GUILayout.Label("Width (Inches): " + ((float)Screen.width / DisplayMetricsAndroid.XDPI));
    //    GUILayout.Label("Height (Inches): " + ((float)Screen.height / DisplayMetricsAndroid.YDPI));
    //}
}
