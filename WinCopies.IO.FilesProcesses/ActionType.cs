namespace WinCopies.IO.FileProcesses
{

    /// <summary>
    /// Énumération servant à décrire le type de processus de fichiers en cours
    /// </summary>
    public enum ActionType
    {

        /// <summary>
        /// The process is not determinated.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The process is a copy.
        /// </summary>
        Copy = 1,

        /// <summary>
        /// The process is a file move.
        /// </summary>
        Move = 2,

        /// <summary>
        /// The process is a file move to the Windows Recycle bin.
        /// </summary>
        MoveToRecycleBin = 3,

        // /// <summary>
        // /// Le processus de fichiers est une suppression. Si le processus marqué comme Deletion plutôt que comme Moving_To_Recycle_Bin ( valeur 2 ), cela signifie que les éléments marqués à supprimer sont effacés définitivement du système.
        // /// 
        // /// Néanmoins, une récupération peut être possible dans certains cas.
        // /// </summary>

        /// <summary>
        /// The process is a deletion.
        /// </summary>
        Deletion = 4,

        // /// <summary>
        // /// Le processus est une recherche de fichiers et dossiers dont certaines caractéristiques correspondent à celles passées au processus de recherche et définient généralement par l'utilisateur.
        // /// </summary>

        /// <summary>
        /// The process is a file search.
        /// </summary>
        Search = 5

    }

}
