// Copyright 2023 Seacraft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \u201CSoftware\u201D), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and\/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED \u201CAS IS\u201D, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;

namespace Seacraft.Framework.Core.Reflection
{
    public class RuntimeHelper
    {
        /// <summary>
        ///  Get the project assembly, exclude all System assemblies (Microsoft.***, System.***, etc.), Nuget download package
        /// </summary>
        /// <returns></returns>
        public static IList<Assembly> GetAllAssemblies()
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            //var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Type!= "referenceassembly");//Exclude all system assemblies and Nuget downloads
            var libs = deps.CompileLibraries.Where(lib => lib.Type == AssembleTypeConsts.Project);//Get only the packages used for this project
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return list;
        }

        public static Assembly GetAssembly(string assemblyName)
        {
            return GetAllAssemblies().FirstOrDefault(assembly => assembly.FullName.Contains(assemblyName));
        }

        public static IList<Type> GetAllTypes()
        {
            var list = new List<Type>();
            foreach (var assembly in GetAllAssemblies())
            {
                var typeInfos = assembly.DefinedTypes;
                foreach (var typeInfo in typeInfos)
                {
                    list.Add(typeInfo.AsType());
                }
            }
            return list;
        }

        public static IList<Type> GetTypesByAssembly(string assemblyName)
        {
            var list = new List<Type>();
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            var typeInfos = assembly.DefinedTypes;
            foreach (var typeInfo in typeInfos)
            {
                list.Add(typeInfo.AsType());
            }
            return list;
        }

        public static Type GetImplementType(string typeName, Type baseInterfaceType)
        {
            return GetAllTypes().FirstOrDefault(t =>
            {
                if (t.Name == typeName &&
                    t.GetTypeInfo().GetInterfaces().Any(b => b.Name == baseInterfaceType.Name))
                {
                    var typeInfo = t.GetTypeInfo();
                    return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType;
                }
                return false;
            });
        }
    }
}
