//#if DEBUG 
//namespace WinCopies.IO
//{
//    public static class RegistryInterop
//    {
//        public static void test(Microsoft.WindowsAPICodePack.Shell.ShellObject shellFile)

//        {

//            foreach (Microsoft.WindowsAPICodePack.Shell.PropertySystem.IShellProperty shellProperty in shellFile.Properties.DefaultPropertyCollection)

//                try

//                {

//                    System.Windows.MessageBox.Show(shellProperty.CanonicalName + " " + shellProperty.Description.DisplayName + " " + shellProperty.ValueAsObject.ToString());

//                }

//                catch (System.Exception ex)

//                {
//                    System.Windows.MessageBox.Show("Exception on " + shellProperty.CanonicalName);


//                }

//        }
//    }
//}
//#endif 
