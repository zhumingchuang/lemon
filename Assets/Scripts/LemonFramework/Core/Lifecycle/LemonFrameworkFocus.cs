namespace LemonFramework
{
    public static class LemonFrameworkFocus
    {
        internal static readonly LemonLinkedList<ILemonFrameworkFocus> m_FrameworkFocus = new LemonLinkedList<ILemonFrameworkFocus> ();

        public static void OnApplicationFocus (bool focus)
        {
            foreach (ILemonFrameworkFocus module in m_FrameworkFocus)
            {
                module.OnApplicationFocus (focus);
            }
        }
    }
}
