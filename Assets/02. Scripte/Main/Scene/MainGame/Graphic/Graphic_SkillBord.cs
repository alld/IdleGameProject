using DG.Tweening;
using IdleGame.Core.Procedure;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Numeric;
using IdleGame.Main.GameLogic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace IdleGame.Main.Scene.Main.UI
{
    public class Graphic_SkillBord : MonoBehaviour
    {
        [SerializeField]
        private Image[] I_skills;
        private bool[] skillLock = new bool[5];

        [SerializeField]
        private TMP_Text t_auto;

        [SerializeField]
        private Color activeColor;

        private Coroutine co_autoSkill;

        private void Start()
        {
            Base_Engine.Event.RegisterEvent(eGlobalEventType.On_LevelUp, UnLockSkill);

            UnLockSkill();
        }

        public void UnLockSkill()
        {
            if (Global_Data.Player.level >= 2)
            {
                I_skills[0].color = Color.gray;
                skillLock[0] = false;
            }
            else
            {
                I_skills[0].color = Color.white;
                skillLock[0] = true;
            }

            if (Global_Data.Player.level >= 5)
            {
                I_skills[1].color = Color.gray;
                skillLock[1] = false;
            }
            else
            {
                I_skills[1].color = Color.white;
                skillLock[1] = true;
            }

            if (Global_Data.Player.level >= 7)
            {
                I_skills[2].color = Color.gray;
                skillLock[2] = false;
            }
            else
            {
                I_skills[2].color = Color.white;
                skillLock[2] = true;
            }

            if (Global_Data.Player.level >= 10)
            {
                I_skills[3].color = Color.gray;
                skillLock[3] = false;
            }
            else
            {
                I_skills[3].color = Color.white;
                skillLock[3] = true;
            }

            if (Global_Data.Player.level >= 15)
            {
                I_skills[4].color = Color.gray;
                skillLock[4] = false;
            }
            else
            {
                I_skills[4].color = Color.white;
                skillLock[4] = true;
            }
        }

        public void TryActiveSkill(int index)
        {
            if (skillLock[index]) return;

            skillLock[index] = true;

            I_skills[index].DOFillAmount(0, 60)
                .OnComplete(() =>
                {
                    skillLock[index] = false;
                }).SetEase(Ease.Linear);

            switch (index)
            {
                case 0:
                    if (Panel_StageManager.Unit_Monsters.Count != 0)
                    {
                        Panel_StageManager.Unit_Monsters[0].Logic_Act_Damaged(Panel_StageManager.Unit_Player, new ExactInt(500));
                    }
                    break;
                case 1:
                    if (Panel_StageManager.Unit_Monsters.Count != 0)
                    {
                        Panel_StageManager.Unit_Monsters[0].Logic_Act_Damaged(Panel_StageManager.Unit_Player, new ExactInt(500));
                    }
                    break;
                case 2:
                    if (Panel_StageManager.Unit_Monsters.Count != 0)
                    {
                        Panel_StageManager.Unit_Monsters[0].Logic_Act_Damaged(Panel_StageManager.Unit_Player, new ExactInt(500));
                    }
                    break;
                case 3:
                    if (Panel_StageManager.Unit_Monsters.Count != 0)
                    {
                        Panel_StageManager.Unit_Monsters[0].Logic_Act_Damaged(Panel_StageManager.Unit_Player, new ExactInt(500));
                    }
                    break;
                case 4:
                    if (Panel_StageManager.Unit_Monsters.Count != 0)
                    {
                        Panel_StageManager.Unit_Monsters[0].Logic_Act_Damaged(Panel_StageManager.Unit_Player, new ExactInt(500));
                    }
                    break;
            }
        }

        private IEnumerator AutoSkill()
        {
            while (true)
            {
                for (int i = 0; i < skillLock.Length; i++)
                {
                    if (!skillLock[i])
                    {
                        TryActiveSkill(i);
                        yield return null;
                    }
                }
                yield return null;
            }
        }


        public void OnClickSkill(int index)
        {
            TryActiveSkill(index);
        }

        public void OnClickAuto()
        {
            if (co_autoSkill == null)
            {
                t_auto.color = activeColor;
                co_autoSkill = StartCoroutine(AutoSkill());
            }
            else
            {
                t_auto.color = Color.black;
                StopCoroutine(co_autoSkill);
                co_autoSkill = null;
            }
        }
    }
}