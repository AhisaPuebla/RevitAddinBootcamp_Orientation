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


            

            // 1.Set variables
            int numberVariable = 250;
            int startingElevation = 0;
            int floorHeight = 15;

            // 2.Get titleblock
            FilteredElementCollector tbCollector = new FilteredElementCollector(doc);
            tbCollector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            tbCollector.WhereElementIsElementType();
            ElementId tblockID = tbCollector.FirstElementId();

            // 3.Get all view family types
            FilteredElementCollector vftCollector = new FilteredElementCollector(doc);
            vftCollector.OfClass(typeof(ViewFamilyType));

            // 4.Get floor plan and ceiling plan view family types
            ViewFamilyType floorPlanVFT = null;
            ViewFamilyType ceilingPlanVFT = null;

            foreach (ViewFamilyType curVFT in vftCollector)
            {
                if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                {
                    floorPlanVFT = curVFT;
                }
                else if (curVFT.ViewFamily == ViewFamily.CeilingPlan)
                {
                    ceilingPlanVFT = curVFT;
                }
            }

            // 4b.


            // 5. Create transaction
            Transaction t = new Transaction(doc);
            t.Start("FIZZ BUZZ Challenge");

            // 6. Create floors and check FIZZBUZZ
            for (int i = 1; i <= numberVariable; i++)
                {
                    //7. Create level
                    Level newLevel = Level.Create(doc, startingElevation);
                newLevel.Name = $"Level+{i}";
                //startingElevation = startingElevation + floorHeight;
               

                //8. Check for FIZZ, BUZZ and FIZZBUZZ

                int remainder1 = i % 3;
                int remainder2 = i % 5;

                if (remainder1 == 0 && remainder2 == 0)
                {


                    ViewSheet newSheet = ViewSheet.Create(doc, tblockID);
                    newSheet.SheetNumber = i.ToString();
                    newSheet.Name = $"FIZZBUZZ_#+{i}";

          
                    // create a floorplan view
                    ViewPlan newFloorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                    

                    // create a viewport
                    XYZ insPoint1 = new XYZ(1, 0.5, 0);
                    Viewport newViewport = Viewport.Create(doc, newSheet.Id, newFloorPlan.Id, insPoint1);

                }

                else if (remainder1 == 0)
                {
                    ViewPlan newFloorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                }

                else if (remainder2 == 0)
                {
                    ViewPlan newCeilingPlan = ViewPlan.Create(doc, ceilingPlanVFT.Id, newLevel.Id);
                }

                //9. increment elevation 
                startingElevation += floorHeight;


            }


            t.Commit();
            t.Dispose();

            // 11. alert user
            TaskDialog.Show("Complete", "Created " + numberVariable + " levels.");
            //TaskDialog.Show("Complete", $"Created {numFloors} levels.");

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
