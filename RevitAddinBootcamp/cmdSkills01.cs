namespace RevitAddinBootcamp
{
    [Transaction(TransactionMode.Manual)]
    public class cmdSkills01 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 01 Skills code goes here
            // Delete the TaskDialog below and add your code
            TaskDialog.Show("Module 01 Skills", "Got Here to Skills 01!");
            TaskDialog.Show("test", "Testing to see if this makes it to Github");
            TaskDialog.Show("test", "Testing to see if this makes it back to VisualStudio");

            return Result.Succeeded;
        }
    }

}
