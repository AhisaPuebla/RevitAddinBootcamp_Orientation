using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Visual;
using System.Windows.Media;

namespace RevitAddinBootcamp
{
    [Transaction(TransactionMode.Manual)]
    public class cmdChallenge02 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            //Prompt to select elements
            TaskDialog.Show("Select lines", "Select some lines to convert to Revit elements");
            List<Element> pickList = uidoc.Selection.PickElementsByRectangle("Select Elements").ToList();

            List<CurveElement> filteredList = new List<CurveElement>();
            foreach (Element elem in pickList)
            {
                if (elem is CurveElement)
                {
                    //if the data type is a curve, add it to the allCurves list
                    CurveElement curCurveElement = elem as CurveElement;
                    filteredList.Add(curCurveElement);



                }
            }

            // get level
            View curView = doc.ActiveView;
            //Parameter levelParameter = curView.LookupParameter("Associated Level");        //this is 
            Parameter levelParameter = curView.get_Parameter(BuiltInParameter.PLAN_VIEW_LEVEL);//same as this. but this one will work in other languages
            string levelName = levelParameter.AsString();
            ElementId levelId = levelParameter.AsElementId();

            Level currentLevel = GetLevelByName(doc, levelName);

         
            //get types
            WallType wt1 = GetWallTypeByName(doc, "Storefront");
            WallType wt2 = GetWallTypeByName(doc, "Generic - 8\""); // <-------- add " to the string
            MEPSystemType ductSystem = GetMEPSystemByName(doc, "Supply Air");
            MEPSystemType pipeSystem = GetMEPSystemByName(doc, "Domestic Hot Water");
            DuctType ductType = GetDuctTypeByName(doc, "Default");
            PipeType pipeType = GetPipeTypeByName(doc, "Default");

            // loop through curve elements
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create elements");

                foreach (CurveElement currentCurve in filteredList)
                {
                    //Get graphic style from a curve
                    GraphicsStyle currentStyle = currentCurve.LineStyle as GraphicsStyle;
                    string lineStyleName = currentStyle.Name;

                    //Get curve geometry
                    Curve curveGeom = currentCurve.GeometryCurve;

                    //use switch statement to create elements
                    switch (lineStyleName)
                    {
                        case "A-GLAZ":
                            Wall wall1 = Wall.Create(doc, curveGeom, wt1.Id, currentLevel.Id, 20, 0, false, false);
                            break;

                        case "A-WALL":
                            Wall wall2 = Wall.Create(doc, curveGeom, wt2.Id, currentLevel.Id, 20, 0, false, false);
                            break;

                        case "M-DUCT":
                            Duct duct = Duct.Create(doc, ductSystem.Id, ductType.Id, currentLevel.Id, curveGeom.GetEndPoint(0), curveGeom.GetEndPoint(1));
                            break;

                        case "P-PIPE":
                            Pipe pipe = Pipe.Create(doc, pipeSystem.Id, pipeType.Id, currentLevel.Id, curveGeom.GetEndPoint(0), curveGeom.GetEndPoint(1));
                            break;

                    }
                }


                t.Commit();

            }
            return Result.Succeeded;
        }
        internal static Level GetLevelByName(Document doc, string levelName)
        {
            FilteredElementCollector levelCollector = new FilteredElementCollector(doc);
            levelCollector.OfCategory(BuiltInCategory.OST_Levels);
            levelCollector.WhereElementIsNotElementType();

            foreach (Level curLevel in levelCollector)
            {
                if (curLevel.Name == levelName)
                {
                    return curLevel;
                }
            }

            return null;
        }

        private PipeType GetPipeTypeByName(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(PipeType));

            foreach (PipeType curType in collector)
            {
                if (curType.Name == typeName)
                {
                    return curType;
                }
            }

            return null; ;
        }

        private DuctType GetDuctTypeByName(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(DuctType));

            foreach (DuctType curType in collector)
            {
                if (curType.Name == typeName)
                {
                    return curType;
                }
            }

            return null; ;
        }

        private MEPSystemType GetMEPSystemByName(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(MEPSystemType));                

            foreach (MEPSystemType curType in collector)
            {
                if (curType.Name == typeName)
                {
                    return curType;
                }
            }

            return null; ;
        }

        private WallType GetWallTypeByName(Document doc, string typeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            //collector.OfCategory(BuiltInCategory.OST_Walls);
            //collector.WhereElementIsElementType();                 this is
            collector.OfClass(typeof(WallType));                   //same as this 

            foreach (WallType curType in collector)
            {
                if (curType.Name == typeName)
                {
                    return curType;
                }
            }

            return null;
        }



               
        








        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnChallenge02";
            string buttonTitle = "Module\r02";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Module02,
                Properties.Resources.Module02,
                "Module 02 Challenge");

            return myButtonData.Data;
        }
    }

}
