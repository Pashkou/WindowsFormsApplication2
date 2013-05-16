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
using Microsoft.master2.Utils;
using TinyMessenger;
using Microsoft.master2.messagebus;
using Microsoft.master2.rules;
using GraphSharp.Controls;
using Company.Master2;
using Microsoft.master2.contextmodel;
using Company.Master2.messagebus;
using Microsoft.master.command;
using Company.Master2.graph;
using Company.Master2.cursorcodeelement;
using Company.Master2.events;
using RamGecTools;
using System.Runtime.InteropServices.ComTypes;
using GalaSoft.MvvmLight;
using Microsoft.Master2.Utils;
using Microsoft.Master2.excel;


namespace Relations.Views
{

    public class MapViewerViewModel : ViewModelBase
    {


        public MapViewerViewModel()
        {

            //GraphToVisualize = new MyGraph();            
        }

        private MyGraph _graph;

        public MyGraph GraphToVisualize
        {
            get { return _graph; }
            set { _graph = value; RaisePropertyChanged("GraphToVisualize"); }
        }

        public void AddVertex(Object objectToAdd)
        {
            // GraphToVisualize.AddVertex(new MyVertexBase() { Name = objectToAdd.ToString() });
        }

    }
    public partial class MapViewerControl : UserControl
    {
        private IBidirectionalGraph<object, IEdge<object>> _graphToVisualize;
        public IBidirectionalGraph<object, IEdge<object>> GraphToVisualize
        {
            get { return _graphToVisualize; }
        }

        private GraphUtils gUt;


        private EnvDTE80.TextDocumentKeyPressEvents textDocKeyEvents;
        private EnvDTE.TextEditorEvents _textEditorEvents;

        public void OnSelectionChanged()
        {
            int t = 0;

        }
        public void TextEditorEvents_LineChanged()
        {
            int r = 0;
        }

        private VSEvents vsEvents;

        void BeforeKeyPress(string Keypress, EnvDTE.TextSelection Selection, bool InStatementCompletion, ref bool CancelKeypress)
        {
            int t = 0;
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (model == null)
            {
                prepareModel();
            }
            if (!ruleEngine.hasModel())
            {
                ruleEngine.setModel(model);
            }
            string fileName = "e://transcripts//transcription#262.xls";
            string listname = "Tabelle1";


            ReadExcel eExcel = new ReadExcel(ruleEngine, graphLayout, model, readFile());
            eExcel.ReadFile(fileName, listname);
        }

        private ArrayList readFile() {
            ArrayList result = new ArrayList();
            try
            {
                StreamReader sr = new StreamReader("E:\\CSharp\\pwsafe\\J3.txt");
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    result.Add(line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return result;
        }
        private void Create_Model_Click(object sender, RoutedEventArgs e)
        {
            //prepareModel();



            ArrayList methods = currentCSharpClass.Methods;
            foreach(CSharpMethod cSharpMethod in methods){
                cSharpMethod.Relevance = 0.1;
                break;
            }

            ruleEngine.classSelectedWithGrpahRefreshButton(currentCSharpClass, graphLayout, true);
        }
        private MyVertexBase selectedEntity = null;
        CSharpClass currentCSharpClass = null;

        private void Refresh_Model_Click(object sender, RoutedEventArgs e)
        {

            /*string methodName = textBox1.Text;

            CSharpMethod cSharpMethod = new CSharpMethod();
            cSharpMethod.Name = methodName;
            cSharpMethod.Relevance = 0.1;
            currentCSharpClass.SelectedMethod = methodName;
            */
            //    CSharpClass classToUpdate = new CSharpClass();
            //   classToUpdate.Name = currentCSharpClass.Name;
            //  classToUpdate.Methods.Add(cSharpMethod);
            ruleEngine.classSelectedWithGrpahRefreshButton(currentCSharpClass, graphLayout, true);
            //     CSharpClass cSharpClass = getCurrentClassFormModel(getLastPartOfName(currentClassName));// readXml.readClassFromFiles(currentClass.Name, "model.xml");
            //currentClass.GetMethods


            //if (selectedEntity != null)
            //{
               // selectedEntity.CommonThreshold = int.Parse(textBox1.Text);
              //  EntityMessage entityMessage = new EntityMessage();
               // entityMessage.Entity = selectedEntity;
                //ruleEngine.updateEntityWithGrpah(selectedEntity, graphLayout);
                //  messageHub.Publish(entityMessage);
            //}
            //positionNumber = 0;
        }

        private string lastClassName = "";

        RuleEngine ruleEngine;
        private TinyMessengerHub messageHub = new TinyMessengerHub();
        private DTE dte;
        private DTE2 dte2;
        CodeElementAtCursor codeElementAtCursor;

        MapViewerViewModel viewModel;
        public MapViewerControl()
        {
            viewModel = new MapViewerViewModel();
            InitializeComponent();
            gUt = new GraphUtils();
            InitializeComponent();
            dte = Master2Package.GetGlobalService(typeof(DTE)) as DTE;
            dte2 = Master2Package.GetGlobalService(typeof(DTE)) as DTE2;

            dte.Events.WindowEvents.WindowActivated += WindowEvents_WindowActivated;
            ruleEngine = new RuleEngine(gUt, graphLayout);
            ruleEngine.dte = dte;

            Loaded += new RoutedEventHandler(MapViewerControl_Loaded);
            codeElementAtCursor = new CodeElementAtCursor(dte, messageHub, graphLayout, ruleEngine);
            InterceptMouse.Main(codeElementAtCursor, positionNumber);

        }
        public CLassName currentClassA = null;



        public String getCurrentElement(FileCodeModel fileCM)
        {
            currentClassA = null;
            InvestigateElement invElem = new InvestigateElement();
            if (fileCM != null)
            {
                CodeElements elts = null;
                elts = fileCM.CodeElements;
                CodeElement elt = null;
                int i = 0;
                for (i = 1; i <= fileCM.CodeElements.Count; i++)
                {
                    elt = elts.Item(i);
                    
                    CollapseElt(elt, elts, i);
                }
                // currentClass = invElem.currentClass;
            }
            return currentClassA.name;
        }


        //check class at runtime
        private string CollapseElt(CodeElement elt, CodeElements elts, long loc)
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
                if (currentClassA == null)
                {
                    currentClassA = new CLassName();
                    currentClassA.name = elt.FullName.ToString();
                }
                return elt.FullName;

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
                    CollapseElt(mems_vb.Item(i), mems_vb, i);
                }
            }
            return result;
        }

