using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Course;
using Trainning24.Domain.Entity;

namespace Training24Admin.Model
{
    public class Employee
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }


    public static class DataStorage
    {
        public static List<Employee> GetAllEmployess()
        {
            return new List<Employee>
            {
                new Employee { Name="Mike", LastName="Turner", Age=35, Gender="Male"},
                new Employee { Name="Sonja", LastName="Markus", Age=22, Gender="Female"},
                new Employee { Name="Luck", LastName="Martins", Age=40, Gender="Male"},
                new Employee { Name="Sofia", LastName="Packner", Age=30, Gender="Female"},
                new Employee { Name="John", LastName="Doe", Age=45, Gender="Male"}
            };
        }
    }

    public static class TemplateGenerator
    { 
        
        public static string GetHTMLString(ReceiptModel receiptModel)
        {
            string htmlString = "";
            string myString = "";
            string logopath = "";
            try
            {
                //htmlString = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "invoice.htm"));
                //string html = File.ReadAllText("invoice.html");
                htmlString = Path.Combine(Directory.GetCurrentDirectory(), "invoice.html");
                logopath = Path.Combine(Directory.GetCurrentDirectory(), "logo.png");

                using (StreamReader reader = new StreamReader(htmlString))
                {
                    string readFile = reader.ReadToEnd();
                    myString = readFile;

                    myString = myString.Replace("Email", "stmsingh07@gmail.com");
                    myString = myString.Replace("logopath", logopath);
                    myString = myString.Replace("studentdt", receiptModel.students);
                    myString = myString.Replace("Coursedt", receiptModel.Course);
                    myString = myString.Replace("Gradedt", receiptModel.Grade);
                    myString = myString.Replace("Packagedt", receiptModel.Package);
                    myString = myString.Replace("StartDate", receiptModel.StartDate);
                    myString = myString.Replace("EndDate", receiptModel.EndDate);
                    myString = myString.Replace("SalesPartner", receiptModel.SalesPartner);
                    myString = myString.Replace("AdditionalGradePurchased", receiptModel.AdditionalGradePurchased);
                    myString = myString.Replace("PackageDiscount", receiptModel.PackageDiscount);
                    myString = myString.Replace("Taxdt", receiptModel.Tax);
                    myString = myString.Replace("Paiddt", receiptModel.Paid);
                    myString = myString.Replace("Totaldt", receiptModel.Total);
                }

                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "invoice.html"), myString);
            }
            catch (Exception ex)
            {

                throw;
            }
            return myString;
        }

        public static string GetHTMLString()
        {
            var employees = DataStorage.GetAllEmployess();
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is tested by me</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Name</th>
                                        <th>LastName</th>
                                        <th>Age</th>
                                        <th>Gender</th>
                                    </tr>");

            foreach (var emp in employees)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", emp.Name, emp.LastName, emp.Age, emp.Gender);
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
    }





    internal class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }
        protected override IntPtr LoadUnmanagedDll(String unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }
}
