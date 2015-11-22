//using UnityEngine;
//using System.Collections;
//using TinyMessenger;

//public class SimpleEventBase<T> : ITinyMessage where T : new() {

//    public object Sender { get; private set; }


//    private SimpleEventBase() {
//        Reset();
//    }

//    virtual protected void Reset() {
//        Sender = null;
//    }

//    virtual protected void Init(object sender) {
//        Sender = sender;
//    }


//    static PoolAllocator<T> poolAllocator;
//    private static void InitPoolAllocator() {
//        poolAllocator = new PoolAllocator<T>(
//                () => { return new T(); }
//        );
//    }

//    public static T New(object sender) {
//        if (poolAllocator == null) {
//            InitPoolAllocator();
//        }
//        T obj = poolAllocator.Get();
//        obj.Init(sender);
//        return obj;
//    }

//    public void Completed() {
//        Reset();
//        poolAllocator.PutBack(this);
//    }
//}
