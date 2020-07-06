using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Animation;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    public static class File
    {
        public static bool IsDuplicate(in string leftPath, in string rightPath, in int bufferLength)
        {
            FileStream leftFileStream = null, rightFileStream = null;

            try
            {
                leftFileStream = new FileStream(leftPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferLength, FileOptions.None);

                rightFileStream = new FileStream(rightPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferLength, FileOptions.None);

                bool result = IsDuplicate(leftFileStream, rightFileStream, bufferLength);

                return result;
            }
            finally
            {
                if (leftFileStream != null)
                {
                    leftFileStream.Dispose();

                    rightFileStream?.Dispose();
                }
            }
        }

        public static bool IsDuplicate(in Stream leftStream, in Stream rightStream, in int bufferLength)
        {
            ThrowIfNull(leftStream, nameof(leftStream));
            ThrowIfNull(rightStream, nameof(rightStream));

            byte[] leftBuffer = new byte[bufferLength], rightBuffer = new byte[bufferLength];

            int leftReadSize, rightReadSize;

            while ((leftReadSize = leftStream.Read(leftBuffer, 0, bufferLength)) > 0 && (rightReadSize = rightStream.Read(rightBuffer, 0, bufferLength)) > 0)
            {
                if (leftReadSize == rightReadSize)
                {
                    for (int i = 0; i < bufferLength; i++)

                        if (leftBuffer[i] != rightBuffer[i])

                            return false;
                }

                else

                    return false;
            }

            return true;
        }

        public static bool? IsDuplicate(in Stream leftStream, in Stream rightStream, in int bufferLength, Func<bool> callback)
        {
            ThrowIfNull(leftStream, nameof(leftStream));
            ThrowIfNull(rightStream, nameof(rightStream));

            byte[] leftBuffer = new byte[bufferLength], rightBuffer = new byte[bufferLength];

            int leftReadSize, rightReadSize;

            while ((leftReadSize = leftStream.Read(leftBuffer, 0, bufferLength)) > 0 && (rightReadSize = rightStream.Read(rightBuffer, 0, bufferLength)) > 0)
            {
                if (leftReadSize == rightReadSize)
                {
                    for (int i = 0; i < bufferLength; i++)

                        if (leftBuffer[i] != rightBuffer[i])

                            return false;
                }

                else

                    return false;

                if (callback())

                    return null;
            }

            return true;
        }
    }
}
