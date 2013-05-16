using Company.Master2.xmlmodel;
using Microsoft.master2.assembly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.master2.model
{
    class ModelUtils
    {
        private Type currentClass;

        public ModelUtils(Type currentClass)
        {
            this.currentClass = currentClass;
        }


        public ArrayList getModelForCurrentAssembly()
        {

            ArrayList cSharpClassesList = new ArrayList();
            //if (currentClass == null)
            //{
              //  return new ArrayList();
            //}

           // Type[] types = Assembly.GetAssembly(currentClass).GetTypes();
            AssemblyInstance ai = new AssemblyInstance();
            
            Type[] types = ai.Assembly.GetTypes();

            foreach (Type type in types)
            {
                CSharpClass cSharpClass = new CSharpClass();
                cSharpClass.Name = type.Name;
                cSharpClass.Parent = type.BaseType.Name;
                Type[] currentTypeInterfaces = type.GetInterfaces();
                foreach (Type currentInterface in currentTypeInterfaces)
                {
                    cSharpClass.Interfaces.Add(currentInterface.Name);
                }
                MethodInfo[] methodInfos = type.GetMethods();
                foreach (MethodInfo methodInfo in methodInfos)
                {
                    if (methodInfo.DeclaringType.Name == cSharpClass.Name)//check if the method from the current class and not from ingherited
                    {
                        CSharpMethod cSharpMethod = new CSharpMethod();
                        cSharpMethod.Name = methodInfo.Name;
                        cSharpMethod.IncomingCalls = drillMethodForIncomingClass(type, methodInfo);
                        cSharpMethod.OutgoingCalls = drillMethodForOutgoingClass(methodInfo, cSharpClass.GetType());
                        cSharpClass.Methods.Add(cSharpMethod);
                    }
                }
                CSharpClass.calculateEdges(cSharpClass);

                PropertyInfo[] propertieInfos = type.GetProperties();

                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    if (fieldInfo.DeclaringType.Name == cSharpClass.Name)//check if the method from the current class and not from ingherited
                    {
                        CSharpField cSharpField = new CSharpField();
                        cSharpField.Name = fieldInfo.Name;
                        cSharpClass.Fields.Add(cSharpField);
                    }

                }
                cSharpClassesList.Add(cSharpClass);
            }

            return cSharpClassesList;
        }

        private ArrayList drillMethodForIncomingClass(Type currentType, MethodInfo method)
        {
            ArrayList result = new ArrayList();
            ArrayList cSharpClassesList = new ArrayList();
           // Type[] types = Assembly.GetAssembly(currentClass).GetTypes();
            AssemblyInstance ai = new AssemblyInstance();

            Type[] types = ai.Assembly.GetTypes();


            foreach (Type type in types)
            {
                MethodInfo[] methodInfos = type.GetMethods();
                foreach (MethodInfo methodInfo in methodInfos)
                {
                    if (methodInfo.DeclaringType.Name == type.Name)//check if the method from the current class and not from ingherited
                    {
                        ArrayList methodCalls = MethodCalls.checkCallsForIncomingCalls(methodInfo, type);
                        foreach (MethodTypeCall incomingCall in methodCalls)
                        {
                            if (incomingCall.ClassName + "." + incomingCall.Name == currentType.FullName + "." + method.Name)

                            // if (incomingCall.Name == method.Name)
                            {
                                IncomingCall incomingCallToMethod = new IncomingCall();
                                incomingCallToMethod.Name = methodInfo.Name;
                                incomingCallToMethod.ClassName = type.Name;
                                result.Add(incomingCallToMethod);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private ArrayList drillMethodForOutgoingClass(MethodInfo methodInfo, Type type)
        {

            ArrayList methodCalls = MethodCalls.checkCallsForOutgoingCalls(methodInfo, type);

            return methodCalls;
        }
    }
}
