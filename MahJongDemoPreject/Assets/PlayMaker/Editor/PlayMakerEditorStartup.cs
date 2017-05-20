using UnityEngine;
using UnityEditor;
using System.Collections;

namespace HutongGames.PlayMakerEditor
{
    /// <summary>
    /// Manages initialization of PlayMaker Editor classes
    /// Before Unity 5.4 a lot of this was done in EditorWindow constructors.
    /// In Unity 5.4+ this is not allowed and throws an error.
    /// Unity API calls are also not allowed in constructors of Serializable classes.
    /// So instead we do it all here in a non-Serializable class.
    /// </summary>
    [InitializeOnLoad]
    public class PlayMakerEditorStartup
    {
        static PlayMakerEditorStartup()
        {
            //Debug.Log(EditorApplication.timeSinceStartup);

#if PLAYMAKER
            PlayMakerGlobals.InitApplicationFlags();
#endif

            // Resources.Load fails in static constructor (unity bug?)
            // So we delay some work that needs PlayMakerEditorPrefs until next update

            EditorApplication.update -= DoWelcomeScreen;
            EditorApplication.update += DoWelcomeScreen;

            // Constructor is also called on Playmode change
            // So we need to handle that case (e.g., don't show welcome window)

            EditorApplication.playmodeStateChanged -= PlayModeChanged;
            EditorApplication.playmodeStateChanged += PlayModeChanged;
        }

        private static void DoWelcomeScreen()
        {
            //Debug.Log("ShowWelcomeScreen: " + PlayMakerEditorPrefs.ShowWelcomeScreen);
            //Debug.Log("WelcomeScreenVersion: " + PlayMakerEditorPrefs.WelcomeScreenVersion);

            const float startupTime = 30f; // time window to filter startup events from re-compiles. TODO: Is there a better way?
            var showAtStartup = EditorStartupPrefs.ShowWelcomeScreen && EditorApplication.timeSinceStartup < startupTime;
            var newVersionImported = EditorStartupPrefs.WelcomeScreenVersion != PlayMakerWelcomeWindow.Version;

            if (showAtStartup || newVersionImported)
            {
                PlayMakerWelcomeWindow.Open();
            }

            // record the welcome screen version
            EditorStartupPrefs.WelcomeScreenVersion = PlayMakerWelcomeWindow.Version;

            EditorApplication.update -= DoWelcomeScreen;
        }

        private static void PlayModeChanged()
        {
            EditorApplication.update -= DoWelcomeScreen;
        }
    }
}
