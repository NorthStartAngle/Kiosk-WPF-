using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace AtmCommon
{
    public enum LogTarget
    {
        File, Database, EventLog
    }

    public abstract class LogBase
    {
        protected readonly object lockObj = new object();
        public abstract void Log(string message);
    }

    public class FileLogger : LogBase
    {
        public string filePath = @"../ATMLog.txt";

        public override void Log(string message)
        {
            lock (lockObj)
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Append))
                {
                    using (StreamWriter streamWriter = new StreamWriter(stream))
                    {
                        streamWriter.WriteLine($"{DateTime.UtcNow:yyyy_M_d HH_mm_ss}:{System.Reflection.Assembly.GetExecutingAssembly()}:{message}");
                        streamWriter.Close();
                    }
                }                
            }
        }
    }

    public class EventLogger : LogBase
    {
        public override void Log(string message)
        {
            lock (lockObj)
            {
                EventLog m_EventLog = new EventLog("");
                m_EventLog.Source = "IDGEventLog";
                m_EventLog.WriteEntry(message);
            }
        }
    }

    public static class LogHelper
    {
        public static void Log(LogTarget target, string message)
        {
            LogBase logger;
            switch (target)
            {
                case LogTarget.File:
                    logger = new FileLogger();
                    logger.Log(message);
                    break;
                case LogTarget.EventLog:
                    logger = new EventLogger();
                    logger.Log(message);
                    break;
                default:
                    return;
            }
        }
    }
}
