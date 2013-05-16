using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System.Reflection;
using System.IO;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.master2.xml;
using System.Collections;
using Microsoft.master2.model;
using QuickGraph;
using Microsoft.master2.graph;
using Company.Master2;



namespace Microsoft.master2.backup
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class MyControl : UserControl
    {
        private DTE dte;
        private DTE2 dte2;
        EnvDTE.FindEvents m_findEvents;
        private IServiceProvider serviceProvider = null;

        string methodFrom = "";
        string methodTo = "";


        private IBidirectionalGraph<object, IEdge<object>> _graphToVisualize;
        public IBidirectionalGraph<object, IEdge<object>> GraphToVisualize
        {
            get { return _graphToVisualize; }
        }


        private void BeforeExecute(object sender, EventArgs e)
        {
            //get selected function
            EnvDTE.TextSelection doc = (EnvDTE.TextSelection)dte.ActiveDocument.Selection;
            EnvDTE.TextPoint textPoint = doc.ActivePoint;
            CodeElement el = dte.ActiveDocument.ProjectItem.FileCodeModel.CodeElementFromPoint(textPoint, EnvDTE.vsCMElement.vsCMElementFunction);
            methodFrom = el.FullName;
        }

        private void AfterExecute(object sender, EventArgs e)
        {
            //get selected function
            EnvDTE.TextSelection doc = (EnvDTE.TextSelection)dte.ActiveDocument.Selection;
            EnvDTE.TextPoint textPoint = doc.ActivePoint;
            CodeElement el = dte.ActiveDocument.ProjectItem.FileCodeModel.CodeElementFromPoint(textPoint, EnvDTE.vsCMElement.vsCMElementFunction);
            methodTo = el.FullName;
            // textBlock2.Text = "method from: " + methodFrom + " \r\nmethod to: " + methodTo;
        }


        //  GraphUtils gUt = new GraphUtils();
        public MyControl()
        {
            // _graphToVisualize = gUt.prepareGraph();


            IVsTextManager textManager = Master2Package.GetGlobalService(typeof(SVsTextManager)) as IVsTextManager;
            Microsoft.VisualStudio.OLE.Interop.IConnectionPointContainer container = textManager as Microsoft.VisualStudio.OLE.Interop.IConnectionPointContainer;
            Microsoft.VisualStudio.OLE.Interop.IConnectionPoint textManagerEventsConnection = null;
            Guid eventGuid = typeof(IVsTextManagerEvents).GUID;
            container.FindConnectionPoint(ref eventGuid, out textManagerEventsConnection);
            uint cookie = 0;
            textManagerEventsConnection.Advise(new TextManager(), out cookie);


            // first get the DTE object from the DataContext.
            //  dte = propertyCollection.Find("DTE", false).GetValue(DataContext) as DTE2;

            dte = Master2Package.GetGlobalService(typeof(DTE)) as DTE;
            dte2 = Master2Package.GetGlobalService(typeof(DTE)) as DTE2;

            dte.Events.WindowEvents.WindowActivated += WindowEvents_WindowActivated;

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


            //find event class

            //dte.Events.TextEditorEvents.
            //dte.Events.TaskListEvents.
            //dte.Events.SolutionItemsEvents
            //dte.Events.SolutionEvents
            // dte.Events.SelectionEvents.OnChange
            //dte.Events.OutputWindowEvents
            //dte.Events.MiscFilesEvents
            //dte.Events.FindEvents.
            // dte.Events.DocumentEvents.

            //dte.Events.DebuggerEvents.

            //dte.Events.CommandEvents
            //dte.Events.CommandBarEvents
            //dte.Events.BuildEvents


            // dte.Events.TextEditorEvents.LineChanged += new EnvDTE._dispTextEditorEvents_LineChangedEventHandler;

        }

        private void OnFindDone(vsFindResult result, bool cancelled)
        {
            System.Diagnostics.Debug.Write("FindDone event fired with vsFindResult = ");
            System.Diagnostics.Debug.WriteLine(result.ToString());
        }


        void m_lineChanedEvents_LineChanged(EnvDTE.vsFindResult Result, bool Cancelled)
        {



        }

        void m_findEvents_FindDone(EnvDTE.vsFindResult Result, bool Cancelled)
        {

            // Get search term, window location, etc...;
            drawElem("elementsList");
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

        private void WindowEvents_WindowActivated(EnvDTE.Window GotFocus, EnvDTE.Window LostFocus)
        {
            if ((dte.ActiveDocument != null) && (dte.ActiveDocument.ProjectItem != null))
            {
                FileCodeModel fileCM = dte.ActiveDocument.ProjectItem.FileCodeModel;
                if (fileCM != null)
                {
                    CodeElements elts = null;
                    elts = fileCM.CodeElements;
                    CodeElement elt = null;
                    int i = 0;
                    string elementsList = "";
                    for (i = 1; i <= fileCM.CodeElements.Count; i++)
                    {
                        elt = elts.Item(i);
                        elementsList += " " + CollapseElt(elt, elts, i);
                    }
                    drawElem(elementsList);
                }
            }
            // textBlock2.Text = "";
        }

        private string checkChildren(Type parentType)
        {

            string result = "";
            Type[] types = Assembly.GetAssembly(parentType).GetTypes();
            foreach (Type type in types)
            {

                if ((type.BaseType != null) && (type.BaseType.Equals(parentType)))
                {
                    result += type.FullName;
                }
            }
            //  }
            return result;

        }


        private Type currentClass = null;

        public string CollapseElt(CodeElement elt, CodeElements elts, long loc)
        {
            string result = "";
            EditPoint epStart = null;
            EditPoint epEnd = null;
            epStart = elt.StartPoint.CreateEditPoint();
            // Do this because we move it later.
            epEnd = elt.EndPoint.CreateEditPoint();
            epStart.EndOfLine();
            if (((elt.IsCodeType) & (elt.Kind != vsCMElement.vsCMElementDelegate)))
            {
                currentClass = Type.GetType(elt.FullName);

                if (currentClass != null)
                {
                    result += "\r\n class name " + currentClass.Name + "\r\n superclassName " + currentClass.BaseType.Name;
                    result += "\r\n subType " + checkChildren(currentClass);
                }
                CodeType ct = null;
                ct = ((EnvDTE.CodeType)(elt));
                CodeElements mems = null;
                mems = ct.Members;
                int i = 0;
                for (i = 1; i <= ct.Members.Count; i++)
                {
                    result += " " + CollapseElt(mems.Item(i), mems, i);
                }

                MethodCalls MC = new MethodCalls();
                result += "\r\n Methods: " + MC.contains(currentClass);
            }
            else if ((elt.Kind == vsCMElement.vsCMElementNamespace))
            {
                CodeNamespace cns = null;
                cns = ((EnvDTE.CodeNamespace)(elt));
                CodeElements mems_vb = null;
                mems_vb = cns.Members;
                int i = 0;

                for (i = 1; i <= cns.Members.Count; i++)
                {
                    result += "\r\n " + CollapseElt(mems_vb.Item(i), mems_vb, i);
                }
            }
            return result;
        }

        private void drawElem(string srt)
        {

            // textBlock1.Text = srt;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ((dte.ActiveDocument != null) && (dte.ActiveDocument.ProjectItem != null))
            {
                FileCodeModel fileCM = dte.ActiveDocument.ProjectItem.FileCodeModel;
                if (fileCM != null)
                {
                    CodeElements elts = null;
                    elts = fileCM.CodeElements;
                    CodeElement elt = null;
                    int i = 0;
                    string elementsList = "";
                    for (i = 1; i <= fileCM.CodeElements.Count; i++)
                    {
                        elt = elts.Item(i);
                        elementsList += " " + CollapseElt(elt, elts, i);
                    }
                    drawElem(elementsList);
                }
            }
        }



        private void Create_Model_Click(object sender, RoutedEventArgs e)
        {
            ModelUtils mUtils = new ModelUtils(currentClass);
            ArrayList cSharpClassesList = mUtils.getModelForCurrentAssembly();
        }

        private void Save_Context_Click(object sender, RoutedEventArgs e)
        {
            ModelUtils mUtils = new ModelUtils(currentClass);
            ArrayList cSharpClassesList = mUtils.getModelForCurrentAssembly();

            WriteXml writeXml = new WriteXml(cSharpClassesList);
            writeXml.writeToFile("model.xml");

            ReadXml readXml = new ReadXml();
            ArrayList model2 = readXml.readModelFromFile("model.xml");

            WriteXml writeXml2 = new WriteXml(model2);
            writeXml2.writeToFile("model2.xml");
        }

        private void Vertex_MouseDown(object sender, RoutedEventArgs e)
        {
            // ContentPresenter contentPresenter = (ContentPresenter)sender;
            // string className = contentPresenter.Content.ToString();
            // int end = className.IndexOf("\r\n");
            // string name =  className.Substring(0,end);
            // int t = 0;
            // var g = new BidirectionalGraph<object, IEdge<object>>();
            // add the vertices


            // gUt.add();

        }


        private void Load_Context_Click(object sender, RoutedEventArgs e)
        {
            ReadXml readXml = new ReadXml();
            CSharpClass classNumber = readXml.readClassFromFiles("Class5", "model.xml");
            classNumber.ToString();

        }

    }
}