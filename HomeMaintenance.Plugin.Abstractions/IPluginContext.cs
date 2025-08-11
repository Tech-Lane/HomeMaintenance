using System;
using System.Collections.Generic;

namespace HomeMaintenance.Plugin.Abstractions
{
    /// <summary>
    /// Context interface that provides services and data to plugins
    /// </summary>
    public interface IPluginContext
    {
        /// <summary>
        /// Gets the application version
        /// </summary>
        Version ApplicationVersion { get; }

        /// <summary>
        /// Gets the plugin's configuration settings
        /// </summary>
        IReadOnlyDictionary<string, object> Configuration { get; }

        /// <summary>
        /// Gets the plugin's data directory
        /// </summary>
        string DataDirectory { get; }

        /// <summary>
        /// Gets the plugin's log directory
        /// </summary>
        string LogDirectory { get; }

        /// <summary>
        /// Gets a service of the specified type
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>The service instance if available, null otherwise</returns>
        T? GetService<T>() where T : class;

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The message to log</param>
        void Log(LogLevel level, string message);

        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log</param>
        void Log(LogLevel level, string message, Exception exception);
    }

    /// <summary>
    /// Log levels for plugin logging
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Debug level logging
        /// </summary>
        Debug,

        /// <summary>
        /// Information level logging
        /// </summary>
        Information,

        /// <summary>
        /// Warning level logging
        /// </summary>
        Warning,

        /// <summary>
        /// Error level logging
        /// </summary>
        Error,

        /// <summary>
        /// Critical level logging
        /// </summary>
        Critical
    }
} 