        Document startDocument = null;
       
        private void WindowEvents_WindowActivated(EnvDTE.Window GotFocus, EnvDTE.Window LostFocus)
        {
            if ((dte.ActiveDocument != null) && (dte.ActiveDocument.ProjectItem != null))
            {
                if (startDocument == null) {
                    startDocument = dte.ActiveDocument;
                }
                FileCodeModel fileCM = dte.ActiveDocument.ProjectItem.FileCodeModel;
                InvestigateElement invElem = new InvestigateElement();
                //read class from user navigation
                String currentClassName = null;
                currentClassName = getCurrentElement(fileCM);
                if (model == null)
                {
                    prepareModel();
                    vsEvents = new VSEvents(dte, dte2, messageHub, model, ruleEngine, graphLayout);
                }
                if (!ruleEngine.hasModel()) {
                    ruleEngine.setModel(model);
                }
                if ((currentClassName != null) && (currentClassName != lastClassName))
                {
                    lastClassName = currentClassName;
                    //read xml model of selected class 
                    try
                    {
                        currentCSharpClass = getCurrentClassFormModel(getLastPartOfName(currentClassName));// readXml.readClassFromFiles(currentClass.Name, "model.xml");
                        ClassMessage classMessage = new ClassMessage();
                        classMessage.CSharpClass = currentCSharpClass;
                        // messageHub.Publish(classMessage);
                        // ruleEngine.classSelected(cSharpClass);
                        ruleEngine.classSelectedWithGrpah(currentCSharpClass, graphLayout, false);
                    }
                    catch (Exception e)
                    {
                        int i = 0;
                    }
                }
            }
            positionNumber = 0;
        }

        private string getLastPartOfName(string name) {
            string[] st = name.Split('.');
     
            return st[st.Length-1];
        }

        private ArrayList  prepareModel()
        {
            string modelXmlfile = "classesModel.xml";
            ModelUtils mUtils = new ModelUtils(currentClass);
           /*
            it is possible here to decise how to read the model 
            * from generated XML or from exe 
            */
            
             model = mUtils.getModelForCurrentAssembly();
           //   ReadXml readXml = new ReadXml();
            //model = readXml.readModelFromFile(modelXmlfile);

            
            //WriteXml writeXml = new WriteXml(model);
            //writeXml.writeToFile(modelXmlfile);
            return model;
        }

        private CSharpClass getCurrentClassFormModel(string className)
        {
            if (model == null)
            {
                return null;
            }
            foreach (CSharpClass cSharpClass in model)
            {
                if (cSharpClass.Name == className)
                {
                    return cSharpClass;
                }
            }
            return null;
        }

        private Type currentClass = null;

        private ArrayList model = null;



        void MapViewerControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = viewModel;
        }

        private void DropHandler(object sender, DragEventArgs e)
        {
            Object objToAdd = e.Data.GetData(typeof(Object)) as Object;
            if (objToAdd != null)
            {
                viewModel.AddVertex(objToAdd);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null && menuItem.Tag != null)
            {
                var vertex = menuItem.Tag as MyVertexBase;
                if (vertex != null)
                {
                    MessageBox.Show("You want to link ");
                }
            }
        }

        private void Vertex_MouseDown(object sender, RoutedEventArgs e)
        {
            ContentPresenter contentPresenter = (ContentPresenter)sender;
            selectedEntity = (MyVertexBase)contentPresenter.Content;
            if (selectedEntity != null)
            {
                ArrayList classes = selectedEntity.CSharpClasses;

                foreach (CSharpClass cSharpClass in classes)
                {
                    dte.ExecuteCommand("File.OpenFile", cSharpClass.Name + ".cs");
                }
            }
        
            
        }


        Coordinates[] coordinates = Coordinates.getCoordinates();
        int positionNumber = 1;

        private void vertex_Loaded(object sender, RoutedEventArgs e)
        {
           

            VertexControl aux = new VertexControl();
            aux = ((VertexControl)sender);
          
            double posx = 0, posy = 0;

           
            string name = aux.Vertex.ToString();

            Coordinates position = null;
            if (Coordinates.hashMap.ContainsKey(name)) {
                position = Coordinates.hashMap[name];
            }
            if (position != null)
            {
                posx = position.x;
                posy = position.y;
           
            }
            GraphCanvas.SetX(aux, posx);
            GraphCanvas.SetY(aux, posy);

            e.Handled = true;
        }

        private void Load_Context_Click(object sender, RoutedEventArgs e)
        {

            // ReadXml readXml = new ReadXml();
            //ArrayList model = readXml.readModelFromFile("model.xml");
            //foreach(CSharpClass cSharpClass in model){
            //  CSharpClass.calculateEdges(cSharpClass);
            // }
        }
    }
}