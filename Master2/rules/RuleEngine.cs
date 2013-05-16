using EnvDTE;
using Microsoft.master2.assembly;
using Microsoft.master2.contextmodel;
using Microsoft.master2.graph;
using Microsoft.master2.model;
using Microsoft.Master2.assembly;
using Microsoft.Master2.command;
using Relations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.master2.rules
{
    public class RuleEngine
    {
        GraphUtils gUt;
        public DTE dte;
        public StructuralConnection currentStructuralConnection;
        public ArrayList xmlModel = null;
        public bool hasModel() {
            if (xmlModel == null)
            {
                return false;
            }
            else { return true; }
        }
        public void setModel(ArrayList model) {
            xmlModel = model;
        }
        public int positionNumber = 0;

        public RuleEngine(GraphUtils gUt, GraphLayout graphLayout)
        {
            this.gUt = gUt;
        }

        ArrayList cSharpClassModel = new ArrayList();
        ArrayList cSharpEntityModel = new ArrayList();
        ArrayList entitiesList = new ArrayList();

        public void updateEntity(MyVertexBase entity)
        {
            gUt.Clear(cSharpEntityModel);
            ArrayList cSharpClasses = entity.CSharpClasses;
            foreach (CSharpClass sCharpClassFromEntity in cSharpClasses)
            {
                sCharpClassFromEntity.Threshold = entity.CommonThreshold;
            }
            CSharpClass.resetModel(cSharpClassModel);
            updateGUI();
        }

        ArrayList entitiesToDisplayModel = new ArrayList();

        public void updateEntityWithGrpah(MyVertexBase entity, GraphLayout gL)
        {
            gUt.Clear(cSharpEntityModel);
            ArrayList cSharpClasses = entity.CSharpClasses;
            foreach (CSharpClass sCharpClassFromEntity in cSharpClasses)
            {
                sCharpClassFromEntity.Threshold = entity.CommonThreshold;
            }
            CSharpClass.resetModel(cSharpClassModel);
            positionNumber = 0;
            cSharpEntityModel = new ArrayList();
            updateEntities(null,false);
             //gUt.updateVertexWithGraph(null, cSharpEntityModel, gL, entitiesToDisplayModel);
         //   gUt.updateEdgesWithGraph(cSharpEntityModel, gL);
          //  gUt.updateCoordinates(null, gL);
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


        private void tryToGetSelectedElement(CSharpClass c)
        {
            try
            {
                /*EnvDTE.TextSelection doc = (EnvDTE.TextSelection)dte.ActiveDocument.Selection;
                EnvDTE.TextPoint textPoint = doc.ActivePoint;
                CodeElement el = dte.ActiveDocument.ProjectItem.FileCodeModel.CodeElementFromPoint(textPoint, EnvDTE.vsCMElement.vsCMElementFunction);
                string methodTo = el.FullName;
                CSharpClass c = prepareClassMethodsFromName(methodTo);*/
                currentStructuralConnection.classTo = c;
               // currentStructuralConnection.methodTo = methodTo;
                if(!structuralConnections.Contains(currentStructuralConnection)){
                    StructuralConnection sc= new StructuralConnection();
                    sc.classFrom = currentStructuralConnection.classFrom;
                    sc.classTo = currentStructuralConnection.classTo;

                    sc.methodFrom = currentStructuralConnection.methodFrom;
                   // sc.methodTo = currentStructuralConnection.methodTo;
                    structuralConnections.Add(sc);
                }
            }catch(Exception e){
                
            }
        }
        private CSharpClass lastClassSelected = null;

        public void updateRelevanceOfClass(CSharpClass cSharpClass)
        {
            cSharpClassModel = Heuristics.updateRelevance(cSharpClassModel, cSharpClass, false);
        }


       

        public ArrayList classSelectedWithGrpah(CSharpClass cSharpClass, GraphLayout gL, bool mouseEvent)
        {
            if (gUt.xmlModel == null) {
                gUt.xmlModel = xmlModel;
            }
            ArrayList result = new ArrayList();
            if ((mouseEvent) || (lastClassSelected == null) || (cSharpClass.Name != lastClassSelected.Name))
            {
                lastClassSelected = cSharpClass;
                gUt.Clear(cSharpEntityModel);
                cSharpClassModel = Heuristics.updateRelevance(cSharpClassModel, cSharpClass, mouseEvent);
                cSharpClassModel.Sort();
                CSharpClass.resetModel(cSharpClassModel);
                positionNumber = 0;
                cSharpEntityModel = new ArrayList();
                updateEntities(cSharpClass, false);
                tryToGetSelectedElement(cSharpClass);
                proActiveClassess = new ArrayList();


                updateProActiveNavigationWithoutRefresh(cSharpClass);
                gL.Graph = new MyGraph();

                entitiesToDisplayModel = gUt.updateVertexWithGraph(cSharpClass, cSharpEntityModel, gL, entitiesToDisplayModel);

                MyVertexBase selectedEntity = updateEdges(entitiesToDisplayModel);

                ArrayList proActiveNavigationEntities = gUt.updateProActiveNavigationWithGraph(cSharpClass, proActiveClassess, gL);
       
                addEdgesToProActiveEntities(gL.Graph, proActiveNavigationEntities, selectedEntity);

                result.Add(proActiveNavigationEntities);

                gUt.updateEdgesWithGraph(cSharpEntityModel, gL);
            }
            result.Add(entitiesToDisplayModel);
           
            return result;
         }

        public ArrayList classSelectedWithGrpahRefreshButton(CSharpClass cSharpClass, GraphLayout gL, bool mouseEvent)
        {
            ArrayList result = new ArrayList();
            if ((mouseEvent) || (lastClassSelected == null) || (cSharpClass.Name != lastClassSelected.Name))
            {
                lastClassSelected = cSharpClass;
                gUt.Clear(cSharpEntityModel);
                cSharpClassModel = Heuristics.updateRelevance(cSharpClassModel, cSharpClass, true);
                cSharpClassModel.Sort();
                gUt.cSharpModel = cSharpClassModel;
                CSharpClass.resetModel(cSharpClassModel);
                positionNumber = 0;
                cSharpEntityModel = new ArrayList();
                updateEntities(cSharpClass, true);
                tryToGetSelectedElement(cSharpClass);
                proActiveClassess = new ArrayList();
                ArrayList proActiveEntities = updateProActiveNavigationWithRefresh(cSharpClass);
                gL.Graph = new MyGraph();

                entitiesToDisplayModel = gUt.updateVertexWithGraph(cSharpClass, cSharpEntityModel, gL, entitiesToDisplayModel);

                MyVertexBase selectedEntity = updateEdges(entitiesToDisplayModel);

                ArrayList proActiveNavigationEntities = gUt.updateProActiveNavigationWithGraphWithRefresh(cSharpClass, proActiveEntities, gL);
                result.Add(proActiveNavigationEntities);
              
                addEdgesToProActiveEntities(gL.Graph, proActiveNavigationEntities, selectedEntity);
                gUt.updateEdgesWithGraph(cSharpEntityModel, gL);


            }
            result.Add(entitiesToDisplayModel);
            return result;
        }


        private void addEdgesToProActiveEntities(MyGraph myGraph, ArrayList proActiveNavigationEntities, MyVertexBase selectedEntity)
        {
            string[] protocol = new string[proActiveNavigationEntities.ToArray().Length];
            int j = 0;
            foreach (MyVertexBase entity in proActiveNavigationEntities)
            {
                //EntityEdge entityEdge = new EntityEdge(selectedEntity.ToString(), entity.ToString());
               if((entity.CSharpClasses != null) &&(entity.CSharpClasses.ToArray().Length > 0)){
                    CSharpClass cl =  (CSharpClass)entity.CSharpClasses[0];
                    if (!Constant.AUTOTEST)
                    {
                        myGraph.AddEdge(new MyEdgeBase(selectedEntity, entity, cl.NumerOfEdges.ToString()));
                    }
               }else{
                   if (!Constant.AUTOTEST)
                   {
                       myGraph.AddEdge(new MyEdgeBase(selectedEntity, entity, ""));
                   }
               }
               
                protocol[j] = protocolEntities(entity);
                j++;
            }
            
            writeProtocolToFile(protocol, 1);
        }

        private void addEdgesToProActiveEntitiesWithSort(MyGraph myGraph, ArrayList proActiveNavigationEntities, MyVertexBase selectedEntity)
        {
            string[] protocol = new string[proActiveNavigationEntities.ToArray().Length];
            int j = 0;
            foreach (MyVertexBase entity in proActiveNavigationEntities)
            {
                //EntityEdge entityEdge = new EntityEdge(selectedEntity.ToString(), entity.ToString());
                myGraph.AddEdge(new MyEdgeBase(selectedEntity, entity, ""));
                protocol[j] = protocolEntities(entity);
                j++;
            }

            writeProtocolToFile(protocol, 1);
        }


        private ArrayList proActiveClassess = new ArrayList();
        public void updateProActiveNavigationWithoutRefresh(CSharpClass selectedCSharpClass)
        {
            ArrayList methods = selectedCSharpClass.Methods;
            foreach (CSharpMethod cSharpMethod in methods)
            {
                foreach (CSharpClass cSharpClass in xmlModel)
                {
                    if (!cSharpClassModel.Contains(cSharpClass))
                    { //class is still not added. We can navigate there

                        ArrayList incomingCalls = cSharpMethod.IncomingCalls;
                        foreach (IncomingCall incomingCall in incomingCalls)
                        {
                            if (incomingCall.ClassName == cSharpClass.Name)
                            {
                                //add the class to navigation
                                if (!proActiveClassess.Contains(cSharpClass))
                                {
                                    cSharpClass.NumerOfEdges = 1;
                                    proActiveClassess.Add(cSharpClass);
                                }
                                else
                                {
                                    CSharpClass proActiveClass = getClassByName(cSharpClass.Name, proActiveClassess);
                                    proActiveClass.NumerOfEdges = proActiveClass.NumerOfEdges + 1;
                                }
                            }
                        }
                        ArrayList outgountCalls = cSharpMethod.OutgoingCalls;
                        foreach (MethodTypeCall outgoingCall in outgountCalls)
                        {
                            if (outgoingCall.ClassName == cSharpClass.Name)
                            {
                                //add the class to navigation
                                if (!proActiveClassess.Contains(cSharpClass))
                                {
                                    cSharpClass.NumerOfEdges = 1;
                                    proActiveClassess.Add(cSharpClass);
                                }
                                else
                                {
                                    CSharpClass proActiveClass = getClassByName(cSharpClass.Name, proActiveClassess);
                                    proActiveClass.NumerOfEdges = proActiveClass.NumerOfEdges + 1;
                                }
                            }
                        }
                    }
                }
            }


        }
        public ArrayList updateProActiveNavigationWithRefresh(CSharpClass selectedCSharpClass)
        {
            ArrayList listToCheck = new ArrayList();
            ArrayList result = new ArrayList();
            ArrayList methods = selectedCSharpClass.Methods;
            foreach(CSharpMethod cSharpMethod in methods){
                if(cSharpMethod.Name == selectedCSharpClass.SelectedMethod){
                    ArrayList outgoingCalls = cSharpMethod.OutgoingCalls;
                    foreach (MethodTypeCall outgoingCall in outgoingCalls)
                    {
                        CSharpClass classToConnect = getMethodFromClassModel(outgoingCall, xmlModel);
                        
                       
                       
                        if (classToConnect != null)
                       {
                           double relevance = getRelevamceFromClass(classToConnect, outgoingCall.Name);

                           MyVertexBaseVerySmall myVertexSmall = new MyVertexBaseVerySmall();
                           myVertexSmall.Name1 = classToConnect.Name;
                           myVertexSmall.Name2 = outgoingCall.Name;
                           myVertexSmall.Relevance = relevance;
                           if (!listToCheck.Contains(myVertexSmall.Name1 + myVertexSmall.Name2))
                           {
                               listToCheck.Add(myVertexSmall.Name1 + myVertexSmall.Name2);
                               result.Add(myVertexSmall);
                           }
                       }
                    }
                }


            }
            return result;

        }
        private double getRelevamceFromClass(CSharpClass currentCSharpClass, String currentMethodName) {
            
            foreach(CSharpClass cl in cSharpClassModel){
                if (cl.Name == currentCSharpClass.Name) {
                    ArrayList methods = currentCSharpClass.Methods;
                    foreach(CSharpMethod method in methods){
                        if(currentMethodName == method.Name){
                            return cl.Relevance + method.Relevance;
                        }
                    }
                }
            }
            return 0;
        }
        private CSharpClass getMethodFromClassModel(MethodTypeCall outgoingCall, ArrayList model) {
            foreach (CSharpClass cSharpClass in model)
            {
                if(outgoingCall.ClassName == cSharpClass.Name){
                    ArrayList methods = cSharpClass.Methods;
                    foreach(CSharpMethod cSharpMethod in methods){
                        if(outgoingCall.Name == cSharpMethod.Name){
                            return cSharpClass;
                        }
                    }
                }
             }
             return null;
        }
/*ArrayList methods = selectedCSharpClass.Methods;
            foreach(CSharpMethod cSharpMethod in methods){

                foreach (CSharpClass cSharpClass in xmlModel)
                {
                   if(!cSharpClassModel.Contains(cSharpClass)){ //class is still not added. We can navigate there

                       ArrayList incomingCalls = cSharpMethod.IncomingCalls;
                       foreach(IncomingCall incomingCall in incomingCalls){
                           if(incomingCall.ClassName == cSharpClass.Name){
                            //add the class to navigation
                               if (!proActiveClassess.Contains(cSharpClass))
                               {
                                   cSharpClass.NumerOfEdges = 1;
                                   proActiveClassess.Add(cSharpClass);
                               }
                               else {
                                   CSharpClass proActiveClass = getClassByName(cSharpClass.Name, proActiveClassess);
                                   proActiveClass.NumerOfEdges = proActiveClass.NumerOfEdges + 1;
                               }
                           }
                       }
                       ArrayList outgountCalls = cSharpMethod.OutgoingCalls;
                       foreach (MethodTypeCall outgoingCall in outgountCalls)
                       {
                           if(outgoingCall.ClassName == cSharpClass.Name){
                            //add the class to navigation
                               if (!proActiveClassess.Contains(cSharpClass))
                               {
                                   cSharpClass.NumerOfEdges = 1;
                                   proActiveClassess.Add(cSharpClass);
                               }
                               else
                               {
                                   CSharpClass proActiveClass = getClassByName(cSharpClass.Name, proActiveClassess);
                                   proActiveClass.NumerOfEdges = proActiveClass.NumerOfEdges + 1;
                               }
                           }
                       }
                   }
                }
            }

        }*/
        private CSharpClass getClassByName(string className, ArrayList listOfClasses) {
            foreach(CSharpClass cSharpClass in listOfClasses){
                if (cSharpClass.Name == className) {
                    return cSharpClass;
                }
            }
            return null;
        }

        ArrayList structuralConnections = new ArrayList();
        public void classSelectedWithGrpahNoUpdate(CSharpClass cSharpClass, GraphLayout gL, StructuralConnection structuralConnection)
        {
           /* gUt.Clear(cSharpEntityModel);
            cSharpClassModel = Heuristics.updateRelevance(cSharpClassModel, cSharpClass);
            cSharpClassModel.Sort();
            CSharpClass.resetModel(cSharpClassModel);
            positionNumber = 0;
            cSharpEntityModel = new ArrayList();
            updateEntities(cSharpClass);*/
           // updateEdges(null);

           /* if (!structuralConnections.Contains(structuralConnection)) {
                structuralConnections.Add(structuralConnection);
            }*/

            gUt.Clear(cSharpEntityModel);
            cSharpClassModel = Heuristics.updateRelevance(cSharpClassModel, cSharpClass, false);
            cSharpClassModel.Sort();
            CSharpClass.resetModel(cSharpClassModel);
            positionNumber = 0;
            cSharpEntityModel = new ArrayList();
            updateEntities(cSharpClass, false);



            //15.02.13 check this code
           // proActiveClassess = new ArrayList();
            // updateProActiveNavigation(cSharpClass);
        //    gL.Graph = new MyGraph();
           // entitiesToDisplayModel = gUt.updateVertexWithGraph(cSharpClass, cSharpEntityModel, gL, entitiesToDisplayModel);


        //    updateEdges(entitiesToDisplayModel);
            //  gUt.updateProActiveNavigationWithGraph(cSharpClass, proActiveClassess, gL);

          //  gUt.updateEdgesWithGraph(cSharpEntityModel, gL);
            // gUt.updateCoordinates(cSharpClass,gL);


        }

        

        public void classSelected(CSharpClass cSharpClass)
        {
            try
            {
                gUt.Clear(cSharpEntityModel);
                cSharpClassModel = Heuristics.updateRelevance(cSharpClassModel, cSharpClass, false);
                cSharpClassModel.Sort();
            }
            catch (Exception e)
            {
                int i = 0;
            }
            try
            {
                CSharpClass.resetModel(cSharpClassModel);
                updateGUI();
            }
            catch (Exception e)
            {
                int i = 0;
            }
        }

        private void updateGUI()
        {
            positionNumber = 0;
            cSharpEntityModel = new ArrayList();
            updateEntities(null, false);
            updateEdges(null);
            gUt.updateVertex(cSharpEntityModel);
            gUt.updateEdges(cSharpEntityModel);

        }

        private void manageSecondSelectedClass(CSharpClass currentCSharpClass, CSharpClass selectedClass, bool isRefresh)
        {
           
            if(!isRefresh){
                if (currentCSharpClass.IsSelected == true)
                {
                    currentCSharpClass.IsSelected = false;
                    currentCSharpClass.IsSecondSelected = true;
                }
                else
                {
                    currentCSharpClass.IsSecondSelected = false;
                }
            }
            if (selectedClass != null)
            {
                if (currentCSharpClass == selectedClass)
                {
                    currentCSharpClass.IsSelected = true;

                }
            }
        }
        private ArrayList updateEntities(CSharpClass selectedClass, bool isRefresh)
        {
            int numberOfEntities = 1; //will be read from UI
                foreach (CSharpClass currentCSharpClass in cSharpClassModel)
                {
                   // if (!isRefresh)
                  //  {
                        manageSecondSelectedClass(currentCSharpClass, selectedClass, isRefresh);
                    //}
                    
                    MyVertexBase entity = new MyVertexBaseMiddle();
                    if (!currentCSharpClass.IsAddedToEntity)
                    {
                        currentCSharpClass.IsAddedToEntity = true;
                       // entity.CSharpClasses.Add(currentCSharpClass);
                        addClassToEntity(entity, currentCSharpClass);
                        
                        //calculate threshold of the class dependent on relevance
                        calculateThresholdDependentOnRelevance(numberOfEntities);
                        numberOfEntities++;
                        entity.CommonThreshold = currentCSharpClass.Threshold;
                        addAllConnectedClasses(entity, currentCSharpClass);
                        cSharpEntityModel.Add(entity);
                    }
  
             }

            int numberOfEntities2 = 0;

            ArrayList result = new ArrayList();
            foreach(MyVertexBase entity in cSharpEntityModel){
                if (numberOfEntities2 < 7) {
                    result.Add(entity);
                    numberOfEntities2++;
                }
            }
            cSharpEntityModel = result;
            return cSharpEntityModel;
        }

        private void calculateThresholdDependentOnRelevance(int number) {
           //for now the threshol set to large number in order to have all classes separate
           /* double averageRelevance = 0;
            foreach (CSharpClass currentCSharpClass in cSharpClassModel)
            {
                averageRelevance += currentCSharpClass.countRelevance();
            }
            averageRelevance = averageRelevance / cSharpClassModel.Count;*/
            foreach (CSharpClass currentCSharpClass in cSharpClassModel)
            {
              //  if (currentCSharpClass.countRelevance() > averageRelevance)
              //  {
                if (number > 4)
                {
                    currentCSharpClass.Threshold = 1;
                }
                else
                {
                    currentCSharpClass.Threshold = 100;
                }
               // }
               // else {
                 //   currentCSharpClass.Threshold = 1;
                //}
            }
        }

        private void addClassToEntity(MyVertexBase myVertexBase, CSharpClass cSharpClass) {
            myVertexBase.CSharpClasses.Add(cSharpClass);
            if ((myVertexBase.NumberInThePast == 0)||(myVertexBase.NumberInThePast > cSharpClass.NumberInThePast))
            {
                myVertexBase.NumberInThePast = cSharpClass.NumberInThePast;
            }
 
        }
        /* private void addAllConnectedClasses(Entity entity, CSharpClass cSharpClass) {
             ArrayList methods = cSharpClass.Methods;
             ArrayList edges = entity.Edges;

             ArrayList incomingEdges = cSharpClass.IncomingEdges;
               foreach(CSharpClassEdge incomingEdge in incomingEdges){
                   if ((incomingEdge.Weight >= entity.CommonThreshold) &&
                       (incomingEdge.Weight >= cSharpClass.Threshold)) {
                       string classToAddName = incomingEdge.Target;
                       CSharpClass classToAdd = getClassFromModelByName(classToAddName);
                       if ((classToAdd != null)&&(!classToAdd.IsAddedToEntity)) {
                           classToAdd.IsAddedToEntity = true;
                           entity.CSharpClasses.Add(classToAdd);
                           addAllConnectedClasses(entity, classToAdd);
                       }
                   }
               }

               ArrayList outgoingEdges = cSharpClass.OutgoingEdges;
               foreach (CSharpClassEdge outgoingEdge in outgoingEdges)
               {
                   if ((outgoingEdge.Weight >= entity.CommonThreshold)&&
                       (outgoingEdge.Weight >= cSharpClass.Threshold))
                   {
                       string classToAddName = outgoingEdge.Target;
                       CSharpClass classToAdd = getClassFromModelByName(classToAddName);
                       if ((classToAdd != null) && (!classToAdd.IsAddedToEntity))
                       {
                           classToAdd.IsAddedToEntity = true;
                           entity.CSharpClasses.Add(classToAdd);
                           addAllConnectedClasses(entity, classToAdd);
                       }
                   }
               }
         }*/

        private void addAllConnectedClasses(MyVertexBase entity, CSharpClass cSharpClass)
        {
            ArrayList methods = cSharpClass.Methods;
            ArrayList edges = entity.Edges;

            ArrayList incomingEdges = cSharpClass.IncomingEdges;
            foreach (CSharpClassEdge incomingEdge in incomingEdges)
            {
                string classToAddName = incomingEdge.Target;
                CSharpClass classToAdd = getClassFromModelByName(classToAddName);
                if (classToAdd != null)
                {
                    if ((incomingEdge.Weight >= entity.CommonThreshold) &&
                        (incomingEdge.Weight >= classToAdd.Threshold))
                    {

                        if ((classToAdd != null) && (!classToAdd.IsAddedToEntity))
                        {
                            classToAdd.IsAddedToEntity = true;
                          //  entity.CSharpClasses.Add(classToAdd);
                            addClassToEntity(entity, classToAdd);
                            addAllConnectedClasses(entity, classToAdd);
                        }
                    }
                }
            }

            ArrayList outgoingEdges = cSharpClass.OutgoingEdges;
            foreach (CSharpClassEdge outgoingEdge in outgoingEdges)
            {
                string classToAddName = outgoingEdge.Target;
                CSharpClass classToAdd = getClassFromModelByName(classToAddName);
                if (classToAdd != null)
                {
                    if ((outgoingEdge.Weight >= entity.CommonThreshold) &&
                        (outgoingEdge.Weight >= classToAdd.Threshold))
                    {

                        if ((classToAdd != null) && (!classToAdd.IsAddedToEntity))
                        {
                            classToAdd.IsAddedToEntity = true;
                            entity.CSharpClasses.Add(classToAdd);
                            addAllConnectedClasses(entity, classToAdd);
                        }
                    }
                }
            }
        }

        private CSharpClass getClassFromModelByName(String className)
        {
            CSharpClass result = null;
            foreach (CSharpClass cSharpClass in cSharpClassModel)
            {
                if (cSharpClass.Name == className)
                {
                    return cSharpClass;
                }
            }
            return result;
        }

        public void proActiveNavigation() {

            foreach (MyVertexBase entity in cSharpEntityModel)
            {

                Object[] cSharpClass = entity.CSharpClasses.ToArray();
                for (int i = 0; i < cSharpClass.Length; i++)
                {
                    CSharpClass cl = (CSharpClass)cSharpClass[i];
                  //  ArrayList navigationElements = cl.NavigationElements;
                    createClassNavigationElements(cl, 3);
                    
                    
                    
                    //checkEdges(cl, cSharpEntityModel, entity);
                }
            }

        }

        private void createClassNavigationElements(CSharpClass cl, int deepLevel)
        {
            ArrayList methods = cl.Methods;
            CSharpClass newClass = null;

            if (deepLevel > 0) {
                deepLevel--;
                createClassNavigationElements(newClass, deepLevel);
            }
        }

        

        int numberOfProtocolStep = 0;
        public MyVertexBase updateEdges(ArrayList entityiesToProcess)
        {
            string[] protocol = new string[entityiesToProcess.ToArray().Length];
            int j = 0;
            MyVertexBase result = null;
            foreach (MyVertexBase entity in entityiesToProcess)
            {

                if (entity is MyVertexBaseMiddle)
                {
                    result = entity;
                }
                Object[] cSharpClass = entity.CSharpClasses.ToArray();

                for (int i = 0; i < cSharpClass.Length; i++)
                {
                    Type t = cSharpClass[i].GetType();
                    CSharpClass cl = (CSharpClass)cSharpClass[i];
                    checkEdges(cl, cSharpEntityModel, entity);
                }

                protocol[j] = protocolEntities(entity);
                j++;
            }
            writeProtocolToFile(protocol, 0);
            return result;

        }
        private string protocolEntities(MyVertexBase entity)
        {
            Object[] cSharpClass = entity.CSharpClasses.ToArray();
            string result = "";
            for (int i = 0; i < cSharpClass.Length; i++)
            {
                CSharpClass cl = (CSharpClass)cSharpClass[i];
                result += cl.Name + " " + cl.NumerOfEdges + ";;" ;
            }
            return result;
        }
           
        private void writeProtocolToFile(string[] protocol, int navigation)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("e:\\rachota\\s14\\protocol" + numberOfProtocolStep + "_"+navigation+".txt");
         //   for (int i = protocol.Length - 1; i >= 0; i-- )
            for (int i = 0; i < protocol.Length; i++)
            {
                file.Write(protocol[i]);
            }
            if (navigation == 1)
            {
                numberOfProtocolStep++;
            }
            file.Close();
        }

        private void addToEdgeStructuralConnection(EntityEdge edge, CSharpClass cSharpClass, CSharpClass currentClass) { 
            foreach(StructuralConnection structuralConnection in structuralConnections){
                if ((cSharpClass.Name == structuralConnection.classFrom.Name) &&
                    (currentClass.Name == structuralConnection.classTo.Name))
                {
                    edge.NameOfTheConnection = prepareMethodToDispaly(structuralConnection.methodFrom);
                }
            }
        }

        private string prepareMethodToDispaly(string fullName) {

            string[] names = fullName.Split('.');
            return names[2];
        }


        private void checkEdges(CSharpClass cSharpClass, ArrayList entities, MyVertexBase currentEntity)
        {
            foreach (MyVertexBase entity in entities)
            {
                if (entity.ToString() == currentEntity.ToString())
                {
                    continue;
                }
                // ArrayList cSharpClasses = entity.CSharpClasses;
                Object[] cSharpClasses = entity.CSharpClasses.ToArray();

                for (int i = 0; i < cSharpClasses.Length; i++)

                //           foreach (CSharpClass currentSharpClass in cSharpClasses)
                {
                    CSharpClass currentSharpClass = (CSharpClass)cSharpClasses[i];
                    int result = checkCalls(cSharpClass, currentSharpClass);
                    if (result != 0)
                    {
                        EntityEdge entityEdge = new EntityEdge(currentEntity.ToString(), entity.ToString());
                        addToEdgeStructuralConnection(entityEdge, cSharpClass, currentSharpClass);
                        entityEdge.Weight = result;

                        if (!currentEntity.Edges.Contains(entityEdge))
                        {
                            currentEntity.Edges.Add(entityEdge);
                        }
                        else
                        {
                            IEnumerator enumeratorEdges = currentEntity.Edges.GetEnumerator();
                            while (enumeratorEdges.MoveNext())
                            {
                                EntityEdge currentEdge = (EntityEdge)enumeratorEdges.Current;
                                
                                if (currentEdge.Equals(entityEdge))
                                {
                                    currentEdge.Weight += result;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private int checkCalls(CSharpClass cSharpClass, CSharpClass currentSharpClass)
        {
            int result = 0;
            ArrayList methods = cSharpClass.Methods;
            foreach (CSharpMethod cSharpMethod in methods)
            {
                /*ArrayList incomingCalls = cSharpMethod.IncomingCalls;
                foreach (IncomingCall incomingCall in incomingCalls)
                {
                    if (incomingCall.ClassName == currentSharpClass.Name)
                    {
                        result++;
                    }
                }*/
                ArrayList outgoingCalls = cSharpMethod.OutgoingCalls;
                foreach (MethodTypeCall outgoingCall in outgoingCalls)
                {
                    if (outgoingCall.ClassName == currentSharpClass.Name)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public void methodSelected(CSharpMethod cSharpMethod)
        {
            cSharpMethod.ToString();
        }
    }
}
