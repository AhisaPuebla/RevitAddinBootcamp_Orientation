namespace RevitAddinBootcamp
{
    [Transaction(TransactionMode.Manual)]
    public class cmdChallenge01 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 01 Challenge code goes here
            // Delete the TaskDialog below and add your code
            //TaskDialog.Show("Module 01 Challenge", "Coming Soon!");



            Transaction t = new Transaction(doc);
            t.Start("Challenge 01");

            int numberVariable = 250;
            int startingElevation = 0;
            int floorHeight = 15;


            // loop throuh a range of numbers
            for (int i = 1; i <= numberVariable; i++)
                {
                    
                    Level newLevel = Level.Create(doc, startingElevation);
                //newLevel.Name = "Level";
                startingElevation = startingElevation + floorHeight;

                int remainder1 = i % 3;
                int remainder2 = i % 5;

                if (remainder1 == 0 && remainder2 == 0)
                {
                    FilteredElementCollector collector1 = new FilteredElementCollector(doc);
                    collector1.OfCategory(BuiltInCategory.OST_TitleBlocks);
                    collector1.WhereElementIsElementType();

                    ViewSheet newSheet = ViewSheet.Create(doc, collector1.FirstElementId());
                    newSheet.Name = "FIZZBUZZ_#";

                    // create a filtered element collector to get a view family type
                    FilteredElementCollector collector2 = new FilteredElementCollector(doc);
                    collector2.OfClass(typeof(ViewFamilyType));

                    ViewFamilyType floorPlanVFT = null;
                    foreach (ViewFamilyType curVFT in collector2)
                    {
                        if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                        {
                            floorPlanVFT = curVFT;
                        }
                    }

                    // create a floorplan view
                    ViewPlan newFloorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                    newFloorPlan.Name = "My new fizzbuzz floor plan"+i;

                    // create a viewport
                    XYZ insPoint1 = new XYZ(1, 0.5, 0);

                    Viewport newViewport = Viewport.Create(doc, newSheet.Id, newFloorPlan.Id, insPoint1);


                }
                else if (remainder2 == 0)
                {
                    newLevel.Name = $"BUZZ_#"+i;
                }
                else if (remainder1 == 0)
                {
                    newLevel.Name = $"FIZZ_#"+i;
                }


                }


            t.Commit();
            t.Dispose();

            return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnChallenge01";
            string buttonTitle = "Module\r01";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Module01,
                Properties.Resources.Module01,
                "Module 01 Challenge");

            return myButtonData.Data;
        }
    }

}
