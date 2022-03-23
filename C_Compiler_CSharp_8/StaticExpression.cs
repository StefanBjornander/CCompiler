using System.Numerics;

namespace CCompiler {
  public class StaticExpression {
    public static Expression Binary(MiddleOperator middleOp,
                                    Expression leftExpression,
                                    Expression rightExpression) {
      Symbol leftSymbol = leftExpression.Symbol,
             rightSymbol = rightExpression.Symbol;
      Type leftType = leftSymbol.Type, rightType = rightSymbol.Type;
      object leftValue = leftSymbol.Value, rightValue = rightSymbol.Value;

      switch (middleOp) {
        case MiddleOperator.Add:
          if ((leftValue is StaticAddress) && // &p + 2
              (rightValue is BigInteger)) {
            return GenerateAddition(leftSymbol, (BigInteger) rightValue);
          }
          else if ((leftValue is BigInteger) && // 2 + &p
                   (rightValue is StaticAddress)) {
            return GenerateAddition(rightSymbol, (BigInteger) leftValue);
          }
          else if (leftSymbol.IsExternOrStaticArray() && // a + 2
                   (rightValue is BigInteger)) {
            return GenerateAddition(leftSymbol, (BigInteger) rightValue);
          }
          else if ((leftValue is BigInteger) && // 2 + a
                   rightSymbol.IsExternOrStaticArray()) {
            return GenerateAddition(rightSymbol, (BigInteger) leftValue);
          }
          break;

        case MiddleOperator.Subtract:
          if ((leftValue is StaticAddress) && // &p - 2
              (rightValue is BigInteger)) {
            return GenerateAddition(leftSymbol, -((BigInteger) rightValue));
          }
          else if (leftSymbol.IsExternOrStaticArray() && // a - 2
                   (rightValue is BigInteger)) {
            return GenerateAddition(leftSymbol, -((BigInteger) rightValue));
          }
          break;

        case MiddleOperator.Dot:
          if (leftSymbol.IsExternOrStatic()) {
            object resultValue =
              new StaticValue(leftSymbol.UniqueName,
                              rightSymbol.Offset); // s.i
            Symbol resultSymbol = new Symbol(leftType, resultValue);
            return (new Expression(resultSymbol, null, null));
          }
          break;
      }
    
      return null;
    }  

    private static Expression GenerateAddition(Symbol symbol,
                                               BigInteger value) {
      int offset = ((int) value) * symbol.Type.PointerOrArrayType.Size();
      StaticAddress resultValue;

      if (symbol.Value is StaticAddress staticAddress) {
        resultValue = new StaticAddress(staticAddress.UniqueName,
                                        staticAddress.Offset + offset);
      }
      else {
        resultValue = new StaticAddress(symbol.UniqueName, offset);
      }

      Symbol resultSymbol = new Symbol(symbol.Type, resultValue);
      return (new Expression(resultSymbol, null, null));
    }

    public static Expression Unary(MiddleOperator middleOp,
                                   Expression expression) {
      Symbol symbol = expression.Symbol;

      switch (middleOp) {
        case MiddleOperator.Address: {
            if (symbol.Value is StaticValue staticValue) { // &a[i], &s.i
              StaticAddress staticAddress =
                new StaticAddress(staticValue.UniqueName, staticValue.Offset);
              Symbol resultSymbol =
                new Symbol(new Type(symbol.Type), staticAddress);
              return (new Expression(resultSymbol, null, null));
            }
            else if (symbol.IsExternOrStatic()) { // &i
              StaticAddress staticAddress =
                new StaticAddress(symbol.UniqueName, 0);
              Symbol resultSymbol =
                new Symbol(new Type(symbol.Type), staticAddress);
              return (new Expression(resultSymbol, null, null));
            }
          }
          break;

        case MiddleOperator.Dereference: {
            if (symbol.Value is StaticAddress staticAddress) {
              StaticValue staticValue =
                new StaticValue(staticAddress.UniqueName,
                                staticAddress.Offset);
              Symbol resultSymbol =
                new Symbol(new Type(symbol.Type), staticValue);
              return (new Expression(resultSymbol, null, null));
            }
          }
          break;
      }

      return null;
    }
  }
}
