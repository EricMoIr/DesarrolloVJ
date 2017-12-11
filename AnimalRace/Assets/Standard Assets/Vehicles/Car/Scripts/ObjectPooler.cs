using Boo.Lang;
using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    [AddComponentMenu("Gameplay/ObjectPooler")]
    class ObjectPooler : MonoBehaviour
    {
        private static ObjectPooler instance;
        [Serializable]
        private class ObjectPool
        {
            [SerializeField]
            private GameObject gameObject;
            [SerializeField]
            private int amount;
            internal GameObject GameObject { get { return gameObject; } }
            internal int Amount { get { return amount; } set { amount = value; } }
            private List<GameObject> pool = new List<GameObject>();
            internal List<GameObject> Pool { get { return pool; } set { pool = value; } }
        }
        [SerializeField]
        private ObjectPool[] objectPools;

        public static ObjectPooler GetInstance()
        {
            if (instance == null)
                throw new NullReferenceException("The object pooler was not active in the scene");
            return instance;
        }


        void Start()
        {
            for (var i = 0; i < objectPools.Length; i++)
            {
                var objectPool = objectPools[i];
                if (objectPool.GameObject == null)
                {
                    print("The element " + i + " of the pooler was not added");
                    continue;
                }
                for (var j = 0; j < objectPool.Amount; j++)
                {
                    GameObject objectInstance = Instantiate(objectPool.GameObject);
                    objectInstance.SetActive(false);
                    objectInstance.name = objectPool.GameObject.name;
                    objectInstance.transform.SetParent(gameObject.transform, false);
                    objectPool.Pool.Add(objectInstance);
                }
            }
            instance = this;
        }

        public GameObject GetInactiveObjectOfType(string objectName)
        {
            for (int i = 0; i < objectPools.Length; i++)
            {
                if (objectPools[i].GameObject.name != objectName)
                    continue;
                for (int j = 0; j < objectPools[i].Amount; j++)
                {
                    if (!objectPools[i].Pool[j].activeSelf)
                    {
                        return objectPools[i].Pool[j];
                    }
                }
                GameObject objectInstance = Instantiate(objectPools[i].GameObject);
                objectInstance.transform.SetParent(gameObject.transform, false);
                objectPools[i].Amount++;
                objectPools[i].Pool.Add(objectInstance);
                return objectInstance;
            }
            throw new NullReferenceException("Couldn't find the Prefab '" + objectName + "' in the pooler");
        }
    }
}
