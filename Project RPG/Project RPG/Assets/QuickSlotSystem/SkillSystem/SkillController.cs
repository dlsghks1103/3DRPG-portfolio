using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CharacterControl;
using RPG.PlyerController;

public class SkillController : MonoBehaviour
{
    public Combat Combat;
    public GameObject effectPrefabs;
    public Transform effectPoint;

    public float coolTime = 0;
    public Image coolTimeImage;

    public int useMana = 0;

    public int animationIndex = 0;

    public bool IsCoolTime = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)&& IsCoolTime)
        {
            UseSkill();
        }
    }

    public void UseSkill()
    {
        float skillCoolTime = coolTime;

        if(IsCoolTime)
        {
            Combat.SkillAttack(useMana);
            if (effectPrefabs)
            {
                Instantiate<GameObject>(effectPrefabs, effectPoint);
            }

            StartCoroutine(SkillCoolTime(skillCoolTime));
        }
    }

    IEnumerator SkillCoolTime(float skillCoolTime)
    {
        IsCoolTime = false;

        while (skillCoolTime > 1.0f)
        {
            skillCoolTime -= Time.deltaTime;
            coolTimeImage.fillAmount = (1.0f / skillCoolTime);
            yield return new WaitForFixedUpdate();
        }
        IsCoolTime = true;
    }
}
