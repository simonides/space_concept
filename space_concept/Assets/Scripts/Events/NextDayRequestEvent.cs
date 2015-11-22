//using UnityEngine;
//using System.Collections;
//using TinyMessenger;
//using System;


//// The user wants the next day to be started (DOES NOT START A NEW DAY)
//// This event needs to be approved by the GameState object (eg. to avoid multiple button clicks)


//public class NextDayRequestEvent : ITinyMessage {

//    public object Sender { get; private set; }

//    private NextDayRequestEvent() {
//        Reset();
//    }

//    private void Reset() {
//        Sender = null;
//    }

//    private void Init(object sender) {
//        Sender = sender;
//    }


//    static PoolAllocator<NextDayRequestEvent> poolAllocator;
//    private static void InitPoolAllocator() {
//        poolAllocator = new PoolAllocator<NextDayRequestEvent>(
//                () => { return new NextDayRequestEvent(); }
//        );
//    }

//    public static NextDayRequestEvent New(object sender) {
//        if (poolAllocator == null) {
//            InitPoolAllocator();
//        }
//        NextDayRequestEvent obj = poolAllocator.Get();
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
using System;


// The user wants the next day to be started (DOES NOT START A NEW DAY)
// This event needs to be approved by the GameState object (eg. to avoid multiple button clicks)


public class NextDayRequestEvent : TinyMessageBase {

    public NextDayRequestEvent(object sender)
        : base(sender) {

    }

}