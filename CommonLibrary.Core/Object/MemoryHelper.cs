using System;
using System.Collections.Generic;
using System.Text;

namespace hwj.CommonLibrary.Object
{
    public class MemoryHelper
    {
        /// <summary>
        /// 释放物理内存
        /// </summary>
        public static void ReleaseMemory()
        {
            System.Diagnostics.Process loProcess = System.Diagnostics.Process.GetCurrentProcess();
            loProcess.MaxWorkingSet = loProcess.MaxWorkingSet;//(IntPtr)lnMaxSize;
        }
    }
}
