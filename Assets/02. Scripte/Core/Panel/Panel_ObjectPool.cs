using IdleGame.Core;
using UnityEngine;

namespace IdleGame.Temp
{

    public class Panel_ObjectPool : Base_ManagerPanel
    {
        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private Transform pool;

        protected override void Logic_Init_Custom()
        {
            AutoSize();
        }


        public void AutoSize()
        {
            if (pool.childCount != 0)
                return;


            for (int i = 0; i < 20; i++)
            {
                Instantiate(prefab, pool);
            }
        }

        public GameObject GetObject()
        {
            AutoSize();

            return pool.GetChild(0).gameObject;
        }


        public void ReturnObject(GameObject m_obj)
        {
            m_obj.transform.SetParent(pool);
            m_obj.SetActive(false);
        }
    }
}