using System.Collections;

using TMPro;
using UnityEngine;

public class ConversationListener : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _text;

    private Coroutine _fadeCoroutine;

    private void Start()
    {
        SetText(string.Empty, Color.clear);
    }

    public void SetText(string text, Color color)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _canvasGroup.alpha = 0;
            _text.text = text;
            return;
        }

        _canvasGroup.alpha = 1;
        _text.text = text;
        _text.color = color;

        if (_fadeCoroutine != null )StopCoroutine( _fadeCoroutine );
        _fadeCoroutine = StartCoroutine(fadeOut());
    }

    private IEnumerator fadeOut()
    {
        while (_canvasGroup.alpha  > 0)
        {
            yield return null;
            _canvasGroup.alpha -= Time.deltaTime / 4;
        }
    }
}
