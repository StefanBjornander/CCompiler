// int months[] = {31, isLeapYear ? 29 : 28, 31, ..};

// char s[] = "Hello";
// char s[] = {'H', 'e', 'l', 'l', 'o', '\n'};
// char *p = "Hello";

using System.Numerics;
using System.Collections.Generic;

namespace CCompiler {
  class Initializer {
    public static void Generate(Symbol toSymbol, object fromInitializer,
                                List<MiddleCode> codeList, int extraOffset = 0) {
      Type toType = toSymbol.Type;

      if (fromInitializer is Expression) {
        Expression fromExpression = (Expression) fromInitializer;

        if (toType.IsArray() && toType.ArrayType.IsChar() &&
            fromExpression.Symbol.Type.IsString()) {
          string text = ((string) fromExpression.Symbol.Value) + "\0";

          if (toType.ArraySize == 0) {
            toType.ArraySize = text.Length;
          }
          else {
            Error.Check(text.Length < toType.ArraySize, toType,
                         Message.Too_many_initializers_in_array);
          }

          List<object> list = new List<object>();

          foreach (char c in text) {
            Symbol charSymbol =
              new Symbol(toType.ArrayType, (BigInteger)((int)c));
            list.Add(new Expression(charSymbol));
          }

          fromInitializer = list;
        }
      }

      if (fromInitializer is List<object> fromList) {
        switch (toType.Sort) {
          case Sort.Array: {
              fromList = ModifyInitializer.ModifyArray(toType, fromList);

              if (toType.ArraySize == 0) {
                toType.ArraySize = fromList.Count;
              }
              else {
                Error.Check(fromList.Count <= toType.ArraySize,
                            toType, Message.Too_many_initializers_in_array);
              }

              for (int index = 0; index < fromList.Count; ++index) {
                Symbol indexSymbol = new Symbol(toType.ArrayType);
                indexSymbol.Storage = toSymbol.Storage;
                indexSymbol.Offset = toSymbol.Offset +
                                    (index * toType.ArrayType.Size());
                indexSymbol.Name = toSymbol.Name + "[" + index + "]";
                Generate(indexSymbol, fromList[index], codeList, extraOffset);
                extraOffset += toType.ArrayType.Size();
              }

              if (toSymbol.IsStatic()) {
                int restSize = toType.Size() -
                               (fromList.Count * toType.ArrayType.Size());
                codeList.Add(new MiddleCode(MiddleOperator.InitializerZero,
                                            restSize));
              }
            }
            break;

          case Sort.Struct: {
            List<Symbol> memberList = toType.MemberList; 
              Error.Check(fromList.Count <= memberList.Count, toType,
                          Message.Too_many_initializers_in_struct);

              int initSize = 0;
              for (int index = 0; index < fromList.Count; ++index) {
                Symbol memberSymbol = memberList[index];
                Symbol subSymbol = new Symbol(memberList[index].Type);
                subSymbol.Storage = toSymbol.Storage;
                subSymbol.Name = toSymbol.Name + "." + memberSymbol.Name;
                subSymbol.Offset = toSymbol.Offset + memberSymbol.Offset;
                Generate(subSymbol, fromList[index], codeList, extraOffset);
                extraOffset += memberSymbol.Type.Size();
                initSize += memberSymbol.Type.Size();
              }

              if (toSymbol.IsStatic()) {
                int restSize = toType.Size() - initSize;
                codeList.Add(new MiddleCode(MiddleOperator.InitializerZero,
                                            restSize));
              }
            }
            break;

          case Sort.Union: {
              List<Symbol> memberList = toType.MemberList;
              Error.Check(fromList.Count == 1, toType,
                          Message.Only_one_Initlizer_allowed_in_unions);
              Symbol memberSymbol = memberList[0];
              Symbol subSymbol = new Symbol(memberSymbol.Type);
              subSymbol.Storage = toSymbol.Storage;
              subSymbol.Name = toSymbol.Name + "." + memberSymbol.Name;
              subSymbol.Offset = toSymbol.Offset;
              Generate(subSymbol, fromList[0], codeList, extraOffset);

              if (toSymbol.IsStatic()) {
                int restSize = toType.Size() - memberSymbol.Type.Size();
                codeList.Add(new MiddleCode(MiddleOperator.InitializerZero,
                                            restSize));
              }
            }
            break;

          default:
            Error.Report(toType, Message.
                Only_array_struct_or_union_can_be_initialized_by_a_list);
            break;
        }
      }
      else {
        Expression fromExpression = (Expression) fromInitializer;

        if (toSymbol.IsAutoOrRegister()) {
          fromExpression = TypeCast.ImplicitCast(fromExpression, toType);

          foreach (MiddleCode middleCode in fromExpression.LongList) {
            switch (middleCode.Operator) {
              case MiddleOperator.PreCall:
              case MiddleOperator.ParameterInitSize:
              case MiddleOperator.Parameter:
              case MiddleOperator.Call:
              case MiddleOperator.PostCall:
                middleCode[0] = ((int) middleCode[0]) + extraOffset;
                break;
            }
          }

          codeList.AddRange(fromExpression.LongList);
      
          if (toSymbol.Type.IsFloating()) {
            codeList.Add(new MiddleCode(MiddleOperator.PopFloat, toSymbol));
          }
          else {
            if (fromExpression.Symbol.Type.IsStructOrUnion()) {
              codeList.Add(new MiddleCode(MiddleOperator.AssignInitSize,
                                          toSymbol, fromExpression.Symbol));
            }

            codeList.Add(new MiddleCode(MiddleOperator.Assign, toSymbol,
                                        fromExpression.Symbol));
          }
        }
        else {
          Symbol fromSymbol = fromExpression.Symbol;
          Error.Check(fromSymbol.IsExternOrStatic(), fromSymbol,
                       Message.Non__static_initializer);
          Type fromType = fromSymbol.Type;

          if (toType.IsPointer() && fromType.IsArrayFunctionOrString()) {
            Error.Check((fromType.IsString() && toType.PointerType.IsChar()) ||
                         (fromType.IsArray() &&
                          fromType.ArrayType.Equals(toType.PointerType)) ||
                         (fromType.IsFunction() &&
                          fromType.Equals(toType.PointerType)),
                         Message.Invalid_type_cast);
            StaticAddress staticAddress =
              new StaticAddress(fromSymbol.UniqueName, 0);
            codeList.Add(new MiddleCode(MiddleOperator.Initializer,
                                        toType.Sort, staticAddress));
          }
          else {
            Expression toExpression =
              TypeCast.ImplicitCast(fromExpression, toType);
            Symbol constantSymbol = toExpression.Symbol;
            Error.Check(constantSymbol.Value != null, constantSymbol,
                         Message.Non__constant_expression);
            codeList.Add(new MiddleCode(MiddleOperator.Initializer,
                                        constantSymbol.Type.Sort, constantSymbol.Value));
          }
        }
      }
    }
  }
}
