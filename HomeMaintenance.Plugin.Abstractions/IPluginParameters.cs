using System.Collections.Generic;

namespace HomeMaintenance.Plugin.Abstractions
{
    /// <summary>
    /// Interface for parameters passed to plugins during execution
    /// </summary>
    public interface IPluginParameters
    {
        /// <summary>
        /// Gets the command or action to execute
        /// </summary>
        string Command { get; }

        /// <summary>
        /// Gets the parameters as a dictionary
        /// </summary>
        IReadOnlyDictionary<string, object> Parameters { get; }

        /// <summary>
        /// Gets a parameter value by key
        /// </summary>
        /// <param name="key">The parameter key</param>
        /// <returns>The parameter value if found, null otherwise</returns>
        object? GetParameter(string key);

        /// <summary>
        /// Gets a parameter value by key with type conversion
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="key">The parameter key</param>
        /// <returns>The parameter value if found and convertible, default(T) otherwise</returns>
        T? GetParameter<T>(string key);

        /// <summary>
        /// Gets a parameter value by key with a default value
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="key">The parameter key</param>
        /// <param name="defaultValue">The default value if parameter is not found</param>
        /// <returns>The parameter value if found and convertible, defaultValue otherwise</returns>
        T GetParameter<T>(string key, T defaultValue);
    }
} 