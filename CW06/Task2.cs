using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CW06
{
    public class Task2
    {
        public void Analize()
        {
            Console.WriteLine("Fields: ");
            //lista pól w klasie Pogrupowane względem dostępu
            Console.WriteLine("-- Public: ");
            //publiczne
            Type type = Type.GetType("CW06.Customer", false, true);
            List<FieldInfo> nonPublicFields = new List<FieldInfo>();
            foreach (FieldInfo field in type.GetFields())
            {
                Console.WriteLine($"Type: \"{field.FieldType}\"; name: \"{field.Name}\"");   
            }

            Console.WriteLine("-- Non Public: ");
            //niepubliczne
            //Przykład:
            //Type: “string”; name: “_name”
            nonPublicFields = type.GetFields(
                         BindingFlags.NonPublic |
                         BindingFlags.Instance).ToList<FieldInfo>();
            foreach (FieldInfo field in nonPublicFields)
            {
                Console.WriteLine($"Type: \"{field.FieldType}\"; name: \"{field.Name}\"");
            }

            Console.WriteLine();
            Console.WriteLine("Methods: ");
            //Lista metod
            foreach (MethodInfo method in type.GetMethods())
            {
                string modificator = "";
                if (method.IsStatic)
                    modificator += "static ";
                if (method.IsVirtual)
                    modificator += "virtual";
                Console.Write($"{modificator} {method.ReturnType.Name} {method.Name} (");
                //получаем все параметры
                ParameterInfo[] parameters = method.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    Console.Write($"{parameters[i].ParameterType.Name} {parameters[i].Name}");
                    if (i + 1 < parameters.Length) Console.Write(", ");
                }
                Console.WriteLine(")");
            }

            Console.WriteLine();
            Console.WriteLine("Nested types: ");
            //typy zagnieżdżone
            foreach (Type nested in type.GetNestedTypes())
            {
                Console.WriteLine($"Type: \"{nested}\"; name: \"{nested.Name}\"");
            }

            Console.WriteLine();
            Console.WriteLine("Properties: ");
            //propercje
            foreach (PropertyInfo prop in type.GetProperties())
            {
                Console.WriteLine($"Type: \"{prop.PropertyType}\"; name: \"{prop.Name}\"");
            }

            Console.WriteLine();
            Console.WriteLine("Members: ");
            //Członkowie
            Type myType = Type.GetType("CW06.Customer", false, true);

            foreach (MemberInfo mi in myType.GetMembers())
            {
                Console.WriteLine($"{mi.DeclaringType} {mi.MemberType} {mi.Name}");
            }
            Console.WriteLine();
        }
    }
}
