using System;
using Sdp;

namespace SnapdragonProfilerCLI
{
    /// <summary>
    /// Console implementation of IPlatform for SdpApp initialization
    /// Required to use SDPClientFramework's event system that QGLPlugin depends on
    /// </summary>
    public class ConsolePlatform : IPlatform
    {
        private readonly Action _onExitApplication;

        public ConsolePlatform(Action? onExitApplication = null)
        {
            _onExitApplication = onExitApplication ?? (() => Environment.Exit(0));
        }

        public void Invoke(EventHandler handler)
        {
            // For CLI, just execute immediately (no UI thread)
            handler?.Invoke(this, EventArgs.Empty);
        }

        public void SetIdleHandler(IdleHandler handler)
        {
            // No idle loop in CLI mode
        }

        public void ExitApplication()
        {
            _onExitApplication();
        }

        public int ScreenWidth
        {
            get
            {
                try
                {
                    return Console.WindowWidth;
                }
                catch
                {
                    // Return default if console not available (e.g., redirected output)
                    return 1920;
                }
            }
        }

        public int ScreenHeight
        {
            get
            {
                try
                {
                    return Console.WindowHeight;
                }
                catch
                {
                    // Return default if console not available
                    return 1080;
                }
            }
        }
    }
}
