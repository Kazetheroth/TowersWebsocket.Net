using System;
using System.Collections.Generic;

namespace TowersWebsocketNet31.Server
{
    public class Message
    {
        public string _TARGET { get; set; }
        public string _ROOMID { get; set; }
        public string _SENDER { get; set; }
        public string GRID { get; set; }
        public string _CLASS { get; set; }
        public string _METHOD { get; set; }
        public ArgumentMessage[] _ARGS { get; set; }
    }

    public class ArgumentMessage
    {
        public string tokenPlayer { get; set; }
        public string tokenTarget { get; set; }
        public string room { get; set; }
    }

    public class Callbacks
    {
        public List<string> callbacks;

        public Callbacks(List<string> callbacks)
        {
            this.callbacks = callbacks;
        }
    }

    [Serializable]
    public class CallbackMessages
    {
        public CallbackMessage callbackMessages { get; set; }
    }
    
    [Serializable]
    public class CallbackMessage
    {
        public string message { get; set; } = null;
        public CallbackIdentity identity { get; set; } = null;
        public string room { get; set; } = null;
        public int[] maps { get; set; } = null;
        public int roleTimer { get; set; } = -1;
        public int defenseTimer { get; set; } = -1;
        public int attackTimer { get; set; } = -1;

    }
    
    [Serializable]
    public class CallbackIdentity
    {
        public int classes { get; set; } = -1;
        public int weapon { get; set; } = -1;
    }
    
    [Serializable]
    public class CallbackChatMessages
    {
        public CallbackChatMessage callbackChatMessage { get; set; }
    }
    
    [Serializable]
    public class CallbackChatMessage
    {
        public string sender { get; set; } = null;
        public string message { get; set; } = null;
    }
    
}