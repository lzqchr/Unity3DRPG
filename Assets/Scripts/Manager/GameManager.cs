using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameManager : Singleton<GameManager>
{
    //�������д������characterStats��ʱ��ͨ��gamemanager������
    public CharacterStats playerStats;

    CinemachineFreeLook followCamera;

    //����һ���б��¼����IEndGameObserver�ӿ��µ�����ʵ��
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }


    //�ù۲���ģʽ����ע��ķ�ʽ��player���ɵ�ʱ�����gamemanager
    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;
        followCamera = FindObjectOfType<CinemachineFreeLook>();

        if(followCamera!=null)
        {
            followCamera.LookAt = playerStats.transform.GetChild(2);
            followCamera.Follow = playerStats.transform.GetChild(2);
        }
    }

    //����ע��ķ�ʽ��endGameOberverȫ����ӵ��б���
    public void AddObserver(IEndGameObserver observer)
    {
        //û���ж��ظ�����Ϊֻ�е��˴���ʱ�ż�����б�һ�������ظ�
        endGameObservers.Add(observer);
    }

    //���б����Ƴ�����
    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    //ʵ�ֹ㲥
    public void NotifyObservers()
    {
        foreach(var observer in endGameObservers)
        {//�б��е����е���ʵ�嶼ִ��EndNotify();
            observer.EndNotify();
        }
    }

 

}
