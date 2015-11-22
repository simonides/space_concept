//using UnityEngine;
//using System.Collections;
//using TinyMessenger;

//// Starts a new day

//public class NextDayEvent : ITinyMessage {


//    public object Sender { get; private set; }


//    private NextDayEvent() {
//        Reset();
//    }

//    private void Reset() {
//        Sender = null;
//    }

//    private void Init(object sender) {
//        Sender = sender;
//    }


//    static PoolAllocator<NextDayEvent> poolAllocator;
//    private static void InitPoolAllocator() {
//        poolAllocator = new PoolAllocator<NextDayEvent>(
//                () => { return new NextDayEvent(); }
//        );
//    }

//    public static NextDayEvent New(object sender) {
//        if (poolAllocator == null) {
//            InitPoolAllocator();
//        }
//        NextDayEvent obj = poolAllocator.Get();
//        obj.Init(sender);
//        return obj;
//    }

//    public void Completed() {
//        Reset();
//        poolAllocator.PutBack(this);
//    }

//}

using UnityEngine;
using System.Collections;
using TinyMessenger;

// Starts a new day

public class NextDayEvent : TinyMessageBase {

    public NextDayEvent(object sender)
        : base(sender)
    {

    }
}
