using Dalamud.Plugin;
using Dalamud.Plugin.Ipc.Exceptions;
using System;

namespace NotificationMasterAPI
{
    public class NotificationMasterApi
    {
        DalamudPluginInterface PluginInterface;

        /// <summary>
        /// Creates an instance of NotificationMaster API. You do not need to check if NotificationMaster plugin is installed.
        /// </summary>
        /// <param name="dalamudPluginInterface">Plugin interface reference</param>
        public NotificationMasterApi(DalamudPluginInterface dalamudPluginInterface)
        {
            PluginInterface = dalamudPluginInterface;
        }

        void Validate()
        {
            if (PluginInterface == null) throw new NullReferenceException("NotificationMaster API was called before it was initialized");
        }

        /// <summary>
        /// Checks if IPC is ready. You DO NOT need to call this method before invoking any of API functions unless you specifically want to check if plugin is installed and ready to accept requests.
        /// </summary>
        /// <returns></returns>
        public bool IsIPCReady()
        {
            Validate();
            try
            {
                PluginInterface.GetIpcSubscriber<object>(NMAPINames.Active).InvokeAction();
                return true;
            }
            catch (IpcNotReadyError)
            {
            }
            return false;
        }

        /// <summary>
        /// Displays tray notification. This function does not throws an exception or displays an error if NotificationMaster is not installed.
        /// </summary>
        /// <param name="text">Text of tray notification</param>
        /// <returns>Whether operation succeed.</returns>
        public bool DisplayTrayNotification(string text) => DisplayTrayNotification(null, text);

        /// <summary>
        /// Displays tray notification. This function does not throws an exception or displays an error if NotificationMaster is not installed.
        /// </summary>
        /// <param name="title">Title of tray notification</param>
        /// <param name="text">Text of tray notification</param>
        /// <returns>Whether operation succeed.</returns>
        public bool DisplayTrayNotification(string? title, string text)
        {
            Validate();
            try
            {
                return PluginInterface.GetIpcSubscriber<string, string, bool>(NMAPINames.DisplayToastNotification).InvokeFunc(title, text);
            }
            catch (IpcNotReadyError)
            {
            }
            return false;
        }

        /// <summary>
        /// Flashes game's taskbar icon. This function does not throws an exception or displays an error if NotificationMaster is not installed.
        /// </summary>
        /// <returns>Whether operation succeeded</returns>
        public bool FlashTaskbarIcon()
        {
            Validate();
            try
            {
                return PluginInterface.GetIpcSubscriber<bool>(NMAPINames.FlashTaskbarIcon).InvokeFunc();
            }
            catch (IpcNotReadyError)
            {
            }
            return false;
        }

        /// <summary>
        /// Attempts to bring game's window foreground. Due to Windows inconsistencies, it's not guaranteed to work. This function does not throws an exception or displays an error if NotificationMaster is not installed.
        /// </summary>
        /// <returns>Whether operation succeeded</returns>
        public bool TryBringGameForeground()
        {
            Validate();
            try
            {
                return PluginInterface.GetIpcSubscriber<bool>(NMAPINames.BringGameForeground).InvokeFunc();
            }
            catch (IpcNotReadyError)
            {
            }
            return false;
        }

        /// <summary>
        /// Begins to play a sound file. If another sound file is already playing, stops previous file and begins playing specified. This function does not throws an exception or displays an error if NotificationMaster is not installed.
        /// </summary>
        /// <param name="pathOnDisk">Path to local file. Can not be web URL. See <see cref="Data.MFAudioFormats"/> for supported formats.</param>
        /// <param name="volume">Volume between 0.0 and 1.0</param>
        /// <param name="repeat">Whether to repeat sound file.</param>
        /// <param name="stopOnGameFocus">Whether to stop file once game is focused. </param>
        /// <returns>Whether operation succeeded</returns>
        public bool PlaySound(string pathOnDisk, float volume = 1f, bool repeat = false, bool stopOnGameFocus = true)
        {
            Validate();
            try
            {
                return PluginInterface.GetIpcSubscriber<string, float, bool, bool, bool>(NMAPINames.PlaySound).InvokeFunc(pathOnDisk, volume, repeat, stopOnGameFocus);
            }
            catch (IpcNotReadyError)
            {
            }
            return false;
        }

        /// <summary>
        /// Stops playing sound. This function does not throws an exception or displays an error if NotificationMaster is not installed.
        /// </summary>
        /// <returns>Whether operation succeeded</returns>
        public bool StopSound()
        {
            Validate();
            try
            {
                return PluginInterface.GetIpcSubscriber<bool>(NMAPINames.StopSound).InvokeFunc();
            }
            catch (IpcNotReadyError)
            {
            }
            return false;
        }
    }
}
