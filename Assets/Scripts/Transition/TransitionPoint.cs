using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {  //ö��ֻ������������ͬ�����Ͳ�ͬ����
        SameScene, DifferentScene
    }
    [Header("Transition Info")]
    public string sceneName; //���ô��͵ĳ���
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;

    private bool canTrans;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canTrans) //�������E�����ҵ�ǰ״̬Ϊ���Դ���
        {
            SceneController.Instance.TransitionToDestination(this);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canTrans = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canTrans = false;
        }
    }
}
