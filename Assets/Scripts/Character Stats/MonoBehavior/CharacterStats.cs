using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public event Action<int, int> UpdateHealthBarOnAttack;

    public CharacterData_SO templateData;

    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;
    [HideInInspector]
    public bool isDefend;

    private void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
    }

    #region Read from Data_SO
    public int MaxHealth 
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0;}
        set { characterData.maxHealth = value; }
        //Ҳ������ôд
        //get => characterData = null ? 0 : characterData.maxHealth;
        //set => characterData.maxHealth = vcalue;
    } //�ɶ�����д

    public int CurrentHealth
    {
        get => characterData == null ? 0 : characterData.currentHealth;
        set => characterData.currentHealth = value;
    }

    public int BaseDefence
    {
        get => characterData == null ? 0 : characterData.baseDefence;
        set => characterData.baseDefence  = value;
    }

    public int CurrentDefence
    {
        get => characterData == null ? 0 : characterData.currentDefence;
        set => characterData.currentDefence = value;
    }

    #endregion

    #region Read from AttackData_SO
    public float AttackRange
    {
        get => attackData == null ? 0 : attackData.attackRange;
        set => attackData.attackRange = value;
    }
    public float SkillRange
    {
        get => attackData == null ? 0 : attackData.skillRange;
        set => attackData.skillRange = value;
    }
    public float CoolDown
    {
        get => attackData == null ? 0 : attackData.coolDown;
        set => attackData.coolDown = value;
    }
    public int MinDamage
    {
        get => attackData == null ? 0 : attackData.maxDamage;
        set => attackData.minDamage = value;
    }
    public int MaxDamage
    {
        get => attackData == null ? 0 : attackData.maxDamage;
        set => attackData.maxDamage = value;
    }
    public float CriticalMultiplier
    {
        get => attackData == null ? 0 : attackData.criticalMultiplier;
        set => attackData.criticalMultiplier = value;
    }
    public float CriticalChance
    {
        get => attackData == null ? 0 : attackData.criticalChance;
        set => attackData.criticalChance = value;
    }
    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStats attacker,CharacterStats defender)
    {
        if (!defender.isDefend)
        {
            int damage = Mathf.Max(0, attacker.CurrentDamage() - defender.CurrentDefence);
            //Ҫ���ǵ������ȹ�����Ҫ�����������ɼ�Ѫ
            defender.CurrentHealth = Mathf.Max(0, defender.CurrentHealth - damage);

            if (attacker.isCritical) //�����������ط��������˶���,�����Ͳ�����ÿ��Controller���涼������
                defender.GetComponent<Animator>().SetTrigger("Hit");


            defender.UpdateHealthBarOnAttack?.Invoke(defender.CurrentHealth, defender.MaxHealth); //����Ѫ��

            if (defender.CurrentHealth <= 0)
            {
                //���ܹ������˵�killpoint�ӵ������ߵ�data��
                attacker.characterData.UpdateExp(defender.characterData.killPoint);
            }
        }
        else //�ܷ�
        {
            int damage = Mathf.Max(0, attacker.CurrentDamage() - attacker.CurrentDefence);
            damage = (int)(damage * 0.5f); //���ذٷ�֮��ʮ���˺�

            //�������ܵ��˺�
            attacker.CurrentHealth = Mathf.Max(0, attacker.CurrentHealth - damage);

            attacker.GetComponent<Animator>().SetTrigger("Hit");


            attacker.UpdateHealthBarOnAttack?.Invoke(attacker.CurrentHealth, attacker.MaxHealth); //����Ѫ��

            if (attacker.CurrentHealth <= 0)
            {
                defender.characterData.UpdateExp(attacker.characterData.killPoint);
            }
        }
    }

    //�Է�����������ʵ��ʯͷ����
    public void TakeDamage(int damage,CharacterStats defender)
    {
        //Ҫ���ǵ������ȹ�����Ҫ�����������ɼ�Ѫ
        int curDamage = Mathf.Max(0, damage - defender.CurrentDefence);
        defender.CurrentHealth = Mathf.Max(0, defender.CurrentHealth - curDamage);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth); //����Ѫ��

        if (CurrentHealth <= 0)
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killPoint);
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        coreDamage *= (1 + 0.2f * characterData.currentLevel); //����һ������������20%

        coreDamage = isCritical? coreDamage * CriticalMultiplier:coreDamage;

       // Debug.Log("�˺�" + coreDamage);

        return (int)coreDamage;
    }

    #endregion
}
