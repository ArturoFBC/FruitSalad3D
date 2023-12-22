using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class TransitionInOutPanel : MonoBehaviour
{
    [SerializeField] private float duration = 1f;

    private CanvasGroup myCanvasGroup;

    private static TransitionInOutPanel _ref;
    public static TransitionInOutPanel _Ref
    {
        get
        {
            if (_ref == null)
            {
                _ref = FindObjectOfType<TransitionInOutPanel>();

                if (_ref == null)
                    _ref = new GameObject("InstantiatedTransitionPanel", typeof(TransitionInOutPanel)).GetComponent<TransitionInOutPanel>();
            }

            return _ref;
        }
        private set
        {
            _ref = value;
        }
    }

    private void Awake()
    {
        if (_Ref != null && _Ref != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        if (myCanvasGroup == null)
            myCanvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartFade()
    {
        StartCoroutine(Fade(duration));
    }

    private IEnumerator Fade(float duration)
    {
        yield return Fade(true, duration / 3f);
        yield return new WaitForSeconds(duration / 3f);
        yield return Fade(false, duration / 3f);
    }

    private IEnumerator Fade(bool fadeIn, float duration)
    {
        gameObject.SetActive(true);
        myCanvasGroup.alpha = fadeIn ? 0f : 1;
        myCanvasGroup.enabled = true;

        float counter = 0f;

        while (counter < duration)
        {
            yield return new WaitForEndOfFrame();
            counter += Time.deltaTime;
            float progress = counter / duration;
            myCanvasGroup.alpha = fadeIn ? progress : 1 - progress;
        }

        myCanvasGroup.alpha = fadeIn ? 1f : 0;
    }


    private void SetChildrenActive( bool active )
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive( active );
        }
    }

    internal float GetDuration()
    {
        return duration / 2f;
    }
}
