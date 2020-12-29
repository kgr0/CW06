using System;
using System.Reflection;
using System.Windows;

namespace CW06
{
    public class Task3
    {
        public void Start()
        {
            Customer customer = new Customer("Bill");
            Type customerType = typeof(Customer);

            PropertyInfo nameProperty = customerType.GetProperty("Name");
            PropertyInfo addressProperty = customerType.GetProperty("Address");
            PropertyInfo someValueProperty = customerType.GetProperty("SomeValue");

            try
            {
                addressProperty.SetValue(customer, "London, UK", null);
                someValueProperty.SetValue(customer, 15, null);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Console.WriteLine($"{nameProperty} = {nameProperty.GetValue(customer)}");
            Console.WriteLine($"{addressProperty} = {addressProperty.GetValue(customer)}");
            Console.WriteLine($"{someValueProperty} = {someValueProperty.GetValue(customer)}");
        }
    }
}
