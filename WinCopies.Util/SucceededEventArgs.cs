namespace WinCopies.Util
{

    public delegate void SucceededEventHandler(object sender, SucceededEventArgs e);

    public class SucceededEventArgs
    {

        public bool Succeeded { get; } = false;

        public SucceededEventArgs(bool succeeded) => Succeeded = succeeded;

    }

}
