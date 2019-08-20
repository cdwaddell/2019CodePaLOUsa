namespace MicroServicesDemo
{
    public class DebugSettings
    {        
        /// <summary>
        /// Static check for debugging state
        /// </summary>
        public static bool IsDebugging =>
#if DEBUG
            true;
#else
            false;
#endif
    }
}
