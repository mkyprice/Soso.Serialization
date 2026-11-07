using System;
using System.Diagnostics;
using System.IO;
using Soso.Utils.Logging;

namespace Soso.Serialization.Logging
{
	public static class Log
	{
		public static SosoLogger Logger = new SosoLogger();
		public static void Debug(string format, params object[]? args) => Logger.Debug(format, args);
		public static void Info(string format, params object[]? args) => Logger.Info(format, args);
		public static void Warn(string format, params object[]? args) => Logger.Warn(format, args);
		public static void Error(string format, params object[]? args) => Logger.Error(format, args);
	}
}
