namespace RevitAddinBootcamp.Common
{
    internal static class Utils
    {
        // cmdSkills03 methods
        public static void SetParameterValue(Element curElem, string paramName, string value)
        {
            Parameter curParam = curElem.LookupParameter(paramName);

            if (curParam != null)
            {
                curParam.Set(value);
            }
        }

        public static void SetParameterValue(Element curElem, string paramName, int value)
        {
            Parameter curParam = curElem.LookupParameter(paramName);

            if (curParam != null)
            {
                curParam.Set(value);
            }
        }

        public static string GetParameterValueAsString(Element curElem, string paramName)
        {
            Parameter curParam = curElem.LookupParameter(paramName); // does that parameter exist?
                                                                     // Parameter curParam = CurveElement.get_Parameter(BuiltInParameter.ROOM_AREA);

            if (curParam != null)
            {
                return curParam.AsString();
            }
            else
                return ""; // if it exists, return something
        }

        public static string GetParameterValueAsString(Element curElem, BuiltInParameter bip) // since we have two methods with the same name, its an overload
        {
            Parameter curParam = curElem.get_Parameter(bip);

            if (curParam != null)
            {
                return curParam.AsString();
            }
            else
                return ""; // if it exists, return something
        }

        public static double GetParameterValueAsDouble(Element curElem, string paramName)
        {
            Parameter curParam = curElem.LookupParameter(paramName); // does that parameter exist?
                                                                     // Parameter curParam = CurveElement.get_Parameter(BuiltInParameter.ROOM_AREA);

            if (curParam != null)
            {
                return curParam.AsDouble();
            }
            else
                return 0; // if it exists, return something
        }

        public static double GetParameterValueAsDouble(Element curElem, BuiltInParameter bip)
        {
            Parameter curParam = curElem.get_Parameter(bip);

            if (curParam != null)
            {
                return curParam.AsDouble();
            }
            else
                return 0; // if it exists, return something
        }

        //private string GetBIPValueAsString(Element curElem, BuiltInParameter bip)
        //{
        //    Parameter curParam = curElem.get_Parameter(bip);

        //    if (curParam != null)
        //    {
        //        return curParam.AsString();
        //    }
        //    else
        //        return ""; // if it exists, return something
        //}

        public static FamilySymbol GetFamilySymbolByName(Document doc, string familyName, string familySymbolName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol curSymbol in collector) // check family name and type name
            {
                if (curSymbol.FamilyName == familyName)
                {
                    if (curSymbol.Name == familySymbolName)
                    {
                        return curSymbol;
                    }
                }
            }
            return null;

        }

        // Ribbon methods
        internal static RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            RibbonPanel curPanel;
            if (GetRibbonPanelByName(app, tabName, panelName) == null)
                curPanel = app.CreateRibbonPanel(tabName, panelName);
            else
                curPanel = GetRibbonPanelByName(app, tabName, panelName);
            return curPanel;
        }
        internal static RibbonPanel GetRibbonPanelByName(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel tmpPanel in app.GetRibbonPanels(tabName))
            {
                if (tmpPanel.Name == panelName)
                    return tmpPanel;
            }
            return null;
        }
    }
}
