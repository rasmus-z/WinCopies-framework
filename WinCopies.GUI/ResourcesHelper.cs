namespace WinCopies.GUI
{

    // todo: to move to xaml

    public sealed class ResourcesHelper

    {

        #region Resources

        /// <summary>
        /// Gets the Ok resource.
        /// </summary>
        public static string Ok => (string)Instance.ResourceDictionary[nameof(Ok)];

        /// <summary>
        /// Gets the Apply resource.
        /// </summary>
        public static string Apply => (string)Instance.ResourceDictionary[nameof(Apply)];

        /// <summary>
        /// Gets the Yes resource.
        /// </summary>
        public static string Yes => (string)Instance.ResourceDictionary[nameof(Yes)];

        /// <summary>
        /// Gets the No resource.
        /// </summary>
        public static string No => (string)Instance.ResourceDictionary[nameof(No)];

        /// <summary>
        /// Gets the Cancel resource.
        /// </summary>
        public static string Cancel => (string)Instance.ResourceDictionary[nameof(Cancel)];

        #endregion

        public static ResourcesHelper Instance { get; private set; } = null;

        public System.Windows.ResourceDictionary ResourceDictionary { get; } = null;

        static ResourcesHelper() => Instance = new ResourcesHelper();

        private ResourcesHelper() => ResourceDictionary = Util.Generic.AddNewDictionary("/WinCopies.GUI;component/Themes/Generic.xaml");

    }
}
