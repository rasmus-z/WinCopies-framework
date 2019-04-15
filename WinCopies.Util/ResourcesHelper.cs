//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Reflection;

//namespace WinCopies.Util
//{
//    /// <summary>
//    /// Provides some single instance methods to interact with assemblies resources using the WinCopies framework resources gesture design pattern. See the WinCopies website for more explanation.
//    /// </summary>
//    public sealed class ResourcesHelper
//    {

//        public static ResourcesHelper Instance { get; private set; } = null;

//        static readonly object instanceLock = new object();

//        static ResourcesHelper()

//        {

//            if (Instance == null)

//                lock (instanceLock)

//                    if (Instance == null)

//                        Instance = new ResourcesHelper();

//        }
//    }
//}
