using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Architecture;
using RevitAddinBootcamp.Common;

namespace RevitAddinBootcamp
{
    [Transaction(TransactionMode.Manual)]
    public class cmdSkills03 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            //creating my own classes!
            Building building1 = new Building("Big Office Building" , "10 Main Street", 10, 150000 ); //2. create an instance of the dynamic class we just created in the bottom
            //building.Name = "Big Office Building";
            //building.Adress = "10 Main Street";
            //building.NumberOfFloors = 10;
            //building.Area = 150000;------------------- i dont need this anymore

            Building building2 = new Building("Fancy Hotel", "15 Main Street", 15, 200000);


            //change the number of floors
            building1.NumberOfFloors = 11;

            // building is recognized as a data type now, so i can create a list of data type building
            List<Building> buildings = new List<Building>();
            buildings.Add(building1);
            buildings.Add(building2);
            buildings.Add(new Building ("Hospital", "20 Main Street", 20, 350000 ));
            buildings.Add(new Building("Giant Store", "20 Main Street", 5, 40000));

            //5. Create neighborhood instance
            Neighborhood downtown = new Neighborhood("Downtown ", "Middletown", "CT", buildings);
            TaskDialog.Show("Test", $"There are {downtown.GetBuildingCount()} buildings in the " + $" {downtown.Name} neighborhood");

            //6. work with rooms
            FilteredElementCollector roomCollector = new FilteredElementCollector(doc);
            roomCollector.OfCategory(BuiltInCategory.OST_Rooms); // ---------------------------- all the rooms in the model

            Room curRoom = roomCollector.First() as Room; // DONT FORGET TO CAST

            // 7. get room name 
            string RoomName = curRoom.Name;

            //7a. check room name --------------------------according to revit, the room name is: ROOM NAME + ROOM #     eg. "Office 1"
            if (RoomName.Contains("Office"))
            {
                TaskDialog.Show("Test", "Found the room!");
            }

            //7b. Get room Point (location)
            Location roomLocation = curRoom.Location; // - dont really need this i think
            LocationPoint roomLocPt = curRoom.Location as LocationPoint;
            XYZ roomPoint = roomLocPt.Point;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Insert families into rooms");
                // 8. insert families
                FamilySymbol curFamSymbol = Utils.GetFamilySymbolByName(doc, "Desk", "Large");
                curFamSymbol.Activate(); // load family symbol so i can place it in the model if its not placed yet

                foreach(Room curRoom2 in roomCollector)
                {
                    LocationPoint loc = curRoom2.Location as LocationPoint;

                    FamilyInstance curFamInstance = doc.Create.NewFamilyInstance(loc.Point, curFamSymbol, StructuralType.NonStructural);

                    string department = Utils.GetParameterValueAsString(curRoom2, "Department");
                    double area = Utils.GetParameterValueAsDouble(curRoom2, BuiltInParameter.ROOM_AREA);
                    double area2 = Utils.GetParameterValueAsDouble(curRoom, "Area");

                    Utils.SetParameterValue(curRoom, "Department", "Architecture");

                }
                t.Commit();
            }

            return Result.Succeeded;
        }

 
    }


}
