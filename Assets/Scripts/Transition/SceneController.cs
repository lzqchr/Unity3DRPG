using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    public GameObject playerPrefab;

    public SceneFader sceneFaderPrefab;

    GameObject player;

    NavMeshAgent agent;

    bool fadeFinish;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);//�ڼ���ʱ�����ٸýű�
    }

    private void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinish = true;
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch(transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                //����Э�̣����͵����غõĳ����У�TransitionDestination�����ӦTag��λ��
                StartCoroutine(Transition(SceneManager.GetActiveScene().name,transitionPoint.destinationTag));

                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }

    IEnumerator Transition(string sceneName,TransitionDestination.DestinationTag destinationTag)
    {
        
        //����֮ǰ��������
        SaveManager.Instance.SavePlayerData();//����ͬ��������

        if (SceneManager.GetActiveScene().name != sceneName) // ���͵���ͬ����
        {
            SceneFader fade = Instantiate(sceneFaderPrefab);
            yield return StartCoroutine(fade.FadeOut(2.0f));
            yield return SceneManager.LoadSceneAsync(sceneName); //�ȴ���������
            TransitionDestination destination = GetTransitionDestination(destinationTag);
            yield return Instantiate(playerPrefab, destination.transform.position,destination.transform.rotation); //����player
            SaveManager.Instance.LoadPlayerData(); //������ҵ�����
            yield return StartCoroutine(fade.FadeIn(2.0f));
            yield break; //��Э��������

        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject; //�õ�player
            agent = player.GetComponent<NavMeshAgent>();
            agent.enabled = false; //�����agent�ر�
            TransitionDestination destination = GetTransitionDestination(destinationTag);
            player.transform.SetPositionAndRotation(destination.transform.position,destination.transform.rotation);
            agent.enabled = true; //���͵�λ���ٿ���
            yield return null;
        }

    }

    //����tag���ָ��destination�������
    private TransitionDestination GetTransitionDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();

        for ( int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];
        }
        return null;
    }

    //TODO�� �򻯺ϲ���ǰ�����������Ǹ�����
    //����³����е��������
    public Transform GetEntrance()
    {
        foreach (var point in FindObjectsOfType<TransitionDestination>())
        {   //Ѱ�ҳ����е���ڵ㣬���ƥ���˾ͷ��ظõ��transform
            if (point.destinationTag == TransitionDestination.DestinationTag.ENTER)
                return point.transform;
        }
        return null;
    }
    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }
    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("Scene1"));
    }

    public void TransitionToLoadedGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    //���ص�һ������
    IEnumerator LoadLevel(string scene)
    {
        //���ɳ�FadeCanvasʵ�ֽ�������
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if(scene!="")
        {
            yield return StartCoroutine(fade.FadeOut(2.0f));
            yield return SceneManager.LoadSceneAsync(scene);//���س���
            Transform startTrans = GetEntrance();
            yield return Instantiate(playerPrefab, startTrans.position, startTrans.rotation);//��������

            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeIn(2.0f));
            yield break;

        }
    }

    //�������˵�
    IEnumerator LoadMain()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(2.0f));
        yield return SceneManager.LoadSceneAsync("MainMenu");
        yield return StartCoroutine(fade.FadeIn(2.0f));
        yield break;
    }

    public void EndNotify()
    {
        if(fadeFinish) //��֤�������˵���Э��ֻ����һ��
        {
            fadeFinish = false;
            StartCoroutine(LoadMain());

        }
    }
}
