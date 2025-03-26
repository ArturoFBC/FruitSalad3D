using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class GoToMainButton : MonoBehaviour
{
    private Button myButton;
    [SerializeField] private string sceneName;

    private void Awake()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(GoToScene);
    }

    private void GoToScene()
    {
        StartCoroutine(ChangeSceneWithTransition());
    }

    private IEnumerator ChangeSceneWithTransition()
    {
        TransitionInOutPanel._Ref.StartFade();
        yield return new WaitForSeconds(TransitionInOutPanel._Ref.GetDuration() / 2f);
        ChangeScene();
    }

    private void ChangeScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = true;
        operation.completed += Operation_completed;
    }

    private void Operation_completed(AsyncOperation obj)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);

        obj.completed -= Operation_completed;
    }
}
