using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button newGameButton;
    Button continueButton;
    Button quitButton;
    PlayableDirector director;

    private void Awake()
    {
        newGameButton = transform.GetChild(1).GetComponent<Button>();
        continueButton = transform.GetChild(2).GetComponent<Button>();
        quitButton = transform.GetChild(3).GetComponent<Button>();

        quitButton.onClick.AddListener(QuitGame);
        continueButton.onClick.AddListener(ContinueGame);
        newGameButton.onClick.AddListener(PlayStartTimeLine);

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
    }

    void PlayStartTimeLine()
    {
        director.Play();
    }
    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();//ɾ�����м�¼
        SceneController.Instance.TransitionToFirstLevel();
       // Debug.Log("���¿�ʼ��Ϸ");

    }

    void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadedGame();
        //������Ϸ
       // Debug.Log("������Ϸ");
    }

    void QuitGame()
    {
        Application.Quit();
      //  Debug.Log("�˳���Ϸ");
    }
}
