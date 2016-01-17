using System;
using UnityEngine;
using TinyMessenger;

// I use the following implementation:
// https://bitbucket.org/grumpydev/tinyioc/wiki/TinyMessenger


public class MessageHub : MonoBehaviour {
    private const string messageHubGameObjectName = "MessageHub";

    private TinyMessengerHub hub;
    private static MessageHub instance_ = null;
    private static bool applicationIsShuttingDown = false;

    private static MessageHub instance {
        get {
            if (instance_ == null && !applicationIsShuttingDown) {
                var obj = GameObject.Find(messageHubGameObjectName);
                if (obj == null)
                    obj = new GameObject(messageHubGameObjectName);

                instance_ = obj.GetComponent<MessageHub>();
                if (instance_ == null)
                    instance_ = obj.AddComponent<MessageHub>();

                instance_.hub = new TinyMessengerHub();
                DontDestroyOnLoad(obj);
            }

            return instance_;
        }
    }

    public static TinyMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> handler)
        where TMessage : class, ITinyMessage {
        return instance.hub.Subscribe(handler);
    }

    public static TinyMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> handler, Func<TMessage, bool> messageFilter)
    where TMessage : class, ITinyMessage {
        return instance.hub.Subscribe(handler, messageFilter);
    }
   
    public static void Publish<TMessage>(TMessage message)
        where TMessage : class, ITinyMessage {
        instance.hub.Publish(message);
    }

    public static void Unsubscribe<TMessage>(TinyMessageSubscriptionToken token)
        where TMessage : class, ITinyMessage {
        instance.hub.Unsubscribe<TMessage>(token);
    }

    private void OnApplicationQuit() {
        UnityEngine.Object.DestroyImmediate(this.gameObject);
        applicationIsShuttingDown = true;
    }

}