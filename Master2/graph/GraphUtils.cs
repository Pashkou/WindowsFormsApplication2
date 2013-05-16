using Company.Master2.graph;
using Microsoft.master2.contextmodel;
using Microsoft.master2.model;
using Microsoft.master2.rules;
using Microsoft.Master2.assembly;
using QuickGraph;
using Relations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Microsoft.master2.graph
{
    public class GraphUtils
    {
        BidirectionalGraph<object, IEdge<object>> g = new BidirectionalGraph<object, IEdge<object>>();
        public ArrayList xmlModel = null;
        Image methodSelectedImage = null;
       public  ArrayList cSharpModel = null;
        private void initSelectedMethodImage(){

        }
        public IBidirectionalGraph<object, IEdge<object>> prepareGraph()
        {
            return g;
        }

        private ArrayList graphEntities = new ArrayList();


        private int edgesCount = 0;
        public void Clear(ArrayList entitiesModel)
        {

            foreach (MyVertexBase entity in entitiesModel)
            {
                if (g.EdgeCount > 0)
                {
                    edgesCount = g.EdgeCount;
                    g.ClearEdges(entity);
                }
                if (g.VertexCount > 0)
                {
                    g.RemoveVertex(entity);
                }
            }
        }

        MyVertexBase[] entitiesToDisplayModel;

        public ArrayList updateProActiveNavigationWithGraphWithRefresh(CSharpClass cSharpClass, ArrayList proActiveModel, GraphLayout gL)
        {

           
            MyVertexBaseVerySmall[] entitiesToDisplayModel = new MyVertexBaseVerySmall[proActiveModel.Count];
            int i = 0;
            foreach (MyVertexBaseVerySmall currentEntity in proActiveModel)
            {
                string methodSelectedPath = "Resources/methodSelected.jpg";
                BitmapImage methodBitmap = new BitmapImage(new Uri(methodSelectedPath, UriKind.Relative));
                methodSelectedImage = new Image();
                methodSelectedImage.Source = methodBitmap;
                entitiesToDisplayModel[i] = new MyVertexBaseVerySmall();
                entitiesToDisplayModel[i].Name1 = currentEntity.Name1;
                entitiesToDisplayModel[i].Name2 = currentEntity.Name2;
                entitiesToDisplayModel[i].Image2 = methodSelectedImage;
                entitiesToDisplayModel[i].Relevance = currentEntity.Relevance;
                i++;
            }
            
            //sort according to relevance
            //what is relevance for navigation is to decide
           // MyVertexBase[] arrayEntityies = (MyVertexBase[])entitiesToDisplayModel.ToArray();

            entitiesToDisplayModel = updateCoordinatesProActiveNavigation(cSharpClass, entitiesToDisplayModel, gL);
            ArrayList result = new ArrayList();
            foreach (MyVertexBase entity in entitiesToDisplayModel)
            {
                result.Add(entity);
                if (!Constant.AUTOTEST)
                {
                    gL.Graph.AddVertex(entity);
                }
            }

            return result;

        }
        private CSharpClass[] sortThisArrayClasses(ArrayList thisProActiveModel)
        {
            CSharpClass[] classes = new CSharpClass[thisProActiveModel.ToArray().Length];
            int newNumber = 0;
            foreach(CSharpClass thisClass in thisProActiveModel){
                classes[newNumber] = thisClass;
                newNumber++;
            }
            int n = classes.Length;
            CSharpClass[] newVertexArray = new CSharpClass[n];
            CSharpClass temp;

            for (int k = n - 1; k > 0; k--)
                for (int i = 0; i < k; i++)

                    if (classes[i].NumerOfEdges > classes[i + 1].NumerOfEdges)
                    {
                        temp = classes[i];
                        classes[i] = classes[i + 1];
                        classes[i + 1] = temp;
                    }
            return classes;
        }
    
        public ArrayList updateProActiveNavigationWithGraph(CSharpClass cSharpClass, ArrayList proActiveModel, GraphLayout gL)
        {

            CSharpClass[] classes = sortThisArrayClasses(proActiveModel);

            MyVertexBase[] entitiesToDisplayModel = new MyVertexBase[proActiveModel.Count];
            int threshold = 0;
            if (entitiesToDisplayModel.Length > 3)
            {
                threshold = entitiesToDisplayModel.Length - 3;
            }
            for (int i = classes.Length - 1; i >= threshold; i--)
            {
                entitiesToDisplayModel[i] = new MyVertexBaseSmall();
                entitiesToDisplayModel[i].CSharpClasses.Add(classes[i]);
                entitiesToDisplayModel[i].prepareToDisplay();


            }
            
            //sort according to relevance
            //what is relevance for navigation is to decide
            // MyVertexBase[] arrayEntityies = (MyVertexBase[])entitiesToDisplayModel.ToArray();

            //TODO 2
            updateCoordinatesProActiveNavigationWithoutRefresh(cSharpClass, entitiesToDisplayModel, gL);
            //entitiesToDisplayModel = updateCoordinatesProActiveNavigation(cSharpClass, entitiesToDisplayModel, gL);
            ArrayList result = new ArrayList();
           
            
            for (int i = entitiesToDisplayModel.Length - 1; i >= threshold; i--)
            {
                //TODO
                result.Add(entitiesToDisplayModel[i]);
                if (!Constant.AUTOTEST)
                {
                    gL.Graph.AddVertex(entitiesToDisplayModel[i]);
                }
            }

      
            //
            return result;

        }

        public void updateCoordinatesProActiveNavigationWithoutRefresh(CSharpClass cSharpClass, MyVertexBase[] vertices, GraphLayout gL)
        {
            double lastCoordinateX = 400;
            double lastCoordinateY = 20;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i] != null)
                {
                    Coordinates coordinates = new Coordinates();
                    coordinates.x = lastCoordinateX;
                    coordinates.y = lastCoordinateY;
                    Coordinates.hashMap.Add(vertices[i].ToString(), coordinates);
                    lastCoordinateY += 150;
                }
            }
          
        }

        public ArrayList updateVertexWithGraph(CSharpClass cSharpClass, ArrayList entitiesModel, GraphLayout gL, ArrayList entitiesToDisplayModel)
        {
            initSelectedMethodImage();
            MyGraph g = new MyGraph();
            entitiesToDisplayModel = new ArrayList();
            entitiesModel = updateCoordinates(cSharpClass, entitiesModel);

            int displayElementInThePast = 0; ;
            int i = 0;
            foreach (MyVertexBase entity in entitiesModel)
            {
                MyVertexBase entityToAdd = null;
                if (entity.CSharpClasses.Count == 1)
                {
                    if (cSharpClass.Equals( entity.CSharpClasses.ToArray()[0] ) )
                    {
                        entityToAdd = new MyVertexBaseMiddle();
                     //   entityToAdd = new MyVertexBaseSmall();
                    }
                    else if (!entity.toAggregate)
                    {
                        switch (entity.displayElementInThePast)
                        {
                            case 0: entityToAdd = new MyVertexBaseSmallFirst(); break;
                            case 1: entityToAdd = new MyVertexBaseSmallSecond(); break;
                            case 2: entityToAdd = new MyVertexBaseSmallThird(); break;
                        }
                        displayElementInThePast++;
                        //entityToAdd = new MyVertexBaseSmall();
                    }
                    else
                    {
                        entityToAdd = new MyVertexBaseVerySmall();
                    }
                }
                else  {
                    if (entity.CSharpClasses.Count > 2)
                    {
                        entityToAdd = new MyVertexBaseSmall();
                    }
                    else {
                        entityToAdd = new MyVertexBaseVerySmall();
                    }
                }
                entityToAdd.CommonThreshold = entity.CommonThreshold;
                entityToAdd.CSharpClasses = entity.CSharpClasses;
                entityToAdd.NumberInThePast = entity.NumberInThePast;
                entityToAdd.SortedNumberInThePast = entity.SortedNumberInThePast;
                entityToAdd.IsThirdColumn = entity.IsThirdColumn;
                entityToAdd.prepareToDisplay();
                entitiesToDisplayModel.Add(entityToAdd);
                i++;
            }


            //commented?
            if ((cSharpClass.SelectedMethod != null) && (cSharpClass.SelectedMethod != ""))
            {
                entitiesToDisplayModel = highlithStructuralDependencies(entitiesToDisplayModel, cSharpClass.SelectedMethod);
            }

            foreach (MyVertexBase entity in entitiesToDisplayModel)
            {
                if (!Constant.AUTOTEST)
                {
                    g.AddVertex(entity);
                }
            }

            MyVertexBase absolute = new MyVertexBaseAbsolute();
            absolute.Name1 = "absolute";
            g.AddVertex(absolute);



            Coordinates abs = new Coordinates();
            abs.x = -270;
            abs.y = -70;
            Coordinates.hashMap.Add(absolute.ToString(), abs);

            if (!Constant.AUTOTEST)
            {
                gL.Graph = g;
            }
            return entitiesToDisplayModel;
        }
        string selectedFunction = "";
        public ArrayList highlithStructuralDependencies(ArrayList entToDisplay, string selectedMethod) {
            ArrayList result = new ArrayList();
            selectedFunction = "";
            foreach (MyVertexBase myVertexBase in entToDisplay)
            {
                result.Add( setSelectedMethodToEleement(myVertexBase, selectedMethod));
            }
            secondLevelConnection = new ArrayList();
            ArrayList newResult = new ArrayList();
            if (selectedFunction != "")
            {
                foreach (MyVertexBase myVertexBase in entToDisplay)
                {
                    MyVertexBase toAdd = highlithSecondLevel(myVertexBase);
                    if (toAdd != null)
                    {
                        newResult.Add(toAdd);
                    }
                }
            }

            foreach (MyVertexBase myVertexBase in entToDisplay)
            {

                MyVertexBase toAdd = highlithThirdLevel(myVertexBase);
                if (toAdd != null)
                {
                    newResult.Add(toAdd);
                }
            }

            return newResult;
        }

        private MyVertexBase highlithThirdLevel(MyVertexBase myVertexBase)
        {
            string methodSelectedPath = "Resources/methodSelected.jpg";
            BitmapImage methodBitmap = new BitmapImage(new Uri(methodSelectedPath, UriKind.Relative));
            Image methodSelectedImage = new Image();
            MyVertexBaseVerySmall result = null;

            if (myVertexBase.GetType() == typeof(MyVertexBaseVerySmall))
            {
                string selectedClass = myVertexBase.Name1;
                result = (MyVertexBaseVerySmall)myVertexBase;
                result = isEntityContainsStructuralDependencieThird(selectedClass, result);
            }

            return result;

        }
      

        private CSharpClass getClassBYName(String selectedClass) {
            foreach (CSharpClass cSharpClass in xmlModel)
            {
                if (cSharpClass.Name == selectedClass)
                {
                    return cSharpClass;
                }
            }
            return null;

        }

        private MyVertexBase highlithSecondLevel(MyVertexBase myVertexBase)
        {
            string methodSelectedPath = "Resources/methodSelected.jpg";
            BitmapImage methodBitmap = new BitmapImage(new Uri(methodSelectedPath, UriKind.Relative));
            Image methodSelectedImage = new Image();

            methodSelectedImage.Source = methodBitmap;
            MyVertexBaseSmall result = null;
            if ((myVertexBase.GetType() == typeof(MyVertexBaseSmall))||
                (myVertexBase.GetType() == typeof(MyVertexBaseSmallFirst))||
            (myVertexBase.GetType() == typeof(MyVertexBaseSmallSecond))||
                (myVertexBase.GetType() == typeof(MyVertexBaseSmallThird))) {
                string selectedClass = myVertexBase.Name1;
                result = (MyVertexBaseSmall)myVertexBase;
                result = isEntityContainsStructuralDependencie(selectedClass, result);
            }
            if ((result == null)&&( myVertexBase.GetType() != typeof(MyVertexBaseVerySmall)))
            {
                return myVertexBase;
            }
            return result;
        }

        ArrayList secondLevelConnection = new ArrayList();
        private MyVertexBaseSmall isEntityContainsStructuralDependencie(string selectedClass, MyVertexBaseSmall myVertexBase)
        {
            string methodSelectedPath = "Resources/methodSelected.jpg";
            BitmapImage methodBitmap = new BitmapImage(new Uri(methodSelectedPath, UriKind.Relative));
            methodSelectedImage = new Image();
            methodSelectedImage.Source = methodBitmap;
            foreach (CSharpClass cSharpClass in xmlModel)
            {
                if (cSharpClass.Name == selectedClass)
                {
                    ArrayList methods = cSharpClass.Methods;
                    foreach (CSharpMethod cSharpMethod in methods)
                    {                        
                        if ((cSharpMethod.Name == myVertexBase.Name2))
                        {
                            ArrayList outgoingCalls = cSharpMethod.OutgoingCalls;
                            foreach (MethodTypeCall outgoingCall in outgoingCalls)
                            {
                                if ((outgoingCall.Name == selectedFunction)&& (outgoingCall.ClassName == selectedClassName))
                                {
                                    myVertexBase.Image2 = methodSelectedImage;
                                    myVertexBase.Name2Bold = myVertexBase.Name2;
                                    myVertexBase.Name2 = "";
                                    secondLevelConnection.Add(cSharpMethod.Name);
                                    selectedFunction = cSharpMethod.Name;
                                    return myVertexBase;
                                }
                            }
                        }
                        if ((cSharpMethod.Name == myVertexBase.Name3))
                        {
                            ArrayList outgoingCalls = cSharpMethod.OutgoingCalls;
                            foreach (MethodTypeCall outgoingCall in outgoingCalls)
                            {
                             if    ((outgoingCall.Name == selectedFunction) && (outgoingCall.ClassName == selectedClassName))
                                {
                                    myVertexBase.Image3 = methodSelectedImage;
                                    myVertexBase.Name3Bold = myVertexBase.Name3;
                                    myVertexBase.Name3 = "";
                                    secondLevelConnection.Add(cSharpMethod.Name);
                                    selectedFunction = cSharpMethod.Name;
                                    return myVertexBase;
                                }
                            }
                        }
                        if ((cSharpMethod.Name == myVertexBase.Name4))
                        {
                            ArrayList outgoingCalls = cSharpMethod.OutgoingCalls;
                            foreach (MethodTypeCall outgoingCall in outgoingCalls)
                            {






                                if ((outgoingCall.Name == selectedFunction)&& (outgoingCall.ClassName == selectedClassName))
                                {
                                    myVertexBase.Image4 = methodSelectedImage;
                                    myVertexBase.Name4Bold = myVertexBase.Name4;
                                    myVertexBase.Name4 = "";
                                    secondLevelConnection.Add(cSharpMethod.Name);
                                    selectedFunction = cSharpMethod.Name;
                                    return myVertexBase;
                                }


                            }
                        }


                            ArrayList outgoingCalls2 = cSharpMethod.OutgoingCalls;
                            foreach (MethodTypeCall outgoingCall in outgoingCalls2)
                            {
                                if ((outgoingCall.Name == selectedFunction) && (outgoingCall.ClassName == selectedClassName))
                                {
                                    myVertexBase.Name2 = cSharpMethod.Name;
                                    myVertexBase.Image2 = methodSelectedImage;
                                    myVertexBase.Name2Bold = myVertexBase.Name2;
                                    myVertexBase.Name2 = "";
                                    secondLevelConnection.Add(cSharpMethod.Name);
                                    selectedFunction = cSharpMethod.Name;
                                   cSharpModel = Heuristics.updateRelevanceForSelect(cSharpModel, selectedClass, selectedFunction);
                                    return myVertexBase;
                                }
                            }
                        //
                    }
                }
            }
            return null;
        }

        private MyVertexBaseVerySmall isEntityContainsStructuralDependencieThird(string selectedClass, MyVertexBaseVerySmall myVertexBase)
        {
            string methodSelectedPath = "Resources/methodSelected.jpg";
            BitmapImage methodBitmap = new BitmapImage(new Uri(methodSelectedPath, UriKind.Relative));
            methodSelectedImage = new Image();
            methodSelectedImage.Source = methodBitmap;
             foreach (CSharpClass cSharpClass in xmlModel)
            {
                if (cSharpClass.Name == myVertexBase.Name1){
                     ArrayList methods = cSharpClass.Methods;
                     foreach (CSharpMethod cSharpMethod in methods)
                     {
                         ArrayList outgoingCalls = cSharpMethod.OutgoingCalls;
                         foreach (MethodTypeCall outgoingCall in outgoingCalls)
                         {
                             if ((outgoingCall.Name == selectedFunction))
                             {
                                 myVertexBase.Image2 = methodSelectedImage;
                                 myVertexBase.Name2Bold = myVertexBase.Name2;
                                 myVertexBase.Name2 = "";
  
                                 secondLevelConnection.Add(cSharpMethod.Name);
                                 return myVertexBase;
                             }
                         }
                     }
                }
            }


             return myVertexBase;
        }
        string selectedClassName;
        private MyVertexBase setSelectedMethodToEleement(MyVertexBase myVertexBase, string selectedMethod)
        {
            string methodSelectedPath = "Resources/methodSelected.jpg";
            BitmapImage methodBitmap = new BitmapImage(new Uri(methodSelectedPath, UriKind.Relative));
            methodSelectedImage = new Image();
            methodSelectedImage.Source = methodBitmap;

            if (myVertexBase.GetType() == typeof(MyVertexBaseMiddle))
            {
                myVertexBase = (MyVertexBaseMiddle)myVertexBase;
                if (myVertexBase.Name2 == selectedMethod)
                {
                    myVertexBase.Image2 = methodSelectedImage;
                    myVertexBase.Name2Bold = myVertexBase.Name2;
                    myVertexBase.Name2 = "";
                    selectedFunction = selectedMethod;
                    selectedClassName = myVertexBase.Name1;
                }
                if (myVertexBase.Name3 == selectedMethod)
                {
                    myVertexBase.Image3 = methodSelectedImage;
                    myVertexBase.Name3Bold = myVertexBase.Name3;
                    myVertexBase.Name3 = "";
                    selectedFunction = selectedMethod;
                    selectedClassName = myVertexBase.Name1;
                }
                if (myVertexBase.Name4 == selectedMethod)
                {
                    myVertexBase.Image4 = methodSelectedImage;
                    myVertexBase.Name4Bold = myVertexBase.Name4;
                    myVertexBase.Name4 = "";
                    selectedFunction = selectedMethod;
                    selectedClassName = myVertexBase.Name1;
                }
            }
            return myVertexBase;
        }

        public void updateVertex(ArrayList entitiesModel)
        {
            int i = 100;

            foreach (MyVertexBase entity in entitiesModel)
            {
                g.AddVertex(entity);
            }
        }

        ArrayList edges = new ArrayList();
        public void updateEdgesWithGraph(ArrayList entitiesModel, GraphLayout gL)
        {
            MyGraph newG = gL.Graph;
            IEnumerable vertices = newG.Vertices;
            foreach (MyVertexBase vertic in vertices)
            {
                ArrayList edges = vertic.Edges;
                foreach (EntityEdge edge in edges)
                {
                    string entityNameToConnect = edge.Target;
                    foreach (MyVertexBase verticToConnect in vertices)
                    {
                        if (verticToConnect.ToString() == edge.Target)
                        {
                            chechAndAddEntitiesToDisplay(vertic, verticToConnect, newG, edge);
                        }
                    }
                }
            }
        }

        private void edgeAddedEvent() {
            int i = 0;
        }
        private void chechAndAddEntitiesToDisplay(MyVertexBase vertic, MyVertexBase verticToConnect, MyGraph newG, EntityEdge edge)
        {

            if (vertic.CSharpClasses.ToArray().Length == 1)
            {
                if (((CSharpClass)vertic.CSharpClasses.ToArray()[0]).IsSelected)
                {
                    if (!verticToConnect.IsThirdColumn)
                    {
                        newG.AddEdge(new MyEdgeBase(vertic, verticToConnect, edge.ToString()));

                    }
                }
                else if (vertic.IsThirdColumn)
                {
                    if (!((CSharpClass)verticToConnect.CSharpClasses.ToArray()[0]).IsSelected)
                    {
                        newG.AddEdge(new MyEdgeBase(vertic, verticToConnect, edge.ToString()));
                    }
                }
                else
                {
                    newG.AddEdge(new MyEdgeBase(vertic, verticToConnect, edge.ToString()));
                }
            }
            else {
                if (!((CSharpClass)vertic.CSharpClasses.ToArray()[0]).IsSelected) {
                    newG.AddEdge(new MyEdgeBase(vertic, verticToConnect, edge.ToString()));
                }
            }
        }

        public void addProActiveEdge(MyVertexBase vertic, MyVertexBase verticToConnect, MyGraph newG)
        {

          
           newG.AddEdge(new MyEdgeBase(vertic, verticToConnect, ""));
                   
        }

        private MyVertexBase[] sortTheNumberInThePast(ArrayList vertices)
        {
            MyVertexBase[] arrayOfVertex = new MyVertexBase[vertices.Count];
            foreach (MyVertexBase vertic in vertices)
            {
                int lastNumber = 0;
                foreach (MyVertexBase verticToCompare in vertices)
                {
                    if (vertic.NumberInThePast > verticToCompare.NumberInThePast) {
                        lastNumber++;
                    }
                }
                vertic.SortedNumberInThePast = lastNumber;
                arrayOfVertex[lastNumber] = vertic;
            }
            return arrayOfVertex;
        }

        public MyVertexBaseVerySmall[] updateCoordinatesProActiveNavigation(CSharpClass cSharpClass, MyVertexBaseVerySmall[] vertices, GraphLayout gL)
        {
            MyVertexBaseVerySmall[] result;
            if (vertices.Length >= 3)
            {
                result = new MyVertexBaseVerySmall[3];
            }
            else {
                result = new MyVertexBaseVerySmall[vertices.Length];
            }

            double lastCoordinateX = 400;
            double lastCoordinateY = 20;
            sortThisArray(vertices);

            int k = 0;
            
            for (int i = vertices.Length - 1; i >= 0; i--)
            {
                if (vertices[i] != null)
                {
                    Coordinates coordinates = new Coordinates();
                    coordinates.x = lastCoordinateX;
                    coordinates.y = lastCoordinateY;
                    Coordinates.hashMap.Add(vertices[i].ToString(), coordinates);
                    lastCoordinateY += 100;
                }
                if (k < 3) {
                    result[k] = vertices[i];
                    k++;
                }
            }
            
            //save sorted verticels
            savedVerticels = vertices;

            return result;
        }
        MyVertexBaseVerySmall[] savedVerticels;

        public ArrayList getTopElementsOfProactiveNavigation(ArrayList elements) {
            ArrayList result = new ArrayList();
            if (savedVerticels.Length > 3)
            {
                for (int i = savedVerticels.Length - 1; i >= savedVerticels.Length - 3; i--)
                {
                    
                }
            }
            return null;
        }

        private void sortThisArray(MyVertexBaseVerySmall[] vertices)

        {
 
            int n = vertices.Length;
            MyVertexBaseVerySmall[] newVertexArray = new MyVertexBaseVerySmall[n];
            MyVertexBaseVerySmall temp;

            for (int k = n - 1; k > 0; k--)
                for (int i = 0; i < k; i++)

                    if (vertices[i].Relevance > vertices[i+1].Relevance)
                    {
                        temp = vertices[i];
                        vertices[i] = vertices[i+1];
                        vertices[i + 1] = temp;
                    }
        }

        private MyVertexBase setIt(MyVertexBase myVertexBase, int displayElementInThePast)
        {
            switch (displayElementInThePast)
            {
                case 0: myVertexBase = new MyVertexBaseSmallFirst(); break;
                case 1: myVertexBase = new MyVertexBaseSmallSecond(); break;
                case 2: myVertexBase = new MyVertexBaseSmallThird(); break;
            }
            displayElementInThePast++;
            return myVertexBase;
        }

        public ArrayList updateCoordinates(CSharpClass cSharpClass, ArrayList verticesList)
        {
            double lastCoordinateX = 0;
            double lastCoordinateY = 0;

   
            Coordinates.hashMap = new Dictionary<string, Coordinates>();
            int sumOfVertex = 0;
            int sizeOfVertex = 0;
            int nextLineCount = 2;
            int displayElementInThePast = 1;
            foreach (MyVertexBase vertices in verticesList)
            {
                if (!vertices.IsCoordinateCalculated)
                {
                    Coordinates coordinates = new Coordinates();
                    if (vertices.CSharpClasses.Contains(cSharpClass))
                    {
                        //set first element position
                        coordinates.x = 200;
                        coordinates.y = 120;
                        Coordinates.hashMap.Add(vertices.ToString(), coordinates);
                        vertices.IsCoordinateCalculated = true;
                    }
                    else if (((CSharpClass)vertices.CSharpClasses[0]).IsSecondSelected)
                    {
                        //set second element position 
                        coordinates.x = 35;
                        coordinates.y = 40;
                        lastCoordinateX = coordinates.x;
                        lastCoordinateY = coordinates.y;
                        sumOfVertex = 1;
                        sizeOfVertex = 160;
                        Coordinates.hashMap.Add(vertices.ToString(), coordinates);
                        vertices.IsCoordinateCalculated = true;
                        vertices.displayElementInThePast = 0;

                    }

                }
            }
            foreach (MyVertexBase vertices in verticesList)
            {
                if (!vertices.IsCoordinateCalculated)
                {
                    Coordinates coordinates = new Coordinates();

                        if ((sumOfVertex > nextLineCount))
                        {
                            sizeOfVertex = 0;
                            lastCoordinateX = lastCoordinateX - 200;
                            nextLineCount += 4;

                       
                        }
                        if (nextLineCount > 5) {
                            vertices.toAggregate = true;

                            vertices.IsThirdColumn = true;
                           
                       
                        }
                        if (displayElementInThePast < 3)
                        {
                            vertices.displayElementInThePast = displayElementInThePast;
                            displayElementInThePast++;
                        }
                        coordinates.x = lastCoordinateX;
                        coordinates.y = sizeOfVertex;
                        lastCoordinateX = coordinates.x;
                        sumOfVertex += 1;
                        vertices.IsCoordinateCalculated = true;
                        sizeOfVertex = 125 + sizeOfVertex;
                        Coordinates.hashMap.Add(vertices.ToString(), coordinates);
               }
            }

            return verticesList;
        }





        public void updateEdges(ArrayList entitiesModel)
        {

            IEnumerable vertices = g.Vertices;
            foreach (MyVertexBase vertic in vertices)
            {
                ArrayList edges = vertic.Edges;
                foreach (EntityEdge edge in edges)
                {
                    string entityNameToConnect = edge.Target;
                    foreach (MyVertexBase verticToConnect in vertices)
                    {
                        if (verticToConnect.ToString() == edge.Target)
                        {
                            g.AddEdge(new TaggedEdge<object, object>(verticToConnect, vertic, edge.ToString()));
                            //        g.addEdge(new MyEdge<object, object>(verticToConnect, vertic, edge.Weight.ToString()));

                        }

                    }
                }
            }
        }

        public void add(CSharpClass cSharpClass)
        {

            IEnumerable vertices = g.Vertices;
            bool removed = false;
            foreach (CSharpClass vertic in vertices)
            {
                if (vertic.Name == cSharpClass.Name)
                {
                    g.RemoveVertex(vertic);
                    removed = true;

                }
            }

            if (!removed)
            {
                g.AddVertex(cSharpClass);
            }

        }
    }
}
