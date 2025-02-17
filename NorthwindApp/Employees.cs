namespace NorthwindApp
{
    class Employees
    {
        private int _employeeID; // Поле для свойства employeeID
        private string _lastName; // Поле для свойства lastName
        private string _firstName; // Поле для свойства firstName
        private int _extension; // Поле для свойства extension
        private string _country; // Поле для свойства country

        public Employees(int employeeID, string lastName, string firstName, int extension, string country)
        {
            _employeeID = employeeID;
            _lastName = lastName;
            _firstName = firstName;
            _extension = extension;
            _country = country;
        }

        // Свойства
        public int employeeID { get => _employeeID; set => _employeeID = value; }
        public string lastName { get => _lastName; set => _lastName = value; }
        public string firstName { get => _firstName; set => _firstName = value; }
        public int extension { get => _extension; set => _extension = value; }
        public string country { get => _country; set => _country = value; }
    }
}

