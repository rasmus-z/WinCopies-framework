// todo : orthographe de 'occured' ? + autres fichiers

namespace WinCopies.IO.FileProcesses
{
    public enum HowToRetry
    {

        // todo : ou none ? - autre place/valeur dans l'enum ? - ou autre nom ?

            /// <summary>
            /// Does not retry.
            /// </summary>
        None = 0,

        /// <summary>
        /// Ignores the current file and pass to the next item.
        /// </summary>
        Ignore = 1, 

        /// <summary>
        /// Retry without any modification on the process for the current file.
        /// </summary>
        Retry = 2,

        /// <summary>
        /// Renames the current file.
        /// </summary>
        Rename = 3,

        /// <summary>
        /// Replaces the file.
        /// </summary>
        Replace = 4, 

        Cancel = 5

    }
}
