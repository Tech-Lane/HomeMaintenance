using System;
using System.Collections.Generic;

namespace HomeMaintenance.Plugin.Abstractions
{
    /// <summary>
    /// Interface for results returned from plugin execution
    /// </summary>
    public interface IPluginResult
    {
        /// <summary>
        /// Gets whether the plugin execution was successful
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Gets the error message if execution failed
        /// </summary>
        string? ErrorMessage { get; }

        /// <summary>
        /// Gets the exception if execution failed
        /// </summary>
        Exception? Exception { get; }

        /// <summary>
        /// Gets the execution time in milliseconds
        /// </summary>
        long ExecutionTimeMs { get; }

        /// <summary>
        /// Gets the result data as a dictionary
        /// </summary>
        IReadOnlyDictionary<string, object> Data { get; }

        /// <summary>
        /// Gets a data value by key
        /// </summary>
        /// <param name="key">The data key</param>
        /// <returns>The data value if found, null otherwise</returns>
        object? GetData(string key);

        /// <summary>
        /// Gets a data value by key with type conversion
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="key">The data key</param>
        /// <returns>The data value if found and convertible, default(T) otherwise</returns>
        T? GetData<T>(string key);

        /// <summary>
        /// Gets a data value by key with a default value
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="key">The data key</param>
        /// <param name="defaultValue">The default value if data is not found</param>
        /// <returns>The data value if found and convertible, defaultValue otherwise</returns>
        T GetData<T>(string key, T defaultValue);
    }
} 