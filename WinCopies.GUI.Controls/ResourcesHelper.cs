namespace WinCopies.GUI.Controls
{

    // todo: to move to xaml

    public sealed class ResourcesHelper

    {
        public static ResourcesHelper Instance { get; private set; } = null;

        public System.Windows. ResourceDictionary ResourceDictionary { get; } = null;

        static ResourcesHelper() => Instance = new ResourcesHelper();

        private ResourcesHelper() => ResourceDictionary = Util.Generic.AddNewDictionary("/WinCopies.GUI.Controls;component/Themes/Generic.xaml");

    }
}
