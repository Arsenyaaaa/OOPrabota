namespace NorthwindApp
{
    public class Employees
    {
        // Свойства
        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Extension { get; set; }
        public string Country { get; set; }

        // Конструктор
        public Employees(int employeeID, string lastName, string firstName, int extension, string country)
        {
            EmployeeID = employeeID;
            LastName = lastName;
            FirstName = firstName;
            Extension = extension;
            Country = country;
        }
    }
}


