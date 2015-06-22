using UnityEngine;
using System.Collections;

namespace Album3D {

public class Screen : CoreBehaviour {

	public GameObject LoadingIcon;
	public Texture screenMaterial;
	public int screenMaterialIndex = 0;
	public string ImageName;
	public ScreenEffect screenEffect;

	public int Priority =1;
	// Use this for initialization
	void Start () {
		ScreenManager.GetInstance ().AddScreen (this,Priority);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//onclick from ngui
	void OnClick() {

	}

	public int GetPriority () {
		return Priority;
	}

	public void SetPriority (int p) {
		Priority = p;
	}

	private Material GetMaterial() {
			return renderer.materials [screenMaterialIndex];
}

	public void SetTexture(Texture texture) {
		if (renderer != null) {
				GetMaterial().mainTexture = texture;//screenMaterial;
				//Material mat = new Material( renderer.material); 
				//mat.mainTexture = texture;
				//renderer.material = mat;
						}
	}

	public void SetMaterial(Material material) {
		if (renderer != null)
				renderer.materials [screenMaterialIndex] = material;
	}

	public void SetShader(Shader shader) {
		if (renderer != null) 
				GetMaterial().shader = shader;
	}

	public void SetShader(string shader) {
		if (renderer != null)
				GetMaterial().shader = Shader.Find (shader);
	}

	//TODO : have list of all effects in effectmanager
	public void SetEffect (ScreenEffect effect) {
		screenEffect = effect;
		ScreenEffectManager.GetInstance ().SetScreenEffect (this, effect);
	}

	public void SetLoading(bool on=true) {
//		LoadingIcon.SetActive (on);
	}

}

}