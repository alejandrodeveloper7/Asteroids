using System;
using UnityEngine;

public class SimplePool : object
{
    //This Script and the other three related are my Pool tool. They allow any Gameobject be pooled with a very quick 
    //implementation and have 2 very recurrent Scripts already implemented to pool particle systems

    #region Variables

    [Header("References")]
    private GameObject objectPooled;
    private GameObject[] availableInstances;
    private int scalation;
    private Transform parent;


    [Header("Properties")]
    private int poolMaxSize;    public int PoolMaxSize
    {
        get => poolMaxSize;
        private set => poolMaxSize = value;
    }
    private int poolCurrentSize;    public int PoolCurrentSize
    {
        get => poolCurrentSize;
        private set => poolCurrentSize = value;
    }

    #endregion

    #region Constuctors

    public SimplePool(GameObject objectCopied, Transform parent)
        : this(objectCopied, parent, 0, 1, int.MaxValue)
    {
    }

    public SimplePool(GameObject objectCopied, Transform parent, int initialSize)
        : this(objectCopied, parent, initialSize, 1, int.MaxValue)
    {
    }

    public SimplePool(GameObject objectCopied, Transform parent, int initialSize, int scalation)
        : this(objectCopied, parent, initialSize, scalation, int.MaxValue)
    {
    }

    public SimplePool(GameObject objectPooled, Transform parent, int initialSize, int scalation, int poolMaxSize)
    {
        if (scalation <= 0)
        {
            Debug.LogError("The pool need a scalation of atleast 1");
            return;
        }
        if (poolMaxSize <= 0)
        {
            Debug.LogError("The pool need a maximun Size of atleast 1");
            return;
        }
        this.objectPooled = objectPooled;
        this.scalation = scalation;
        PoolMaxSize = poolMaxSize;
        this.parent = parent;
        availableInstances = new GameObject[0];
        poolCurrentSize = 0;

        if (initialSize > 0)
        {
            ExpandPoolSize(initialSize);
        }
    }

    #endregion       

    #region public Methods

    public GameObject GetInstance()
    {
        GameObject instance = ObteinReadyInstance();

        if (instance != null)
        {
            return instance;
        }
        else
        {
            if (scalation == 1 && availableInstances.Length < PoolMaxSize)
            {
                instance = CreateNewInstance();
                Array.Resize(ref availableInstances, availableInstances.Length + 1);
                availableInstances[availableInstances.Length - 1] = instance;
                return instance;
            }
            else if (availableInstances.Length < PoolMaxSize)
            {
                ExpandPoolSize(scalation);
                instance = ObteinReadyInstance();
            }

            if (instance == null)
            {
                Debug.LogError("Pool is at max size and there are not ready instances available");
            }
            return instance;
        }

    }

    #endregion

    #region Internal Logic

    private GameObject ObteinReadyInstance()
    {
        for (int i = 0; i < availableInstances.Length; i++)
        {
            if (availableInstances[i].GetComponent<IPooleableItem>().ReadyToUse)
            {
                return availableInstances[i];
            }
        }
        return null;
    }

    internal GameObject CreateNewInstance()
    {
        GameObject newInstance = UnityEngine.Object.Instantiate(objectPooled, parent);
        newInstance.GetComponent<IPooleableItem>().ReadyToUse = true;
        if (parent != null)
        {
            newInstance.transform.localPosition = Vector3.zero;
            newInstance.transform.localRotation = Quaternion.identity;
        }
        return newInstance;
    }

    internal void ExpandPoolSize(int count)
    {
        int allocationCount = PoolMaxSize - availableInstances.Length;

        if (allocationCount == 0)
        {
            Debug.LogError("Pool at max size, can´t be expanded more");
            return;
        }

        if (count < allocationCount)
        {
            allocationCount = count;
            Debug.Log( objectPooled.name + "'s pool expanded in "+ allocationCount + " units");
        }
        else
        {
            Debug.LogWarning(objectPooled.name + "'s pool expanded in " + allocationCount + " units. POOL AT MAX SIZE");
        }

        int firstPosition = availableInstances.Length;
        Array.Resize(ref availableInstances, firstPosition + allocationCount);

        for (int i = firstPosition; i < availableInstances.Length; i++)
        {
            availableInstances[i] = CreateNewInstance();
        }
        PoolCurrentSize = availableInstances.Length;
    }

    #endregion
}
