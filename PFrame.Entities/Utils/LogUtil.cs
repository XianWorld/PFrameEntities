using Unity.Collections;
using Unity.Entities;
#if UNITY_DOTSPLAYER
using System;
#else
using UnityEngine;
#endif

namespace PFrame.Entities
{
    public class LogUtil
    {
#if UNITY_DOTSPLAYER
        public static void Log(string message)
        {
            Console.Write(message);
        }
        public static void LogFormat(string message, params object[] args)
        {
            string msg = string.Format(message, args);
            Console.Write(msg);
        }
#else
        public static void Log(string message)
        {
            Debug.Log(message);
        }
        public static void LogFormat(string message, params object[] args)
        {
            Debug.LogFormat(message, args);
        }
#endif
    }
}