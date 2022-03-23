using System.Numerics;
using System.Collections.Generic;

namespace CCompiler {
  public class TypeCast {
    public static Expression LogicalToIntegral(Expression expression) {
      if (expression.Symbol.Type.IsLogical()) {
        return ImplicitCast(expression, Type.SignedIntegerType);
      }

      return expression;
    }

    public static Expression LogicalToFloating(Expression expression) {
      if (expression.Symbol.Type.IsLogical()) {
        return ImplicitCast(expression, Type.DoubleType);
      }

      return expression;
    }

    public static Expression ToLogical(Expression expression) {
      if (!expression.Symbol.Type.IsLogical()) {
        return ImplicitCast(expression, Type.LogicalType);
      }

      return expression;
    }

    public static Expression ImplicitCast(Expression sourceExpression,
                                          Type targetType) {
      Type fromType = sourceExpression.Symbol.Type;

      if (fromType.Equals(targetType) ||
          ((fromType.IsFloating() && targetType.IsFloating()) ||
           (fromType.IsIntegralPointerArrayStringOrFunction() &&
            targetType.IsIntegralPointerArrayStringOrFunction())) &&
            (fromType.SizeAddress() == targetType.SizeAddress())) {
        return sourceExpression;
      }
      else {
        return ExplicitCast(sourceExpression, targetType);
      }
    }

