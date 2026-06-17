using Dalamud.Plugin;
using Dalamud.Plugin.Ipc;
using Dalamud.Plugin.Ipc.Exceptions;
using Dalamud.Plugin.Services;
using NotificationMaster;
using System;
using System.Collections.Generic;

namespace NotificationMasterAPI;

public class NotificationMasterApi
{
    private IDalamudPluginInterface PluginInterface;

    /// <summary>
    /// Creates an instance of NotificationMaster API. You do not need to check if NotificationMaster plugin is installed.
    /// </summary>
    /// <param name="dalamudPluginInterface">Plugin interface reference</param>
    public NotificationMasterApi(IDalamudPluginInterface dalamudPluginInterface)
    {
        PluginInterface = dalamudPluginInterface;
    }

    ICallGateSubscriber<string, string, string, bool> CallDisplayToastNotification => field ??= PluginInterface.GetIpcSubscriber<string, string, string, bool>(NMAPINames.DisplayToastNotification);
    ICallGateSubscriber<object> CallActive => field ??= PluginInterface.GetIpcSubscriber<object>(NMAPINames.Active);
    ICallGateSubscriber<string, bool> CallFlashTaskbarIcon => field ??= PluginInterface.GetIpcSubscriber<string, bool>(NMAPINames.FlashTaskbarIcon);
    ICallGateSubscriber<string, bool> CallBringGameForeground => field ??= PluginInterface.GetIpcSubscriber<string, bool>(NMAPINames.BringGameForeground);
    ICallGateSubscriber<string, string, float, bool, bool, bool> CallPlaySound => field ??= PluginInterface.GetIpcSubscriber<string, string, float, bool, bool, bool>(NMAPINames.PlaySound);
    ICallGateSubscriber<string, bool> CallStopSound => field ??= PluginInterface.GetIpcSubscriber<string, bool>(NMAPINames.StopSound);
    ICallGateSubscriber<bool> CallIsGameWindowActivated => field ??= PluginInterface.GetIpcSubscriber<bool>(NMAPINames.IsGameWindowActivated);
    ICallGateSubscriber<string, List<HttpRequestElement>, string[][], bool> CallSendHttpRequest => field ??= PluginInterface.GetIpcSubscriber<string, List<HttpRequestElement>, string[][], bool>(NMAPINames.SendHttpRequest);

    private void Validate()
    {
        if(PluginInterface == null)
        {
            throw new NullReferenceException("NotificationMaster API was called before it was initialized");
        }
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
            return CallActive.HasAction;
        }
        catch(IpcNotReadyError)
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
            if(!CallDisplayToastNotification.HasFunction) return false;
            return CallDisplayToastNotification.InvokeFunc(PluginInterface.InternalName, title ?? PluginInterface.Manifest.Name, text);
        }
        catch(IpcNotReadyError)
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
            if(!CallFlashTaskbarIcon.HasFunction) return false;
            return CallFlashTaskbarIcon.InvokeFunc(PluginInterface.InternalName);
        }
        catch(IpcNotReadyError)
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
            if(!CallBringGameForeground.HasFunction) return false;
            return CallBringGameForeground.InvokeFunc(PluginInterface.InternalName);
        }
        catch(IpcNotReadyError)
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
            if(!CallPlaySound.HasFunction) return false;
            return CallPlaySound.InvokeFunc(PluginInterface.InternalName, pathOnDisk, volume, repeat, stopOnGameFocus);
        }
        catch(IpcNotReadyError)
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
            if(!CallStopSound.HasFunction) return false;
            return CallStopSound.InvokeFunc(PluginInterface.InternalName);
        }
        catch(IpcNotReadyError)
        {
        }
        return false;
    }

    private bool IsGameWindowActivated()
    {
        if(!CallIsGameWindowActivated.HasFunction)
        {
            return false;
        }
        return CallIsGameWindowActivated.InvokeFunc();
    }

    private bool SendHttpRequests(List<HttpRequestElement> elements, string[][]? replacements = null)
    {
        Validate();
        try
        {
            if(!CallSendHttpRequest.HasFunction) return false;
            return CallSendHttpRequest.InvokeFunc(PluginInterface.InternalName, elements, replacements ?? []);
        }
        catch(IpcNotReadyError)
        {
        }
        return false;
    }
}
