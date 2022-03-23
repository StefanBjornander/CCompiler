using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Calculator {
  class MainX {
    public static Dictionary<string, double> VariableMap = new();

    static void Main(string[] args) {
      if (args.Length != 1) {
        Console.Out.WriteLine("Usage: calculator inputfile");
      }

      { System.Globalization.CultureInfo customCulture =
         (System.Globalization.CultureInfo)
         System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
      }

      VariableMap["pi"] = Math.PI;
      VariableMap["e"] = Math.E;

      try {
        FileStream file = new(args[0], FileMode.Open);
        Scanner scanner = new(file);
        Parser parser = new(scanner);
        parser.Parse();
      }
      catch (Exception exception) {
        Console.Out.WriteLine(exception.ToString());
      }

//      Console.In.ReadLine();
    }
  }

  public partial class Parser :
         QUT.Gppg.ShiftReduceParser<ValueType, QUT.Gppg.LexLocation> {
    public Parser(Scanner scanner)
     :base(scanner) {
      // Empty.
    }
  }
}