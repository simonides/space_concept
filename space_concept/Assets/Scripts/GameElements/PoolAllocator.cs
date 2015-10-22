using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolAllocator<T> : MonoBehaviour where T : Poolable, new() {

    public int InitialPoolSize;
    public int PoolSizeIncrement;                   // Number of objects that should be allocated if the pool size runs out of objects. This value is multiplied by 1.5 after every allocation
    Stack<T> pool;

    public int PoolSize { get; private set; }       // The number of objects currently handled by the pool

    public PoolAllocator(int initialPoolSize = 100, int poolSizeIncrement = 50) {
        this.InitialPoolSize = initialPoolSize;
        this.PoolSizeIncrement = poolSizeIncrement;
        this.PoolSize = 0;
        if(poolSizeIncrement == 0) {
            throw new UnityException("Invalid argument: poolSizeIncrement must be greater than zero.");
        }
    }

    // Use this for initialization
    void Awake () {
        PoolSize = 0;
        pool = new Stack<T>(InitialPoolSize);
        allocateObjects(InitialPoolSize);
	}


    void allocateObjects(int count)  {
        Debug.Log("Allocating " + count + " new objects for pool.");
        for(int i=0; i< count; ++i) {
            pool.Push(new T());
        }
        PoolSize += count;
    }

    public T Get() {
        T obj = pool.Pop();
        if(obj == null) {
            Debug.LogWarning("Pool ran out of objects. Allocating " + PoolSizeIncrement + " more objects.");
            allocateObjects(PoolSizeIncrement);
            PoolSizeIncrement = (int)(PoolSizeIncrement * 1.5);
            obj = pool.Pop();
        }
        obj.Reset();
        return obj;
    }

    public void PutBack(T obj) {
        pool.Push(obj);
    }


    override
    public string ToString() {
        return "[Pool Allocator: " + PoolSize + " managed objects; " + pool.Count + " available objects]";
    }
}