    public static Expression ExplicitCast(Expression sourceExpression,
                                          Type targetType) {
      Expression constantExpression =
        ConstantExpression.Cast(sourceExpression, targetType);
      if (constantExpression != null) {
        return constantExpression;
      }

      Symbol sourceSymbol = sourceExpression.Symbol, targetSymbol = null;
      Type sourceType = sourceSymbol.Type;
      List<MiddleCode> shortList = sourceExpression.ShortList,
                       longList = sourceExpression.LongList;

      if (targetType.IsVoid()) {
        targetSymbol = new Symbol(targetType);
      }
      else if (sourceType.IsStructOrUnion() && targetType.IsStructOrUnion()) {
        Error.Check(sourceType.Equals(targetType), sourceType + " to " +
                     targetType, Message.Invalid_type_cast);
        targetSymbol = new Symbol(targetType);
      }
      else if (sourceType.IsLogical() &&
               targetType.IsIntegralPointerOrArray()) {
        targetSymbol = new Symbol(targetType);

        Symbol oneSymbol = new Symbol(targetType, BigInteger.One);
        MiddleCode trueCode =
          new MiddleCode(MiddleOperator.Assign, targetSymbol, oneSymbol);
        MiddleCodeGenerator.Backpatch(sourceSymbol.TrueSet, trueCode);
        longList.Add(trueCode);

        MiddleCode targetCode = new MiddleCode(MiddleOperator.Empty);
        longList.Add(new MiddleCode(MiddleOperator.Jump, targetCode));

        Symbol zeroSymbol = new Symbol(targetType, BigInteger.Zero);
        MiddleCode falseCode =
          new MiddleCode(MiddleOperator.Assign, targetSymbol, zeroSymbol);
        MiddleCodeGenerator.Backpatch(sourceSymbol.FalseSet, falseCode);
        longList.Add(falseCode);

        longList.Add(targetCode);
      }
      else if (sourceType.IsLogical() && targetType.IsFloating()) {
        targetSymbol = new Symbol(targetType);

        MiddleCode trueCode = new MiddleCode(MiddleOperator.PushOne);
        MiddleCodeGenerator.Backpatch(sourceSymbol.TrueSet, trueCode);
        longList.Add(trueCode);

        MiddleCode targetCode = new MiddleCode(MiddleOperator.Empty);
        longList.Add(new MiddleCode(MiddleOperator.Jump, targetCode));

        MiddleCode falseCode = new MiddleCode(MiddleOperator.PushZero);
        MiddleCodeGenerator.Backpatch(sourceSymbol.FalseSet, falseCode);
        longList.Add(falseCode);

        longList.Add(targetCode);
      }
      else if ((sourceType.IsArithmetic() || sourceType.IsPointerArrayStringOrFunction()) &&
               targetType.IsLogical()) {
        object zeroValue = sourceType.IsLogical() ? ((object) decimal.Zero)
                                                  : ((object)BigInteger.Zero);
        Symbol zeroSymbol = new Symbol(targetType, zeroValue);

        MiddleCode testCode =
          new MiddleCode(MiddleOperator.NotEqual, null,
                         sourceSymbol, zeroSymbol);
        ISet<MiddleCode> trueSet = new HashSet<MiddleCode>();
        trueSet.Add(testCode);
        longList.Add(testCode);

        MiddleCode gotoCode = new MiddleCode(MiddleOperator.Jump);
        ISet<MiddleCode> falseSet = new HashSet<MiddleCode>();
        falseSet.Add(gotoCode);
        longList.Add(gotoCode);

        targetSymbol = new Symbol(trueSet, falseSet);
      }
      else if (sourceType.IsFloating() && targetType.IsFloating()) {
        targetSymbol = new Symbol(targetType);
      }
      else if (sourceType.IsFloating() &&
               targetType.IsIntegralPointerOrArray()) {
        targetSymbol = new Symbol(targetType);

        if (targetType.Size() == 1) {
          Type tempType = sourceType.IsSigned() ? Type.SignedIntegerType
                                                : Type.UnsignedIntegerType;
          Symbol tempSymbol = new Symbol(tempType);
          MiddleCode tempCode =
            new MiddleCode(MiddleOperator.FloatingToIntegral, tempSymbol,
                           sourceSymbol);
          longList.Add(tempCode);
          MiddleCode resultCode =
            new MiddleCode(MiddleOperator.IntegralToIntegral, targetSymbol,
                           tempSymbol);
          longList.Add(resultCode);
        }
        else {
          MiddleCode resultCode =
            new MiddleCode(MiddleOperator.FloatingToIntegral, targetSymbol,
                           sourceSymbol);
          longList.Add(resultCode);
        }
      }
      else if (sourceType.IsIntegralPointerArrayStringOrFunction () &&
               targetType.IsFloating()) {
        targetSymbol = new Symbol(targetType);

        if (sourceType.Size() == 1) {
          Type tempType = sourceType.IsSigned() ? Type.SignedIntegerType
                                                : Type.UnsignedIntegerType;
          Symbol tempSymbol = new Symbol(tempType);
          MiddleCodeGenerator.
            AddMiddleCode(longList, MiddleOperator.IntegralToIntegral,
                          tempSymbol, sourceSymbol);
          MiddleCodeGenerator.
            AddMiddleCode(longList, MiddleOperator.IntegralToFloating,
                          targetSymbol, tempSymbol);
        }
        else {
          MiddleCodeGenerator.
            AddMiddleCode(longList, MiddleOperator.IntegralToFloating,
                          targetSymbol, sourceSymbol);
        }
      }
      else if (sourceType.IsIntegralPointerArrayStringOrFunction () &&
               targetType.IsIntegralPointerOrArray()) {
        targetSymbol = new Symbol(targetType);
        MiddleCodeGenerator.
          AddMiddleCode(longList, MiddleOperator.IntegralToIntegral,
                        targetSymbol, sourceSymbol);
      }

      Error.Check(targetSymbol != null, sourceType + " to " +
                   targetType, Message.Invalid_type_cast);
      return (new Expression(targetSymbol, shortList, longList));    
    }

    public static Type MaxType(Type leftType, Type rightType) {
      if ((leftType.IsFloating() && !rightType.IsFloating()) ||
          ((leftType.Size() == rightType.Size()) &&
           leftType.IsSigned() && !rightType.IsSigned())) {
        return leftType;
      }
      else if ((!leftType.IsFloating() && rightType.IsFloating()) ||
               ((leftType.Size() == rightType.Size()) &&
               !leftType.IsSigned() && rightType.IsSigned())) {
        return rightType;
      }
      else {
        return (leftType.Size() > rightType.Size()) ? leftType : rightType;
      }
    }
  }
}
