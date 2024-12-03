using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using System.Windows.Controls;

namespace RevitAddinBootcamp
{
    [Transaction(TransactionMode.Manual)]
    public class cmdSkills02 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 02 Skills code goes here

            //1a. Pick single element
            Reference pickRef = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select Element");
            Element pickElement = doc.GetElement(pickRef);

            //1b. Pick multiple elements
            List<Element> pickList = uidoc.Selection.PickElementsByRectangle("Select Elements").ToList();

            TaskDialog.Show("Test", $"I selected {pickList.Count} elements.");

            //2. Filter selected elements for lines -> going through each element the user has selected and checking if its a curve element
            List<CurveElement> allCurves = new List<CurveElement>();
            foreach (Element elem in pickList)
            {
                if (elem is CurveElement)
                {
                    //if the data type is a curve, add it to the allCurves lis
                    allCurves.Add(elem as CurveElement);
                }
            }

            //2. Filter selected elements for model curves (second elevel of filtering)
            List<CurveElement> modelCurves = new List<CurveElement>();
            foreach (Element elem2 in pickList)
            {
                if (elem2 is CurveElement)
                {
                    // CurveElement curveElem = elem2 as CurveElement;
                    CurveElement curveElem = (CurveElement)elem2;

                    if (curveElem.CurveElementType == CurveElementType.ModelCurve)
                    {
                        modelCurves.Add(curveElem);
                    }
                }
            }

            //3. Curve data 
            foreach (CurveElement currentCurve in modelCurves)
            {
                Curve curve = currentCurve.GeometryCurve; //geometry curve property
                XYZ startPoint = curve.GetEndPoint(0);
                XYZ endPoint = curve.GetEndPoint(1);

                //find linestyle
                GraphicsStyle curStyle = currentCurve.LineStyle as GraphicsStyle;

                Debug.Print(curStyle.Name);
            }

            //5. Create transaction

            //Transaction t = new Transaction(doc); <------ this
            //t.Start("Create a wall");

            using (Transaction t = new Transaction(doc)) //  <--------- or this
            {
                t.Start("Create a wall");

                //4. Create wall 
                Level newLevel = Level.Create(doc, 20);


                CurveElement curveElem = modelCurves[0]; //this
                Curve curCurve = curveElem.GeometryCurve;
                Curve curCurve2 = modelCurves[1].GeometryCurve; // is the same as this 


                Wall newWall = Wall.Create(doc, curCurve, newLevel.Id, false);

                //4b. Create wall with wall type
                FilteredElementCollector wallTypes = new FilteredElementCollector(doc);
                wallTypes.OfCategory(BuiltInCategory.OST_Walls);
                wallTypes.WhereElementIsElementType();


                Wall newWall2 = Wall.Create(doc, curCurve2, wallTypes.FirstElementId(), newLevel.Id, 20, 0, false, false);

                //6. Get system types
                FilteredElementCollector systemCollector = new FilteredElementCollector(doc);
                systemCollector.OfClass(typeof(MEPSystemType));

                //7. Get duct system type
                MEPSystemType ductSystem = GetSystemTypeByName(doc, "Supply Air");


                //8. Get duct type
                FilteredElementCollector ductCollector = new FilteredElementCollector(doc);
                ductCollector.OfClass(typeof(DuctType));

                //9. Create duct
                Curve curCurve3 = modelCurves[2].GeometryCurve;
                Duct newDuct = Duct.Create(doc, ductSystem.Id, ductCollector.FirstElementId(),
                    newLevel.Id, curCurve3.GetEndPoint(0), curCurve3.GetEndPoint(1));


                //10. Get pipe system type
                MEPSystemType pipeSystem = GetSystemTypeByName(doc, "Domestic Hot Water");

                // I CAN DELET AL THIS BEACUASE ITS NOW AT THE BOTTOM IN THE METHOD i will use it both times
                //foreach (MEPSystemType systemType in systemCollector)
                //{
                //    if (systemType.Name == "Domestic Hot Water")
                //    {
                //        pipeSystem = systemType;
                //    }
                //}

                //11. Get pipe type
                FilteredElementCollector pipeCollector = new FilteredElementCollector(doc);
                pipeCollector.OfClass(typeof(PipeType));


                //12. Create pipe
                Curve curCurve4 = modelCurves[3].GeometryCurve;
                Pipe newPipe = Pipe.Create(doc, pipeSystem.Id, pipeCollector.FirstElementId(),
                    newLevel.Id, curCurve4.GetEndPoint(0), curCurve4.GetEndPoint(1));

                //13. Switch statement - when we are going to use a lot of if else, if else...
                int numberValue = 5;
                string numberAsString = "";

                switch(numberValue)
                {
                    case 0:
                        numberAsString = "Zero";
                        break;

                    case 5:
                        numberAsString = "Five";
                        break;

                    case 10:
                        numberAsString = "Ten";
                        break;

                    default:
                        numberAsString = "Ninety nine";
                        break;
                }
                    



                t.Commit();
            }




            return Result.Succeeded;
        }

        //Create a method - ways to create methods:
        internal string MyFirstMethod()
        {
            return "This is my first method";
        }

        internal void MySecondMethod()
        {
            Debug.Print("This is my second method");
        }

        internal string MyThirdMethod(string input)
        {
            string returnString = $"This is my third method: {input}";
            return returnString;
        }

        internal MEPSystemType GetSystemTypeByName(Document doc, string name)
        {
            //get all system types 
            FilteredElementCollector systemCollector = new FilteredElementCollector(doc);
            systemCollector.OfClass(typeof(MEPSystemType));

            //get duct system type
            foreach (MEPSystemType systemType in systemCollector)
            {
                if (systemType.Name == name)
                {
                    return systemType;
                }
            }

            return null;


        }

    }

}
