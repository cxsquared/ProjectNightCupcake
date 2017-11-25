using UnityEngine;

public class Crosshair : MonoBehaviour {

    [SerializeField]
    private Texture2D _crosshairImage;
    public Texture2D CrosshairImage { get { return _crosshairImage; } private set { _crosshairImage = value; } }

    [SerializeField]
    private static float _alpha = .4f;
    private static float Alpha { get { return _alpha; } set { _alpha = Mathf.Clamp(value, 0, 1); } }

    [SerializeField]
    private float _alphaFadeRate = 1f;
    private float AlphaFadeRate { get { return _alphaFadeRate; } set { _alphaFadeRate = value; } }

    private static float startingAlpha;
    private static float targetAlpha;

    public static void ResetAlpha()
    {
        SetAlpha(startingAlpha); 
    }

    public static void SetAlpha(float alpha)
    {
        targetAlpha = alpha;
    }

    private void Awake()
    {
        startingAlpha = Alpha;
    }

    private void Update()
    {
       if (Alpha != targetAlpha)
        {
            Alpha = Mathf.Lerp(Alpha, targetAlpha, Time.deltaTime * AlphaFadeRate);
        }
    }

    private void OnGUI()
    {
        float xMin = (Screen.width / 2) - (CrosshairImage.width / 2);
        float yMin = (Screen.height / 2) - (CrosshairImage.height / 2);

        var originalColor = GUI.color;

        GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, Alpha); 
        GUI.DrawTexture(new Rect(xMin, yMin, _crosshairImage.width, _crosshairImage.height), CrosshairImage);
        GUI.color = originalColor;
    }
}
