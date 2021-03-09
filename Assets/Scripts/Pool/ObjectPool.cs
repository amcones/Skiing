using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private List<PoolElement> objectList;

    public ObjectPool(GameObject origin, int size)
    {
        objectList = new List<PoolElement>(size);
        for(int index = 0;index < size;index ++)
        {
            objectList.Add(new PoolElement(origin));
        }
    }

    public GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }

    private class PoolObjectList
    {
        private List<PoolElement> objectList;
        private List<PoolElement> objectInUse;

        public PoolObjectList(GameObject gameObject, int size)
        {

        }
    }

    private class PoolElement
    {
        private GameObject gameObject;
        private bool inUse;

        public PoolElement(GameObject origin)
        {
            gameObject = GameObject.Instantiate(origin);
            inUse = false;
            gameObject.SetActive(inUse);
        }

        public GameObject SetUseStatus(bool inUse)
        {
            this.inUse = inUse;
            gameObject.SetActive(inUse);
            return gameObject;
        }
    }
}
