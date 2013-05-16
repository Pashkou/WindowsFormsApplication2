using EnvDTE;
using EnvDTE80;
using Microsoft.master2;
using Microsoft.master2.assembly;
using Microsoft.master2.messagebus;
using Microsoft.master2.model;
using Microsoft.master2.rules;
using Microsoft.Master2.command;
using Microsoft.VisualStudio.Shell;
using Relations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace Microsoft.master.command
{
    class VSEvents
    {
        private DTE dte;
        private DTE2 dte2;
        private EnvDTE.FindEvents m_findEvents;
        private string methodFrom = "";
        private string methodTo = "";
        private IServiceProvider serviceProvider = null;
        private TinyMessengerHub messageHub;
        private ArrayList model;
        private RuleEngine ruleEngine;
        private GraphLayout graphLayout;

        public VSEvents(DTE dte, DTE2 dte2, TinyMessengerHub messageHub, ArrayList model, RuleEngine ruleEngine, GraphLayout graphLayout)
        {
            this.dte = dte;
            this.dte2 = dte2;
            this.messageHub = messageHub;
            this.model = model;
            this.ruleEngine = ruleEngine;
            this.graphLayout = graphLayout;
            //Ctrl+Shift+F
            m_findEvents = dte.Events.FindEvents;
            m_findEvents.FindDone += new EnvDTE._dispFindEvents_FindDoneEventHandler(m_findEvents_FindDone);

            //F12
            serviceProvider = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)dte2);
            VSCommandInterceptor interceptor = new VSCommandInterceptor(serviceProvider,
                 typeof(Microsoft.VisualStudio.VSConstants.VSStd97CmdID).GUID,
                 (int)Microsoft.VisualStudio.VSConstants.VSStd97CmdID.GotoDefn);

            interceptor.BeforeExecute += new EventHandler<EventArgs>(BeforeExecute);
            interceptor.AfterExecute += new EventHandler<EventArgs>(AfterExecute);
        }

       

        private void BeforeExecute(object sender, EventArgs e)
        {
            //get selected function
            EnvDTE.TextSelection doc = (EnvDTE.TextSelection)dte.ActiveDocument.Selection;
            EnvDTE.TextPoint textPoint = doc.ActivePoint;
            CodeElement el = dte.ActiveDocument.ProjectItem.FileCodeModel.CodeElementFromPoint(textPoint, EnvDTE.vsCMElement.vsCMElementFunction);
            methodFrom = el.FullName;
            CSharpClass cSharpClass = prepareClassMethodsFromName(methodFrom);
            StructuralConnection structuralConnection = new StructuralConnection();
        
            structuralConnection.classFrom  = cSharpClass;
            structuralConnection.methodFrom = methodFrom;
            ruleEngine.currentStructuralConnection = structuralConnection;

        }

        private void AfterExecute(object sender, EventArgs e)
        {
            //get selected function
           /* EnvDTE.TextSelection doc = (EnvDTE.TextSelection)dte.ActiveDocument.Selection;
            EnvDTE.TextPoint textPoint = doc.ActivePoint;
            CodeElement el = dte.ActiveDocument.ProjectItem.FileCodeModel.CodeElementFromPoint(textPoint, EnvDTE.vsCMElement.vsCMElementFunction);
            methodTo = el.FullName;
            CSharpClass cSharpClass = prepareClassMethodsFromName(methodTo);*/
           

           /* ruleEngine.classSelectedWithGrpahNoUpdate(cSharpClass, graphLayout, structuralConnection);
              CSharpClass cSharpClass = getCurrentClassFormModel(methodTo);
             ClassMessage classMessage = new ClassMessage();
             classMessage.CSharpClass = cSharpClass;
             messageHub.Publish(classMessage);
             ruleEngine.classSelectedWithGrpah(cSharpClass, graphLayout);*/
        }

        private CSharpClass prepareClassMethodsFromName(string fullName)
        {
            CSharpClass result = null;
            string[] elementsNames = fullName.Split('.');
            try
            {
                AssemblyInstance asInst = new AssemblyInstance();
                Type[] types = asInst.Assembly.GetTypes();
                // Type[] types = asInst.getTypes();
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
                CSharpMethod cSharpMethod = new CSharpMethod();
                cSharpMethod.Name = elementsNames[2];
                cSharpMethod.Relevance = 0.1;

                result.Methods.Add(cSharpMethod);
            }
            catch (Exception e)
            {
            }
            return result;
        }

        private void OnFindDone(vsFindResult result, bool cancelled)
        {
            System.Diagnostics.Debug.Write("FindDone event fired with vsFindResult = ");
            System.Diagnostics.Debug.WriteLine(result.ToString());
        }


        void m_findEvents_FindDone(EnvDTE.vsFindResult Result, bool Cancelled)
        {

            // Get search term, window location, etc...;

            var x = dte.Find.FindWhat;
            var guid = dte.Find.ResultsLocation == vsFindResultsLocation.vsFindResults1 ?
                         "{0F887920-C2B6-11D2-9375-0080C747D9A0}" : "{0F887921-C2B6-11D2-9375-0080C747D9A0}";

            var findWindow = dte.Windows.Item(guid);
            var selection = findWindow.Selection as System.Windows.Documents.TextSelection;
            // Get search text results;
            // var endPoint = selection.AnchorPoint.CreateEditPoint();
            // endPoint.EndOfDocument();
            //var text = endPoint.GetLines(1, endPoint.Line);

        }

        private CSharpClass getCurrentClassFormModel(string className)
        {
            if (model == null)
            {
                return null;
            }

            string[] strUtl = className.Split('.');
            CSharpClass cSharpClass = null;
            foreach (CSharpClass currentCSharpClass in model)
            {
                if (currentCSharpClass.Name == strUtl[1])
                {
                    cSharpClass = currentCSharpClass;
                }
            }

            ArrayList methods = cSharpClass.Methods;
            foreach (CSharpMethod cSharpMethod in methods)
            {
                if (cSharpMethod.Name == strUtl[2])
                {
                    cSharpMethod.Relevance++;
                    break;
                }
            }
            return cSharpClass;
        }


    }
}
