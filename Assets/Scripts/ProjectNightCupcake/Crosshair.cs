using UnityEngine;

public class Crosshair : MonoBehaviour {

    [SerializeField]
    private Texture2D _crosshairImage;
    public Texture2D CrosshairImage { get { return _crosshairImage; } private set { _crosshairImage = value; } }

    private void OnGUI()
    {
        float xMin = (Screen.width / 2) - (CrosshairImage.width / 2);
        float yMin = (Screen.height / 2) - (CrosshairImage.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, _crosshairImage.width, _crosshairImage.height), CrosshairImage);
    }
}
