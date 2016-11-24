using UnityEngine;
using System.Collections;

/* General Logger
 * Allows for easier error and log implementation
 * You can either use the general Logger, or make a logger for a specific part.
 * Local Loggers can override the level if there is a need for more specific log level
 * 
 * 
 */
public class Log
{
	public string name = "UNDEFINED";
	public static LogLevel globalLogLevel = LogLevel.Execution;
	private static Log globalLog = new Log("GENERAL");
	public LogLevel localLogLevel = LogLevel.Execution;
	public bool overrideGlobalLogLevel = false;

	public Log()
	{}

	public Log(string name)
	{
		this.name = name;
	}

	public Log(string name, LogLevel logLevel)
	{
		overrideGlobalLogLevel = true;
		this.name = name;
		localLogLevel = logLevel;
	}

	public LogLevel GetLogLevel()
	{
		return (overrideGlobalLogLevel ? localLogLevel : globalLogLevel);
	}

	public void Debug(string message)
	{
		if(LogLevelRequired(LogLevel.Debug, GetLogLevel()))
			UnityEngine.Debug.Log ("##" + name + "## " + message);
	}

	public void Exec(string message)
	{
		if(LogLevelRequired(LogLevel.Execution, GetLogLevel()))
			UnityEngine.Debug.Log("[" + name + "] " + message);
	}

	public void Prob(string message)
	{
		if(LogLevelRequired(LogLevel.Problems, GetLogLevel()))
			UnityEngine.Debug.LogWarning("[" + name + "] " + message);
	}

	public void Err(string message)
	{
			UnityEngine.Debug.LogError("[" + name + "] " + message);
	}

	public static void GDebug(string message)
	{
		globalLog.Debug (message);
	}

	public static void GExec(string message)
	{
		globalLog.Exec (message);
	}

	public static void GProb(string message)
	{
		globalLog.Prob (message);
	}

	public static void GErr(string message)
	{
		globalLog.Err (message);
	}

	public static bool LogLevelRequired(LogLevel logRequired, LogLevel logLocal)
	{
		return (int)logLocal <= (int)logRequired;
	}
}


public enum LogLevel
{
	Debug, Execution, Problems, Errors
}