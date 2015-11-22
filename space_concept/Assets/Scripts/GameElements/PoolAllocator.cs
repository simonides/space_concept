﻿using UnityEngine;
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
    private TCreator _tCreator;                     // keep the delegate

    public PoolAllocator(TCreator tcreator, int initialPoolSize = 100, int poolSizeIncrement = 50) {
        this._tCreator = tcreator;
        this.InitialPoolSize = initialPoolSize;
        this.PoolSizeIncrement = poolSizeIncrement;
        this.PoolSize = 0;
        if(poolSizeIncrement == 0) {  throw new UnityException("Invalid argument: poolSizeIncrement must be greater than zero."); }
        pool = new Stack<T>(InitialPoolSize);
        allocateObjects(InitialPoolSize);
    }
    


    void allocateObjects(int count)  {
        Debug.Log("Allocating " + count + " new objects for pool.");
        for(int i=0; i< count; ++i) {
            pool.Push(_tCreator());
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

    public void PutBack(T obj) {
        pool.Push(obj);
    }


    override
    public string ToString() {
        return "[Pool Allocator: " + PoolSize + " managed objects; " + pool.Count + " available objects]";
    }
}
