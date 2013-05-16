using Microsoft.master2.model;
using Microsoft.master2.rules;
using Relations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Master2.excel
{
    class ReadExcel
    {
        RuleEngine ruleEngine;
        GraphLayout graphLayout;
        ArrayList model;
        ArrayList umlModel;
        ArrayList precisionList = new ArrayList();
        ArrayList recallList = new ArrayList();

        public ReadExcel(RuleEngine ruleEngine, GraphLayout graphLayout, ArrayList model, ArrayList umlModel)
        {
            this.ruleEngine = ruleEngine;
            this.graphLayout = graphLayout;
            this.model = model;
            this.umlModel = umlModel;
        }
        
        public void ReadFile(string filename, string listname) {
            System.IO.StreamWriter file = new System.IO.StreamWriter("e:\\rachota\\revisits.txt");

            OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=Excel 8.0");
            con.Open();
            try
            {
                //Create Dataset and fill with imformation from the Excel Spreadsheet for easier reference
                DataSet myDataSet = new DataSet();
                OleDbDataAdapter myCommand = new OleDbDataAdapter(" SELECT * FROM [" + listname + "$]", con);
                myCommand.Fill(myDataSet);
                con.Close();

                //Travers through each row in the dataset
                foreach (DataRow myDataRow in myDataSet.Tables[0].Rows)
                {
                    //Stores info in Datarow into an array
                    Object[] cells = myDataRow.ItemArray;
                    readCells(cells);
                }
                writeUmlDiagram();
            }
            catch (Exception ex)
            {
                int exce = 0;
            }
            finally{con.Close();  }

            file.WriteLine("visits " + statisticUtil.classVisits);
            file.WriteLine("revisits " + statisticUtil.classRevisits);
            file.WriteLine("method visits " + statisticUtil.methodVisits);
            file.WriteLine("method revisits " + statisticUtil.methodRevisits);
            file.Close();
        }
        private void writeUmlDiagram() {
            System.IO.StreamWriter file = new System.IO.StreamWriter("E:\\CSharp\\pwsafe\\J3UML.txt");
            file.WriteLine("Precision");
            foreach (double line in precisionList)
            {
                file.WriteLine(line);
            }
            file.WriteLine("");

            file.WriteLine("Recall");
            foreach (double line in recallList)
            {
                file.WriteLine(line);
            }

            file.Close();
        }
        ArrayList openedClasses = new ArrayList();
        StatisticUtil statisticUtil = new StatisticUtil();
        ArrayList openedMethods = new ArrayList();
        ArrayList results = null;
        CSharpClass lastSelectedClass = null;
        string lastMethodEdit = "";
        private void readCells(Object[] cells)
        {
           
         
            if ((cells[0].ToString() == "Open Declaration") || (cells[0].ToString() == "Open Type"))
            {
                if (cells[3].ToString() == "Class")
                {
                    string elementName = cells[2].ToString();
                    elementName = elementName.Replace(" ", string.Empty);
                    CSharpClass currentClass = getCurrentClassFormModel(elementName);
                    if (currentClass != null)
                    {

                        if (!openedClasses.Contains(elementName))
                        {
                            openedClasses.Add(elementName);
                            checkProActiveNaviForClass(elementName);
                        }
                        else
                        {
                            statisticUtil.classRevisits++;
                            revisitsList.Add(elementName);
                            checkThisStep(elementName);
                        }
                        statisticUtil.classVisits++;

                        testModel(elementName);
                    }
                }
                //-- to test--//
                if (cells[3].ToString() == "Method")
                {

                    string elementName = cells[2].ToString();
                    elementName = elementName.Replace(" ", string.Empty);
                    elementName = removeBrackets(elementName);
                    string classname = cells[4].ToString();

                    lastSelectedClass = getCurrentClassFormModel(classname);
                    if (lastSelectedClass != null)
                    {
                        if (!openedMethods.Contains(classname+elementName))
                        {
                            openedMethods.Add(classname + elementName);
                            checkProActiveNaviForMethod(elementName);
                        }
                        else
                        {
                            statisticUtil.methodRevisits++;
                            if(!checkThisStepForMethod(classname, elementName)){
                                int methodisNot = 0;
                            };
                        }
                        statisticUtil.methodVisits++;

                        testMethodModel(classname, elementName);
                    }
                }
                
            }
        
            if ((cells[0].ToString() == "Show Method")|| (cells[0].ToString() == "Edit Code"))
            {
                if (cells[3].ToString() == "Method")
                {
                    string classname = cells[4].ToString();
                    string elementName = cells[2].ToString();
                    elementName = elementName.Replace(" ", string.Empty);
                    elementName = removeBrackets(elementName);
                    lastSelectedClass = getCurrentClassFormModel(classname);
                    if (lastSelectedClass != null)
                    {

                        if (!openedMethods.Contains(classname + elementName))
                        {
                            openedMethods.Add(classname + elementName);
                            checkProActiveNaviForMethod(elementName);
                        }
                        else
                        {
                                statisticUtil.methodRevisits++;
                                if (!checkThisStepForMethod(classname, elementName))
                                {
                                    int methodisNot = 0;
                                };
                        }
                        statisticUtil.methodVisits++;
                        
                        testMethodModel(classname, elementName);
                    }
                }
            }


          
            if (cells[0].ToString() == "Close Type")
            {
               results = ruleEngine.classSelectedWithGrpah(lastSelectedClass, graphLayout, false);
              // analyseResults(results);
            }
            
        }

        private ArrayList revisitsList = new ArrayList();
        private ArrayList gotRevisitsList = new ArrayList();

        private string removeBrackets(string elementName) {
            if (elementName.Contains('(') && elementName.Contains(')'))
            {
                if (elementName == "save(File)")
                {
                    elementName = "save";
                }
                else
                {
                    elementName = elementName.Substring(0, elementName.Length - 2);
                }
            }
            return elementName;
        }

        private void testMethodModel(string classname, string methodName)
        {
              methodName = removeBrackets(methodName); 
             // checkThisStepForMethod(classname, methodName);
              saveForTheNextStepForMethod(methodName);
        }

        private string previouseName = "";
        private void saveForTheNextStepForMethod(string name)
        {
            lastSelectedClass.SelectedMethod = name;
            if (lastSelectedClass != null)
            {
                if (previouseName != lastSelectedClass.Name)
                {
                    ruleEngine.classSelectedWithGrpah(lastSelectedClass, graphLayout, false);
                    previouseName = lastSelectedClass.Name;
                }
                results = ruleEngine.classSelectedWithGrpahRefreshButton(lastSelectedClass, graphLayout, true);
           }
            lastSelectedClass.SelectedMethod = "";
            analyseResults(results);
        }

        private bool checkThisStepForMethod(string classname, string name)
        {
            //checkProActiveNaviForMethod(name);
            return checkPastElementsForMethod(classname, name);
        }

        private void checkProActiveNaviForMethod(string name)
        {
            if (proactiveNavigation != null)
            {
                foreach (MyVertexBase myBase in proactiveNavigation)
                {
                    checkMethodWithEntity(myBase, name);
                };
            }

        }

        private void checkMethodWithEntity(MyVertexBase myBase, string name)
        {
            ArrayList cSharpClasses = myBase.CSharpClasses;
            if (myBase.GetType() == typeof(MyVertexBaseVerySmall))
            {
                myBase = (MyVertexBaseVerySmall)myBase;
                if (myBase.Name1 == lastSelectedClass.Name)
                {
                    if (myBase.Name2 == name)
                    {
                        statisticUtil.methodInProActiveModelVisits++;
                    }
                }
            }
            if (myBase.GetType() == typeof(MyVertexBaseSmall))
            {
                myBase = (MyVertexBaseSmall)myBase;
                if ((myBase.Name2 == name) || (myBase.Name3 == name) || (myBase.Name4 == name))
                {
                    statisticUtil.methodInProActiveModelVisits++;
                }
            }
            if (myBase.GetType() == typeof(MyVertexBaseMiddle))
            {
                myBase = (MyVertexBaseMiddle)myBase;
                if ((myBase.Name2 == name) || (myBase.Name3 == name) || (myBase.Name4 == name))
                {
                    statisticUtil.methodInProActiveModelVisits++;
                }
            }
            
        }

        private bool checkPastElementsForMethod(string classname, string name) {
            bool result = false;
            if (entitiesModel != null)
            {
                foreach (MyVertexBase myBase in entitiesModel)
                {
                    ArrayList classes = myBase.CSharpClasses;
                    foreach(CSharpClass cSharpClass in classes){
                        if(cSharpClass.Name == lastSelectedClass.Name){
                            result = checkMethodsInEntity(myBase, name);
                        }
                    }
                };
            }
            return result;
        }

        private bool checkMethodsInEntity(MyVertexBase myBase, string methodName)
        {
            bool result = false;
            if (myBase.GetType() == typeof(MyVertexBaseSmall))
            {
                myBase = (MyVertexBaseSmall)myBase;
                if ((myBase.Name2 == methodName) || (myBase.Name3 == methodName) || (myBase.Name4 == methodName))
                {
                    statisticUtil.methodInthePastModelRevisits++;
                    result = true;
                }
            }
 

            if (myBase.GetType() == typeof(MyVertexBaseMiddle))
            {
                myBase = (MyVertexBaseMiddle)myBase;
                if ((myBase.Name2 == methodName) || (myBase.Name3 == methodName) || (myBase.Name4 == methodName))
                {
                    statisticUtil.methodInthePastModelRevisits++;
                    result = true;
                }
            }

            if (myBase.GetType() == typeof(MyVertexBaseVerySmall))
            {
                myBase = (MyVertexBaseVerySmall)myBase;
                if ((myBase.Name2 == methodName))
                {
                    statisticUtil.methodInthePastModelRevisits++;
                    result = true;
                }
            }
            return result;
        }

        //-----//
        private void testModel(string name){

           // checkThisStep(name);
            saveForTheNextStep(name);//save result for testing next step
        }

        private void checkThisStep(string name) {
            //checkProActiveNavi(name);
            checkPastElements(name);
        }
        private void checkProActiveNaviForClass(string name)
        {
            if (proactiveNavigation != null)
            {
                foreach (MyVertexBase myBase in proactiveNavigation)
                {
                    checkClassWithEntityProActive(myBase, name);
                };
            }

        }

        /*private void checkProActiveNavi(string name)
        {
            if (proactiveNavigation != null)
            {
              foreach(MyVertexBase myBase in proactiveNavigation){
                  checkClassWithEntity(myBase, name);
              };
            }

        }*/

        private void checkPastElements(string name)
        {
            if (entitiesModel != null)
            {
                foreach (MyVertexBase myBase in entitiesModel)
                {
                    checkClassWithEntity(myBase, name);
                  
                };
            }
        }

        private void checkClassWithEntityProActive(MyVertexBase myBase, string name)
        {
            try
            {
                if (myBase.GetType() == typeof(MyVertexBaseSmall))
                {
                    myBase = (MyVertexBaseSmall)myBase;
                    if (myBase.Name1 == name)
                    {
                        statisticUtil.classInProActiveModelVisits++;
                    }
                }
            }catch(Exception e){
                int breakpoint = 0;
            }


        }

        private void checkClassWithEntity(MyVertexBase myBase, string name)
        {
           
       
                ArrayList cSharpClasses = myBase.CSharpClasses;
                if (myBase.GetType() == typeof(MyVertexBaseVerySmall))
                {
                   myBase = (MyVertexBaseVerySmall)myBase;
                   if (myBase.Name1 == name)
                   {
                       statisticUtil.classInthePastModelRevisits++;
                    }
                }
                else // NOT MyVertexBaseVerySmall
                {
                    try
                    {
                        foreach (CSharpClass cSharpClasse in cSharpClasses)
                        {
                            if (cSharpClasse.Name == name)
                            {
                                statisticUtil.classInthePastModelRevisits++;
                            }
                        }
                    }catch(Exception e2){

                        int a = 0;
                    }
                }

        }

        private void saveForTheNextStep(string name)
        {
            CSharpClass currentClass = getCurrentClassFormModel(name);
  
            if (name == "Day")
            {
                int breakPoint = 0;
            }
            if (currentClass != null)
            {
            
                results = ruleEngine.classSelectedWithGrpah(currentClass, graphLayout, false);
                lastSelectedClass = currentClass;
                previouseName = currentClass.Name;
            }
            analyseResults(results);
        }




        ArrayList proactiveNavigation;
        ArrayList entitiesModel;
        private void analyseResults(ArrayList results) {
            if (results != null)
            {
                try
                {
                    object[] res = results.ToArray();
                    proactiveNavigation = (ArrayList)res[0];
                    entitiesModel = (ArrayList)res[1];

                    //analyse UML model
                    analyseUmlModel();

                }catch(Exception e){
                }
            }
        }

        private PreRecContainer analyseUmlModel()
        { 
        //precision = (relev and retrived) / retrieved
        //recall = (revl + retrieved) / relevant
           double relevantAndRetrieved = 0;
           string className = "";
           foreach (MyVertexBase myBase in entitiesModel)
           {
               className = getClassNameFromEntity(myBase);
               foreach(string classinUML in umlModel){
                    if(classinUML == className){
                        relevantAndRetrieved++;
                    }
               }
           }
           PreRecContainer preRec = new PreRecContainer();
           preRec.precision = relevantAndRetrieved / entitiesModel.ToArray().Length;
           preRec.recall = relevantAndRetrieved / umlModel.ToArray().Length;
           precisionList.Add(preRec.precision);
           recallList.Add(preRec.recall); 
           return preRec;
        }

        private string getClassNameFromEntity(MyVertexBase myBase)
        {
            if (myBase.GetType() == typeof(MyVertexBaseSmall))
            {
                myBase = (MyVertexBaseSmall)myBase;
            }


            if (myBase.GetType() == typeof(MyVertexBaseMiddle))
            {
                myBase = (MyVertexBaseMiddle)myBase;
            }

            if (myBase.GetType() == typeof(MyVertexBaseVerySmall))
            {
                myBase = (MyVertexBaseVerySmall)myBase;
            }

            return myBase.Name1;
        }

        private CSharpClass getCurrentClassFormModel(string className)
        {
            if(className.Contains(".")){
                className = className.Split('.')[1];
            }
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

        private ArrayList removeRedundantElements(ArrayList model, int numberOfNeedeElements) {
            ArrayList result = new ArrayList();
            int counter = 0;
            foreach(Object element in model){
                if (counter < numberOfNeedeElements)
                {
                    result.Add(element);
                    counter++;
                }
                else {
                    break;
                }
            }
            return result;
        }

    }



}
