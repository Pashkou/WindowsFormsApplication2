using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.master2.assembly
{
    class AssemblyInstance
    {
        private string assemblyName = "C://Users//Maz//Documents//Visual Studio 2012//Projects//WindowsFormsApplication3//WindowsFormsApplication3//bin//Debug//WindowsFormsApplication3.exe";
        private Assembly assembly = null;
        public Assembly Assembly
        {
            get
            {
                if (assembly == null)
                {
                    try
                    {

                        assembly = Assembly.LoadFile(assemblyName);
                    
                    }
                    catch (Exception e)
                    {
                    }
                }
                return assembly;
            }
            set { assembly = value; }
        }

    }

}
    