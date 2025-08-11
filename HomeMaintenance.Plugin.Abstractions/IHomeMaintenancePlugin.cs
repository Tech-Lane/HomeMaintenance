using System;
using System.Threading.Tasks;

namespace HomeMaintenance.Plugin.Abstractions
{
    /// <summary>
    /// Main interface that all Home Maintenance plugins must implement
    /// </summary>
    public interface IHomeMaintenancePlugin
    {
        /// <summary>
        /// Gets the unique identifier for the plugin
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the display name of the plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the plugin
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the version of the plugin
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Gets the author of the plugin
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Gets the website URL for the plugin
        /// </summary>
        string? WebsiteUrl { get; }

        /// <summary>
        /// Initializes the plugin
        /// </summary>
        /// <param name="context">The plugin context</param>
        /// <returns>Task representing the initialization</returns>
        Task InitializeAsync(IPluginContext context);

        /// <summary>
        /// Executes the plugin's main functionality
        /// </summary>
        /// <param name="parameters">Parameters for the execution</param>
        /// <returns>The result of the execution</returns>
        Task<IPluginResult> ExecuteAsync(IPluginParameters parameters);

        /// <summary>
        /// Shuts down the plugin
        /// </summary>
        /// <returns>Task representing the shutdown</returns>
        Task ShutdownAsync();
    }
} 