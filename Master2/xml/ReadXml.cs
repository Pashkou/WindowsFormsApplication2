using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Xml;
using System.Collections;
using Microsoft.master2.model;

namespace Microsoft.master2.xml
{
    class ReadXml
    {

        public ArrayList readModelFromFile(string fileName)
        {
            ArrayList result = new ArrayList();
            XmlReader reader = XmlReader.Create(fileName);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == Tags.CLASS)
                {

                    result.Add(readClass(reader));
                } //end if
            } //end while
            return result;
        }

        public CSharpClass readClassFromFiles(string className, string fileName)
        {
            CSharpClass result = new CSharpClass();
            XmlReader reader = XmlReader.Create(fileName);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == Tags.CLASS)
                {
                    string currentClassName = reader.GetAttribute(0);
                    if (currentClassName == className)
                    {
                        return readClass(reader);
                    }
                } //end if
            } //end while
            return result;
        }

        int numberOfClasses = 0;
        System.IO.StreamWriter file = new System.IO.StreamWriter("e:\\test2.txt");

        private CSharpClass readClass(XmlReader reader)
        {
            numberOfClasses = numberOfClasses + 1;

            CSharpClass cSharpClass = new CSharpClass();
            cSharpClass.Name = reader.GetAttribute(0);
          /*  while (reader.NodeType != XmlNodeType.EndElement)
            {
                file.WriteLine("1");
                reader.Read();
                if (reader.Name == Tags.PARENT)
                {
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            cSharpClass.Parent = reader.Value;
                        }
                    }
                    reader.Read();
                } //end if

                file.WriteLine("2");
                if (reader.Name == Tags.INTERFACES)
                {
                    if (reader.IsEmptyElement)
                    {
                        break;//end of reading interfaces
                    }
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        reader.Read();
                        if (reader.Name == Tags.INTERFACE)
                        {
                            while (reader.NodeType != XmlNodeType.EndElement)
                            {
                                reader.Read();
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    cSharpClass.Interfaces.Add(reader.Value);
                                }
                            }
                            reader.Read();
                        } //end if     
                    }
                } //end if
            } *///end while
            bool lastMethod = false;
            file.WriteLine("3");
            while (!lastMethod)
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Element && reader.Name == Tags.METHODS)
                {
                    cSharpClass.Methods = readSubTreeMethods(reader.ReadSubtree());
                    lastMethod = true;
                }
            }
            file.WriteLine("Number " + numberOfClasses + " Name " + cSharpClass.Name);
            return cSharpClass;
        }

        private ArrayList readSubTreeMethods(XmlReader reader)
        {
            ArrayList result = new ArrayList();
            while (reader.Read())
            {
                if ((reader.Name == "Method") && (reader.NodeType != XmlNodeType.EndElement))
                {
                    CSharpMethod cSharpMethod = new CSharpMethod();
                    cSharpMethod.Name = reader.GetAttribute(0);
                    if (!reader.IsEmptyElement)
                    {
                        readSubTreeMethod(cSharpMethod, reader.ReadSubtree());
                    }
                    result.Add(cSharpMethod);
                }
            }
            return result;
        }


        private ArrayList readSubTreeMethod(CSharpMethod cSharpMethod, XmlReader reader)
        {
            ArrayList result = new ArrayList();
            reader.Read();

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.Read();
                if (reader.Name == Tags.INCOMING_METHOD)
                {
                    IncomingCall incomingCall = new IncomingCall();
                    incomingCall.ClassName = reader.GetAttribute(0);
                    incomingCall.Name = reader.GetAttribute(1);
                    cSharpMethod.IncomingCalls.Add(incomingCall);
                }
                if (reader.Name == Tags.OUTGOING_METHOD)
                {
                    OutgoingCall outgoingCall = new OutgoingCall();
                    outgoingCall.ClassName = reader.GetAttribute(0);
                    outgoingCall.Name = reader.GetAttribute(1);
                    cSharpMethod.OutgoingCalls.Add(outgoingCall);
                }

            }
            return result;
        }
    }
}
