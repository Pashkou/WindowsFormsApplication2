using Company.Master2.xmlmodel;
using EnvDTE;
using Microsoft.master2.assembly;
using Microsoft.master2.messagebus;
using Microsoft.master2.model;
using Microsoft.master2.rules;
using Relations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TinyMessenger;

namespace Company.Master2.cursorcodeelement
{
    class CodeElementAtCursor
    {
        GraphLayout gL;
        DTE dte;
        TinyMessengerHub messageHub;
        RuleEngine ruleEngine;
  

        public CodeElementAtCursor(DTE dte, TinyMessengerHub messageHub, GraphLayout gL, RuleEngine ruleEngine)
        {
            this.dte = dte;
            this.messageHub = messageHub;
            this.gL = gL;
            this.ruleEngine = ruleEngine;
            //this.model = model;
        }


        private CSharpClass getCurrentClassFormModel(string className)
        {
            if (ruleEngine.xmlModel == null)
            {
                return null;
            }
            foreach (CSharpClass cSharpClass in ruleEngine.xmlModel)
            {
                if (cSharpClass.Name == className)
                {
                    return cSharpClass;
                }
            }
            return null;
        }
      

        private string lastElementName = "";
        public EnvDTE.CodeElement GetCodeElementAtCursor()
        {
            EnvDTE.CodeElement objCodeElement = null;
            EnvDTE.TextPoint objCursorTextPoint = default(EnvDTE.TextPoint);
            try
            {
                objCursorTextPoint = GetCursorTextPoint();
                if ((objCursorTextPoint != null))
                {
                    // Get the class at the cursor
                    objCodeElement = GetCodeElementAtTextPoint(vsCMElement.vsCMElementClass, dte.ActiveDocument.ProjectItem.FileCodeModel.CodeElements, objCursorTextPoint);

                }
                if (objCodeElement == null)
                {
                    return null;
                }
                else
                {
                    if (lastElementName != objCodeElement.FullName)
                    {
                        lastElementName = objCodeElement.FullName;
                        CSharpClass cSharpClass = new CSharpClass();
                        cSharpClass.Name = objCodeElement.FullName;

                        ClassMessage classMessage = new ClassMessage();
                        classMessage.CSharpClass = cSharpClass;
                        messageHub.Publish(classMessage);

                        return objCodeElement;
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }


        private string getMiddlePartOfName(string name)
        {
            string[] st = name.Split('.');

            return st[st.Length - 2];
        }

        public EnvDTE.CodeElement GetMethodCodeElementAtCursor()
        {
            EnvDTE.CodeElement objCodeElement = null;
            EnvDTE.TextPoint objCursorTextPoint = default(EnvDTE.TextPoint);
            try
            {
                objCursorTextPoint = GetCursorTextPoint();
                if ((objCursorTextPoint != null))
                {
                    // Get the method at the cursor
                    objCodeElement = GetCodeElementAtTextPoint(vsCMElement.vsCMElementFunction, dte.ActiveDocument.ProjectItem.FileCodeModel.CodeElements, objCursorTextPoint);
                }
                if (objCodeElement == null)
                {
                    return null;
                }
                else
                {
                    if (lastElementName != objCodeElement.FullName)
                    {
                        lastElementName = objCodeElement.FullName;
                        CSharpClass cSharpClass =prepareClassMethodsFromName(objCodeElement.FullName);// readXml.readClassFromFiles(currentClass.Name, "model.xml");
                        ClassMessage classMessage = new ClassMessage();
                        classMessage.CSharpClass = cSharpClass;
                        
                        // messageHub.Publish(classMessage);
                        if ((cSharpClass != null) && (gL != null) && (ruleEngine != null))
                        {
                            ruleEngine.classSelectedWithGrpahRefreshButton(cSharpClass, gL, true);
                        }
                        //ruleEngine.updateRelevanceOfClass(cSharpClass);
                        return objCodeElement;
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }

        private CSharpClass prepareClassMethodsFromName(string fullName)
        {
            CSharpClass result = null;
            string[] elementsNames = fullName.Split('.');
            try
            {
                result = getCurrentClassFormModel(getMiddlePartOfName(fullName));
                result.Relevance = 0;

                ArrayList methods = result.Methods;
                foreach (CSharpMethod cSharpMethod in methods)
                {
                    if (cSharpMethod.Name == elementsNames[2])
                    {
                        cSharpMethod.Relevance = 0.1;
                        result.SelectedMethod = cSharpMethod.Name;
                        break;
                    }
                }
                /*CSharpMethod cSharpMethod = new CSharpMethod();
                cSharpMethod.Name = elementsNames[2];
                cSharpMethod.Relevance = 0.1;
                result.SelectedMethod = cSharpMethod.Name;
                result.Methods.Add(cSharpMethod);*/
            }
            catch (Exception e)
            {
            }
            return result;
        }

        public EnvDTE.CodeElement GetPropertyCodeElementAtCursor()
        {
            EnvDTE.CodeElement objCodeElement = default(EnvDTE.CodeElement);
            EnvDTE.TextPoint objCursorTextPoint = default(EnvDTE.TextPoint);
            try
            {
                objCursorTextPoint = GetCursorTextPoint();
                if ((objCursorTextPoint != null))
                {
                    // Get the method at the cursor
                    objCodeElement = GetCodeElementAtTextPoint(vsCMElement.vsCMElementProperty, dte.ActiveDocument.ProjectItem.FileCodeModel.CodeElements, objCursorTextPoint);

                }
                if (objCodeElement == null)
                {
                    return null;
                }
                else
                {
                    CSharpClass cSharpClass = new CSharpClass();// readXml.readClassFromFiles(currentClass.Name, "model.xml");
                    cSharpClass.Name = objCodeElement.FullName;
                    ClassMessage classMessage = new ClassMessage();
                    classMessage.CSharpClass = cSharpClass;
                    messageHub.Publish(classMessage);
                    return objCodeElement;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;

        }

        public EnvDTE.CodeElement GetFieldCodeElementAtCursor()
        {
            EnvDTE.CodeElement objCodeElement = default(EnvDTE.CodeElement);
            EnvDTE.TextPoint objCursorTextPoint = default(EnvDTE.TextPoint);
            try
            {
                objCursorTextPoint = GetCursorTextPoint();
                if ((objCursorTextPoint != null))
                {
                    // Get the method at the cursor
                    objCodeElement = GetCodeElementAtTextPoint(vsCMElement.vsCMElementVariable, dte.ActiveDocument.ProjectItem.FileCodeModel.CodeElements, objCursorTextPoint);

                }
                if (objCodeElement == null)
                {
                    return null;
                }
                else
                {
                    if (lastElementName != objCodeElement.FullName)
                    {
                        lastElementName = objCodeElement.FullName;
                        CSharpClass cSharpClass = prepareClassFieldFromName(objCodeElement.FullName);// readXml.readClassFromFiles(currentClass.Name, "model.xml");
                        ClassMessage classMessage = new ClassMessage();
                        classMessage.CSharpClass = cSharpClass;
                        //  messageHub.Publish(classMessage);
                        ruleEngine.classSelectedWithGrpah(cSharpClass, gL, false);
                        return objCodeElement;
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }

        private CSharpClass prepareClassFieldFromName(string fullName)
        {
            CSharpClass result = null;
            string[] elementsNames = fullName.Split('.');
            try
            {
                AssemblyInstance asInst = new AssemblyInstance();
                Type[] types = asInst.Assembly.GetTypes();
                //Type[] types = asInst.getTypes();
                Type t = null;
                foreach (Type type in types)
                {
                    if (type.FullName == elementsNames[0] + "." + elementsNames[1])
                    {
                        t = type;
                        break;
                    }
                }
                result = new CSharpClass();
                result.Name = t.Name;

                if (t.DeclaringType != null)
                {
                    result.Parent = t.DeclaringType.Name;
                }
                else
                {
                    result.Parent = "Object";
                }
                result.Relevance = 0;
                CSharpField cSharpField = new CSharpField();
                cSharpField.Name = elementsNames[2];
                cSharpField.Relevance = 0.1;
                result.Fields.Add(cSharpField);
            }
            catch (Exception e)
            {
            }
            return result;

        }
        private EnvDTE.TextPoint GetCursorTextPoint()
        {
            EnvDTE.TextDocument objTextDocument = default(EnvDTE.TextDocument);
            EnvDTE.TextPoint objCursorTextPoint = default(EnvDTE.TextPoint);
            try
            {
                objTextDocument = dte.ActiveDocument.Object() as TextDocument;
                objCursorTextPoint = objTextDocument.Selection.ActivePoint;
            }
            catch (System.Exception ex)
            {
            }
            return objCursorTextPoint;
        }
        private EnvDTE.CodeElement GetCodeElementAtTextPoint(EnvDTE.vsCMElement eRequestedCodeElementKind, EnvDTE.CodeElements colCodeElements, EnvDTE.TextPoint objTextPoint)
        {

            //EnvDTE.CodeElement objCodeElement = default(EnvDTE.CodeElement);
            EnvDTE.CodeElement objResultCodeElement = default(EnvDTE.CodeElement);
            EnvDTE.CodeElements colCodeElementMembers = default(EnvDTE.CodeElements);
            EnvDTE.CodeElement objMemberCodeElement = default(EnvDTE.CodeElement);
            if ((colCodeElements != null))
            {
                foreach (EnvDTE.CodeElement objCodeElement in colCodeElements)
                {
                    if (objCodeElement.StartPoint.GreaterThan(objTextPoint))
                    {
                        // The code element starts beyond the point
                    }
                    else if (objCodeElement.EndPoint.LessThan(objTextPoint))
                    {
                        // The code element ends before the point
                        // The code element contains the point
                    }
                    else
                    {
                        if (objCodeElement.Kind == eRequestedCodeElementKind)
                        {
                            // Found
                            objResultCodeElement = objCodeElement;
                        }
                        // We enter in recursion, just in case there is an inner code element that also 
                        // satisfies the conditions, for example, if we are searching a namespace or a class
                        colCodeElementMembers = GetCodeElementMembers(objCodeElement);
                        objMemberCodeElement = GetCodeElementAtTextPoint(eRequestedCodeElementKind, colCodeElementMembers, objTextPoint);
                        if ((objMemberCodeElement != null))
                        {
                            // A nested code element also satisfies the conditions
                            objResultCodeElement = objMemberCodeElement;
                        }
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
            return objResultCodeElement;
        }

        private EnvDTE.CodeElements GetCodeElementMembers(CodeElement objCodeElement)
        {
            EnvDTE.CodeElements colCodeElements = default(EnvDTE.CodeElements);
            if (objCodeElement is EnvDTE.CodeNamespace)
            {
                colCodeElements = ((EnvDTE.CodeNamespace)objCodeElement).Members;

            }
            else if (objCodeElement is EnvDTE.CodeType)
            {
                colCodeElements = ((EnvDTE.CodeType)objCodeElement).Members;
            }
            else if (objCodeElement is EnvDTE.CodeFunction)
            {
                colCodeElements = ((EnvDTE.CodeFunction)objCodeElement).Parameters;
            }
            return colCodeElements;
        }


    }


}

