using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab; // �������ɵ�Prefab
    public Transform barPoint; // Ѫ�����ɵ�λ��
    public bool isAlwaysVisible; //�Ƿ����ÿɼ�

    public float visibleTime; // ��������ʾʱ��
    float timeLeft; //ʣ�����ʾ�¼�

    Image healthSlider;
    Transform UIbar;
    Transform cam; // ϣ��Ѫ����Զ�泯�����

    CharacterStats currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();

        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable() //��gameObject����ʱ����
    {
        //����ʱ����ȡ�������λ��
        cam = Camera.main.transform;

        foreach(Canvas canvas in FindObjectsOfType<Canvas>()) //Ѱ������Canvas���
        {
            if(canvas.renderMode == RenderMode.WorldSpace) 
                //���canvas�齨����Ⱦģʽ�����緶Χ�ģ�Ҳ���������ֵķ�ʽȥ����,�����Ϸֻ��һ��canvas������Ϊ��������ģ����Կ���ֱ��ʹ�����ַ�ʽ
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;//��Ѫ��Ԥ����������ָ����canvas��λ�ã����������
                healthSlider = UIbar.GetChild(0).GetComponent<Image>(); //��û�����Image
                UIbar.gameObject.SetActive(isAlwaysVisible); //�����Ƿ���һֱ��ʾ����UIbar
            }

        }
    }

    private void UpdateHealthBar(int curHeath, int maxHealth)
    {
        if(curHeath <= 0)
             Destroy(UIbar.gameObject);
          

        UIbar.gameObject.SetActive(true); //gameObject���Դ��ģ�
        timeLeft = visibleTime;
        //���ĿǰѪ���������Ѫ���İٷֱ�
        float slidPersent = (float)curHeath / maxHealth;
        healthSlider.fillAmount = slidPersent;
    }

    //��֡��Ⱦ֮���ִ��
    private void LateUpdate()
    {
        if(UIbar!=null)
        {
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;
            if (timeLeft <= 0 && !isAlwaysVisible)
            {
                UIbar.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }

        
    }

}
