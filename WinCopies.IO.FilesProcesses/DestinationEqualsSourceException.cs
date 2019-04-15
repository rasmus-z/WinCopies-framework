using System.IO;

namespace WinCopies.IO.FileProcesses
{

    // todo:

    /// <summary>
    /// Représente les erreurs qui se produisent lorsque la destination d'un répertoire est égale à sa source lors d'un processus de copie ou de déplacement.
    /// </summary>
    public class DestinationEqualsSourceException : IOException
    {

        /// <summary>
        /// Répertoire de destination.
        /// </summary>
        public string Dest_Path { get; } = "";

        /// <summary>
        /// Le répertoire initial représentant le conflit.
        /// </summary>
        public string Path_With_Conflict { get; } = "";

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="DestinationEqualsSourceException"/>.
        /// </summary>
        /// <param name="message">Message décrivant l'exception.</param>
        /// <param name="dest_path">Répertoire de destination.</param>
        /// <param name="path_with_conflict">Répertoire initial représentant le conflit.</param>
        public DestinationEqualsSourceException(string message, string dest_path, string path_with_conflict)
            : base(message)
        {

            Dest_Path = dest_path;

            Path_With_Conflict = path_with_conflict;

        }

    }

}
