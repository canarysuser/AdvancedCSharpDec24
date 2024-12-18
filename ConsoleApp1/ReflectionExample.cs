using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit; 

namespace ConsoleApp1
{
    public class TestClass
    {
        public int IntField = 10;
        public string NameProperty { get; set; }
        public TestClass(int fieldId)
        {
            Console.WriteLine("TestClass() ctor called.");
            IntField = fieldId;
        }
        public void Show(string name)
        {
            NameProperty = name;
            Console.WriteLine("FieldId: "+ IntField + ", Name: " + NameProperty);
        }
    }

    internal class ReflectionExample
    {
        internal static void Test()
        {
            //Get the current executing assembly 
            Assembly asm = Assembly.GetExecutingAssembly();
            //Assemblies consist of modules - which can be .exe/.dll/.netmodule 
            Module mod = asm.GetModule("ConsoleApp1.exe"); 
            //Get the specific type from the Module 
            Type t = mod.GetType(typeof(TestClass).FullName); //FQN of the Type
            Console.WriteLine($"Type:{t.Name}\nParent:{t.BaseType.FullName}\nVisibility:{(t.IsPublic?"Public":"Not Public")}");

            //Fields using the FieldInfo class 
            var fields = t.GetFields();
            Console.WriteLine("\nFields:");
            fields.ToList().ForEach(f =>
            {
                Console.WriteLine($"Name:{f.Name}, Type:{f.FieldType.FullName}, Attributes: {f.Attributes}");
            });
            //Properies 
            var props = t.GetProperties().ToList();
            Console.WriteLine("\nProperties: ");
            props.ForEach(c =>
            {
                Console.WriteLine($"Name:{c.Name}, Type: {c.PropertyType.FullName}");
            });
            var methods=t.GetMethods().ToList();

            Console.WriteLine("\nMethods:");
            methods.ForEach(c =>
            {
                Console.WriteLine($"Name:{c.Name}, ReturnType:{c.ReturnType.FullName},Declared in: {c.DeclaringType.FullName}");
            });

            Console.WriteLine("\nInstantiating the type: ");
            //Dynamically instantiate the type 
            object obj = Activator.CreateInstance(type: t, args: 10);
            //Invoke Methods on the object 
            var invocationAttributes = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance;
            t.InvokeMember(
                name: "Show",
                invokeAttr: invocationAttributes,
                binder: null,
                target: obj,
                args: new[] { "ABB" });

        }

        internal static void TestDynamicAssembly()
        {
            AssemblyName asmName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder asmBldr = AppDomain.CurrentDomain.DefineDynamicAssembly(
                name: asmName,
                access: AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder mb = asmBldr.DefineDynamicModule(name: "DynamicAssembly.dll");
            TypeBuilder tb = mb.DefineType(name: "DynamicClass", attr: TypeAttributes.Public); 
            FieldBuilder fd = tb.DefineField(fieldName:"Id", type: typeof(int), attributes: FieldAttributes.Public);
            MethodBuilder meb=tb.DefineMethod(name:"Show", attributes: MethodAttributes.Public);
            //Include the IL instructions 
            ILGenerator ilGen = meb.GetILGenerator();
            ilGen.EmitWriteLine("Hello World"); 
            ilGen.Emit(OpCodes.Nop);
            tb.CreateType();
            asmBldr.Save("DynamicAssembly.dll");
            Console.WriteLine("Assembly created.");


        }


    }
}
