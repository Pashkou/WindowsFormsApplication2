using ClrTest.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


using System.Diagnostics;

using System.Reflection.Emit;
using System.Runtime.Serialization;
using Microsoft.master2.model;
using System.Collections;

namespace Microsoft.master2
{
    public class MethodCalls
    {
        MethodBase _method;

        public MethodCalls()
        {

        }

        public string contains(Type classType)
        {
            string result = "";
            if (classType == null)
            {
                return result;
            }
            MethodInfo[] methodsInfos = classType.GetMethods();
            for (int i = 0; i < methodsInfos.Length; i++)
            {
                result += "\r\n" + methodsInfos[i].Name;

            }
            for (int i = 0; i < methodsInfos.Length; i++)
            {
                // IncomingCall incomingCall = checkCalls(methodsInfos[i], classType);
                // result +=" " +incomingCall.Name;
                // result += " " + incomingCall.ClassName;
            }
            return result;
        }


        //dublica this method to gets the outgoing calls
        //not the button click!
        public static ArrayList checkCallsForIncomingCalls(MethodInfo info, Type classType)
        {

            ArrayList result = new ArrayList();
            //    if (info.GetBaseDefinition().Name  == "ToString") {
            //       return null;
            // }
            var module = classType.Module;
            ILReader reader = new ILReader(info);

            foreach (ILInstruction instruction in reader)
            {
                if (instruction.OpCode == OpCodes.Callvirt)
                {
                    MethodTypeCall methodCall = new MethodTypeCall();
                    try
                    {
                        MethodBase methodBase = module.ResolveMethod(((InlineMethodInstruction)instruction).Token);
                        methodCall.Name = methodBase.Name;
                        methodCall.ClassName = methodBase.DeclaringType.FullName;
                        result.Add(methodCall);
                    }
                    catch (ArgumentOutOfRangeException ar)
                    {

                    }
                }
            }
            return result; ;
        }

        public static ArrayList checkCallsForOutgoingCalls(MethodInfo info, Type classType)
        {

            ArrayList result = new ArrayList();
            //    if (info.GetBaseDefinition().Name  == "ToString") {
            //       return null;
            // }
            var module = classType.Module;
            ILReader reader = new ILReader(info);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 

            foreach (ILInstruction instruction in reader)
            {
                if (instruction.OpCode == OpCodes.Callvirt)
                {
                    MethodTypeCall methodCall = new MethodTypeCall();
                    try
                    {
                        ClrTest.Reflection.InlineMethodInstruction methodInstr = (ClrTest.Reflection.InlineMethodInstruction)instruction;
                        string methodName = methodInstr.Method.Name;
                        methodCall.Name = methodName;
                        methodCall.ClassName = methodInstr.Method.ReflectedType.UnderlyingSystemType.Name;
                        result.Add(methodCall);
                    }
                    catch (ArgumentOutOfRangeException ar)
                    {

                    }
                }
            }
            return result; ;
        }


    }
}
