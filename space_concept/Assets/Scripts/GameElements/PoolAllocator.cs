using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

public class PoolAllocator<T> {


    public int InitialPoolSize;
    public int PoolSizeIncrement;                   // Number of objects that should be allocated if the pool size runs out of objects. This value is multiplied by 1.5 after every allocation
    Stack<T> pool;
    public int PoolSize { get; private set; }       // The number of objects currently handled by the pool


    public delegate T TCreator();                   // delegate used to instantiate a new object
    public delegate void TReset(T obj);             // delegate used to reset game object when it is created and when it is put back into the pool

    private TCreator _tCreator;                     // keep the delegate
    private TReset _tReset;                         // keep the delegate

    public PoolAllocator(TCreator tcreator, TReset treset, int initialPoolSize = 100, int poolSizeIncrement = 50) {
        this._tCreator = tcreator;
        this._tReset = treset;

        this.InitialPoolSize = initialPoolSize;
        this.PoolSizeIncrement = poolSizeIncrement;

        this.PoolSize = 0;
        if(poolSizeIncrement == 0) {
            throw new UnityException("Invalid argument: poolSizeIncrement must be greater than zero.");
        }

        pool = new Stack<T>(InitialPoolSize);
        allocateObjects(InitialPoolSize);
    }
    


    void allocateObjects(int count)  {
        Debug.Log("Allocating " + count + " new objects for pool.");
        for(int i=0; i< count; ++i) {
            T obj = _tCreator();
            _tReset(obj);
            pool.Push(obj);
        }
        PoolSize += count;
    }

    public T Get() {
        if(pool.Count == 0) {
            Debug.LogWarning("Pool ran out of objects. Allocating " + PoolSizeIncrement + " more objects.");
            allocateObjects(PoolSizeIncrement);
            PoolSizeIncrement = (int)(PoolSizeIncrement * 1.5);
        }
        return pool.Pop();
    }

    // It is possible to put back objects that have been generated without/another pool allocator.
    // Objects must not, however, put back twice
    public void PutBack(T obj) {
        _tReset(obj);
        pool.Push(obj);
    }


    override
    public string ToString() {
        return "[Pool Allocator: " + PoolSize + " managed objects; " + pool.Count + " available objects]";
    }
}
