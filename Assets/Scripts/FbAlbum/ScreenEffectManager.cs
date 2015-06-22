using UnityEngine;
using System.Collections;

namespace Album3D {

public class ScreenEffectManager : Manager<ScreenEffectManager> {
	/* Implement this function */
	override public void StartInit(){
		
	}
	
	/* Implement this function */
	override public void PopulateDependencies()
	{}

	public void SetScreenEffect(Screen screen, ScreenEffect effect) {
		
	}
}

public enum ScreenEffect {
	NORMAL
};

}