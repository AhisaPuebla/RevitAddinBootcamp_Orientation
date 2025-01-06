using System;



namespace RevitAddinBootcamp
{

    // 4. create neighborhood class
    public class Neighborhood
    {
        public string Name { get; set; }
        public string City { get; set; }

        public string State { get; set; }
        public List<Building> BuildingList { get; set; }
        public Neighborhood(string _name, string _city, string _state, List<Building> _buildingList)
        {
            Name = _name;
            City = _city;
            State = _state;
            BuildingList = _buildingList;
        }

        public int GetBuildingCount()
        {
            return BuildingList.Count;
        }

    }

    //1. create class
    public class Building
    {
        public string Name { get; set; } //  the getter and the setter
        public string Adress { get; set; }
        public int NumberOfFloors { get; set; }
        public double Area { get; set; }

        //3. add constructor to class. matching the arguments to the properties
        public Building(string _name, string _address, int _numberOfFloors, double _area)
        {
            Name = _name;
            Adress = _address;
            NumberOfFloors = _numberOfFloors;
            Area = _area;
        }
    }


    // challenge 03 1. class to hold the spreadsheet info.
    public class RoomFurniture
    {
        public string RoomName { get; set; }
        public string FamilyName { get; set; }
        public string FamilyType { get; set; }
        public int Quantity { get; set; }

        // challenge 03 3. add constructor to class,  matching the arguments to the properties
        public RoomFurniture(string _room, string _family, string _type, int _quantity)
        {
            RoomName = _room;
            FamilyName = _family;
            FamilyType = _type;
            Quantity = _quantity;
        }

    }
}