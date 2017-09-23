//By Joseph Giordano.
using UnityEngine;
using System.Collections;

//Attach this class to the GameObject you want the arrow to be pointing at.
public class ScreenIndicator : MonoBehaviour {

	public Texture2D icon; //The icon. Preferably an arrow pointing upwards.
	public float iconSize = 50f;
	[HideInInspector]
	public GUIStyle gooey; //GUIStyle to make the box around the icon invisible. Public so that everything has the default stats.
	Vector2 indRange;
	float scaleRes = Screen.width / 500; //The width of the screen divided by 500. Will make the GUI automatically
	//scale with varying resolutions.
	Camera cam;
	bool visible = true; //Whether or not the object is visible in the camera.

	void Start () {
		visible = GetComponent<SpriteRenderer> ().isVisible;

		cam = Camera.main; //Don't use Camera.main in a looping method, its very slow, as Camera.main actually
		//does a GameObject.Find for an object tagged with MainCamera.

		indRange.x = Screen.width - (Screen.width / 25);
		indRange.y = Screen.height - (Screen.height / 25);
		indRange /= 2f;

		gooey.normal.textColor = new Vector4 (0, 0, 0, 0); //Makes the box around the icon invisible.
	}

	void OnGUI () {
		if (!visible) {
			Vector3 dir = transform.position - cam.transform.position;
			dir.y *= -1f;
			Vector2 dir2d = new Vector2 (dir.x, dir.y);
			dir = Vector3.Normalize (dir);

			Vector2 indPos = new Vector2 (indRange.x * dir.x, indRange.y * dir.y);
			indPos = new Vector2 ((Screen.width / 2) + indPos.x,
				(Screen.height / 2) + indPos.y);

			Vector3 pdir = transform.position - cam.ScreenToWorldPoint(new Vector3(indPos.x, indPos.y,
				transform.position.z));
			pdir = Vector3.Normalize(pdir);

			float angle = Mathf.Atan2(pdir.x, pdir.y) * Mathf.Rad2Deg;
            float lenght = (dir2d.magnitude > 500 ? 5 : dir2d.magnitude / 100) / 1.2f;
            
			GUIUtility.RotateAroundPivot(angle, indPos); //Rotates the GUI. Only rotates GUI drawn after the rotate is called, not before.
			GUI.Box (new Rect (indPos.x, indPos.y, scaleRes * iconSize / lenght, scaleRes * iconSize / lenght), icon,gooey);
			GUIUtility.RotateAroundPivot(0, indPos); //Rotates GUI back to the default so that GUI drawn after is not rotated.
		}
	}

	void OnBecameInvisible() {
		visible = false;
	}
	//Turns off the indicator if object is onscreen.
	void OnBecameVisible() {
		visible = true;
	}
}