﻿using UnityEngine;
using UnityEngine.UI;

public class TesseractDemoScript : MonoBehaviour
{
    public Texture2D imageToRecognize;
    [SerializeField] public Text displayText;
    [SerializeField] public RawImage outputImage;
    public CameraScript cam;
    private TesseractDriver _tesseractDriver;
    private string _text = "";
    private Texture2D _texture;

    private void Start()
    {
        //Texture2D texture = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
        //texture.SetPixels32(imageToRecognize.GetPixels32());
        //texture.Apply();
        cam = new CameraScript();
        _tesseractDriver = new TesseractDriver();   
        //Recognize(texture);
        //SetImageDisplay();
    }
    private void OnGUI()
    {
        

        //_tesseractDriver = new TesseractDriver();
        if (GUI.Button(new Rect(180, 180, 90, 90), "Click"))
        {
           imageToRecognize = cam.TakeSnapshot();
            Texture2D texture2 = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
            texture2.SetPixels32(imageToRecognize.GetPixels32());
            texture2.Apply();
            if (imageToRecognize != null)
            {
                Recognize(texture2);
                //SetImageDisplay();

            }
        }
    }

    private void Recognize(Texture2D outputTexture)
    {
        _texture = outputTexture;
        ClearTextDisplay();
        AddToTextDisplay(_tesseractDriver.CheckTessVersion());
        _tesseractDriver.Setup(OnSetupCompleteRecognize);
        //Debug.Log("Setup başarılı");
        //AddToTextDisplay(_tesseractDriver.Recognize(outputTexture));
        //AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
    }
    private void OnSetupCompleteRecognize()
    {
        AddToTextDisplay(_tesseractDriver.Recognize(_texture));
        AddToTextDisplay(_tesseractDriver.GetErrorMessage(), true);
        SetImageDisplay();
    }

    private void ClearTextDisplay()
    {
        _text = "";
    }

    private void AddToTextDisplay(string text, bool isError = false)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        _text += (string.IsNullOrWhiteSpace(displayText.text) ? "" : "\n") + text;

        if (isError)
            Debug.LogError(text);
        else
            Debug.Log(text);
    }

    private void LateUpdate()
    {
        if (displayText.text!=null && _text!=null)
            displayText.text = _text;
    }

    private void SetImageDisplay()
    {
        RectTransform rectTransform = outputImage.GetComponent<RectTransform>();
        Debug.Log(rectTransform);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            rectTransform.rect.width * _tesseractDriver.GetHighlightedTexture().height / _tesseractDriver.GetHighlightedTexture().width);
        outputImage.texture = _tesseractDriver.GetHighlightedTexture();
    }
}