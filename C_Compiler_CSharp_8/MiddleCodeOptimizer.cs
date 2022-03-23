using System;
using System.IO;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;

namespace CCompiler {
  public class MiddleCodeOptimizer {
    private bool m_update;
    private List<MiddleCode> m_middleCodeList;

    public MiddleCodeOptimizer(List<MiddleCode> middleCodeList) {
      m_middleCodeList = middleCodeList;
    }

    public void Optimize() {
      ObjectToIntegerAddresses();

      if (SymbolTable.CurrentFunction.Name.Equals("log")) {
        string name = @"C:\Users\Stefa\Documents\vagrant\homestead\code\code\" +
                      SymbolTable.CurrentFunction.Name + ".middlenumber";
        StreamWriter streamWriter = new StreamWriter(name);

        for (int index = 0; index < m_middleCodeList.Count; ++index) {
          MiddleCode middleCode = m_middleCodeList[index];
          streamWriter.WriteLine(index + ": " + middleCode.ToString());
        }

        streamWriter.Close();
      }

      do {
        m_update = false;
        ClearGotoNextStatements();
        ClearDoubleRelationStatements();
        TraceGotoChains();
        ClearUnreachableCode();
        RemovePushPop();
        MergePopPushToTop();
        MergeTopPopEmptyToPop();
        MergeBinary();
        ConstantDereference();
        OptimizeRelation();
        OptimizeCommutative();
        SematicOptimization();
        RemoveTemporaryAssign();
        RemoveTemporaryAccess();
        RemoveTrivialAssign();
        RemoveClearedCode();
      } while (m_update);
    }

    public void ObjectToIntegerAddresses() {
      IDictionary<MiddleCode, int> addressMap =
        new Dictionary<MiddleCode, int>();

      for (int index = 0; index < m_middleCodeList.Count; ++index) {
        MiddleCode middleCode = m_middleCodeList[index];
        addressMap.Add(middleCode, index);
      }

      for (int index = 0; index < m_middleCodeList.Count; ++index) {
        MiddleCode sourceCode = m_middleCodeList[index];

        if (sourceCode.IsGoto() || sourceCode.IsCarry() ||
            sourceCode.IsRelation()) {
          Error.ErrorXXX(sourceCode[0] is MiddleCode);
          MiddleCode targetCode = (MiddleCode)sourceCode[0];
          Error.ErrorXXX(addressMap.ContainsKey(targetCode));
          sourceCode[0] = addressMap[targetCode];
        }
      }
    }

    // --------------------------------------------------------------------------

    // 1. goto 2
    // 2. ...

    private void ClearGotoNextStatements() {
      for (int index = 0; index < (m_middleCodeList.Count - 1); ++index) {
        MiddleCode middleCode = m_middleCodeList[index];

        if (middleCode.IsRelationCarryOrGoto()) {
          int target = (int) middleCode[0];

          if (target == (index + 1)) {
            middleCode.Clear();
            m_update = true;
          }
        }
      }
    }

    // --------------------------------------------------------------------------

    // 1. if a < b goto 3
    // 2. goto 10

    // 1. if a >= b goto 10
    // 2. Clear

    // 1. if a < b goto 3
    // 2. goto 10

    // 1. if a >= b goto 10
    // 2. empty

    public static IDictionary<MiddleOperator, MiddleOperator> m_inverseMap =
      new Dictionary<MiddleOperator, MiddleOperator>() {
        {MiddleOperator.Equal, MiddleOperator.NotEqual},
        {MiddleOperator.NotEqual, MiddleOperator.Equal},
        {MiddleOperator.Carry, MiddleOperator.NotCarry},
        {MiddleOperator.NotCarry, MiddleOperator.Carry},
        {MiddleOperator.LessThan, MiddleOperator.GreaterThanEqual},
        {MiddleOperator.LessThanEqual, MiddleOperator.GreaterThan},
        {MiddleOperator.GreaterThan, MiddleOperator.LessThanEqual},
        {MiddleOperator.GreaterThanEqual, MiddleOperator.LessThan}
      };

    private void ClearDoubleRelationStatements() {
      for (int index = 0; index < (m_middleCodeList.Count - 1); ++index) {
        MiddleCode thisCode = m_middleCodeList[index],
                   nextCode = m_middleCodeList[index + 1];

        if ((thisCode.IsRelation() || thisCode.IsCarry()) &&
            nextCode.IsGoto()) {
          int target1 = (int) thisCode[0], target2 = (int) nextCode[0];

          if (target1 == (index + 2)) {
            MiddleOperator operator1 = thisCode.getOperator();
            thisCode.setOperator(m_inverseMap[operator1]);
            thisCode[0] = target2;
            nextCode.Clear();
            m_update = true;
          }
        }
      }
    }

    // --------------------------------------------------------------------------

    // 1. goto 9
    // 9. goto 21

    // 1. goto 21
    // 9. goto 21

    private void TraceGotoChains() {
      for (int index = 1; index < m_middleCodeList.Count; ++index) {
        MiddleCode middleCode = m_middleCodeList[index];

        if (middleCode.IsRelationCarryOrGoto()) {
          ISet<int> sourceSet = new HashSet<int>();
          sourceSet.Add(index);

          int firstTarget = (int)middleCode[0];
          int finalTarget = TraceGoto(firstTarget, sourceSet);

          if (firstTarget != finalTarget) {
            foreach (int source in sourceSet) {
              MiddleCode sourceCode = m_middleCodeList[source];
              sourceCode[0] = finalTarget;
            }

            m_update = true;
          }
        }
      }
    }

    private int TraceGoto(int target, ISet<int> sourceSet) {
      MiddleCode objectCode = m_middleCodeList[target];

      if (!sourceSet.Contains(target) && objectCode.IsGoto()) {
        sourceSet.Add(target);
        int nextTarget = (int)objectCode[0];
        return TraceGoto(nextTarget, sourceSet);
      }
      else {
        return target;
      }
    }

    // --------------------------------------------------------------------------

    private void ClearUnreachableCode() {
      ISet<MiddleCode> visitedSet = new HashSet<MiddleCode>();
      SearchReachableCode(0, visitedSet);

      for (int index = 0; index < (m_middleCodeList.Count - 1); ++index) {
        MiddleCode middleCode = m_middleCodeList[index];
        if (!visitedSet.Contains(middleCode)) {
          m_middleCodeList[index].Clear();
          m_update = true;
        }
      }
    }

    private void SearchReachableCode(int index, ISet<MiddleCode> visitedSet) {
      for (; index < m_middleCodeList.Count; ++index) {
        MiddleCode middleCode = m_middleCodeList[index];

        if (visitedSet.Contains(middleCode)) {
          return;
        }

        visitedSet.Add(middleCode);

        if (middleCode.IsRelation() || middleCode.IsCarry()) {
          int target = (int)middleCode[0];
          SearchReachableCode(target, visitedSet);
        }
        else if (middleCode.IsGoto()) {
          int target = (int)middleCode[0];
          SearchReachableCode(target, visitedSet);
          return;
        }
        else if (middleCode.getOperator() == MiddleOperator.Return) {
          if (m_middleCodeList[index + 1].getOperator() == MiddleOperator.Exit) {
            visitedSet.Add(m_middleCodeList[index + 1]);
          }

          return;
        }
        else if (middleCode.getOperator() == MiddleOperator.FunctionEnd) {
          Symbol funcSymbol = (Symbol)middleCode[0];
          Error.Check(funcSymbol.Type.ReturnType.IsVoid(),
                       funcSymbol.Name,
                       Message.Reached_the_end_of_a_non__void_function);
          return;
        }
      }
    }

    // Push x + Pop => empty

    // push x
    // pop

    // empty

    public void RemovePushPop() {
      for (int index = 0; index < (m_middleCodeList.Count - 1); ++index) {
        MiddleCode thisCode = m_middleCodeList[index],
                   nextCode = m_middleCodeList[index + 1];
        if ((thisCode.getOperator() == MiddleOperator.PushFloat) &&
            ((nextCode.getOperator() == MiddleOperator.PopEmpty) ||
             ((nextCode.getOperator() == MiddleOperator.PopFloat) &&
              (thisCode[0] == nextCode[0])))) {
          thisCode.Clear();
          nextCode.Clear();
          m_update = true;
        }
      }
    }

    // Pop x + Push x => Top x

    // pop x
    // push x

    // top x

    public void MergePopPushToTop() {
      ISet<int> targetIndexSet = new HashSet<int>();

      foreach (MiddleCode middleCode in m_middleCodeList) {
        if (middleCode.IsRelationCarryOrGoto()) {
          targetIndexSet.Add((int) middleCode[0]);
        }
      }

      for (int index = 0; index < (m_middleCodeList.Count - 1); ++index) {
        if (!targetIndexSet.Contains(index + 1)) {
          MiddleCode thisCode = m_middleCodeList[index],
                      nextCode = m_middleCodeList[index + 1];

          if ((thisCode.getOperator() == MiddleOperator.PopFloat) &&
              (nextCode.getOperator() == MiddleOperator.PushFloat) &&
              (thisCode[0] == nextCode[0])) {
            thisCode.setOperator(MiddleOperator.TopFloat);
            nextCode.Clear();
            m_update = true;
           }
        }
      }
    }

    // Top x + Pop => Pop x 7845

    // top x
    // pop

    // pop x

    public void MergeTopPopEmptyToPop() {
      for (int index = 0; index < (m_middleCodeList.Count - 1); ++index) {
        MiddleCode thisCode = m_middleCodeList[index],
                   nextCode = m_middleCodeList[index + 1];

        if ((thisCode.getOperator() == MiddleOperator.TopFloat) &&
            (nextCode.getOperator() == MiddleOperator.PopEmpty)) {
          thisCode.setOperator(MiddleOperator.PopFloat);
          nextCode.Clear();
          m_update = true;
        }
      }
    }

    // assign a = b => Pop a

    // a = b
    // pop a

    // t = b + c, a = t => a = b + c

    private void MergeBinary() {
      for (int index = 0; index < (m_middleCodeList.Count - 1); ++index) {
        MiddleCode thisCode = m_middleCodeList[index],
                   nextCode = m_middleCodeList[index + 1];

        if (thisCode.IsBinary() &&
            (nextCode.getOperator() == MiddleOperator.Assign)) {
          Symbol resultSymbol = (Symbol) thisCode[0];

          if (resultSymbol.IsTemporary() && (resultSymbol == nextCode[1])) {
            thisCode[0] = nextCode[0];
            nextCode.Clear();
            m_update = true;
          }
        }
      }
    }

    /*
       s = a + 3; t = *s
       t = a->(3)              
    */
    private void ConstantDereference() {
      for (int index = 0; index < (m_middleCodeList.Count - 1); ++index) {
        MiddleCode thisCode = m_middleCodeList[index],
                   nextCode = m_middleCodeList[index + 1];

        if (((thisCode.getOperator() == MiddleOperator.Add) ||
             (thisCode.getOperator() == MiddleOperator.Subtract)) &&
            (nextCode.getOperator() == MiddleOperator.Dereference) &&
            (thisCode[0] == nextCode[1])) {
          Symbol leftSymbol = (Symbol) thisCode[1],
                 rightSymbol = (Symbol) thisCode[2];

          if (leftSymbol.Value is BigInteger leftBigInteger) {
            Symbol resultSymbol = (Symbol) nextCode[0];
            resultSymbol.AddressSymbol = rightSymbol;
            resultSymbol.AddressOffset = (int) leftBigInteger;
            thisCode.Clear();
          }
          else if (rightSymbol.Value is BigInteger rightBigInteger) {
            Symbol resultSymbol = (Symbol) nextCode[0];
            resultSymbol.AddressSymbol = leftSymbol;

            if (thisCode.getOperator() == MiddleOperator.Add) {
              resultSymbol.AddressOffset = (int) rightBigInteger;
            }
            else {
              resultSymbol.AddressOffset = -(int) rightBigInteger;
            }

            thisCode.Clear();
          }
        }
      }
    }

    private void SematicOptimization() {
      for (int index = 0; index < m_middleCodeList.Count; ++index) {
        MiddleCode thisCode = m_middleCodeList[index];

        if (thisCode.IsBinary()) {
          Symbol resultSymbol = (Symbol)thisCode[0],
                 leftSymbol = (Symbol)thisCode[1],
                 rightSymbol = (Symbol)thisCode[2],
                 newSymbol = null;

          if ((leftSymbol.Value is BigInteger) && // t0 = 2 * 3
              (rightSymbol != null) && (rightSymbol.Value is BigInteger)) {
            newSymbol =
              ConstantExpression.ArithmeticIntegral(thisCode.getOperator(),
                                                    leftSymbol, rightSymbol);
          }
          // t0 = 0 + i
          else if ((thisCode.getOperator() == MiddleOperator.Add) &&
                    (leftSymbol.Value is BigInteger) &&
                    (leftSymbol.Value.Equals(BigInteger.Zero))) {
            newSymbol = rightSymbol;
          }
          // t0 = 0 - i; t0 = -i
          else if ((thisCode.getOperator() == MiddleOperator.Subtract) &&
                    (leftSymbol.Value is BigInteger) &&
                    (leftSymbol.Value.Equals(BigInteger.Zero))) {
            thisCode.setOperator(MiddleOperator.Minus);
            thisCode[0] = thisCode[1];
            thisCode[1] = null;
          }

          // t0 = i + 0
          // t0 = i - 0
          else if (((thisCode.getOperator() == MiddleOperator.Add) ||
                    (thisCode.getOperator() == MiddleOperator.Subtract)) &&
                    (rightSymbol != null) && (rightSymbol.Value is BigInteger) &&
                    (rightSymbol.Value.Equals(BigInteger.Zero))) {
            newSymbol = leftSymbol;
          }
          // t0 = 0 * i
          else if ((thisCode.getOperator() == MiddleOperator.Multiply) &&
                   (leftSymbol.Value is BigInteger) &&
                   (leftSymbol.Value.Equals(BigInteger.Zero))) {
            newSymbol = new Symbol(resultSymbol.Type, BigInteger.Zero);
          }
          // t0 = 1 * i
          else if ((thisCode.getOperator() == MiddleOperator.Multiply) &&
                   (leftSymbol.Value is BigInteger) &&
                   (leftSymbol.Value.Equals(BigInteger.One))) {
            newSymbol = rightSymbol;
          }
          // t0 = i * 0
          else if ((thisCode.getOperator() == MiddleOperator.Multiply) &&
                    (rightSymbol != null) && (rightSymbol.Value is BigInteger) &&
                    (rightSymbol.Value.Equals(BigInteger.Zero))) {
            newSymbol = new Symbol(resultSymbol.Type, BigInteger.Zero);
          }
          // t0 = -1 * i
          else if ((thisCode.getOperator() == MiddleOperator.Multiply) &&
                    (leftSymbol.Value is BigInteger) &&
                    (leftSymbol.Value.Equals(BigInteger.MinusOne))) {
            thisCode.setOperator(MiddleOperator.Minus);
            thisCode[0] = thisCode[1];
            thisCode[1] = null;
          }
          // t0 = i * -1
          else if ((thisCode.getOperator() == MiddleOperator.Multiply) &&
                    (rightSymbol != null) && (rightSymbol.Value is BigInteger) &&
                    (rightSymbol.Value.Equals(BigInteger.MinusOne))) {
            thisCode.setOperator(MiddleOperator.Minus);
            thisCode[1] = null;
          }

          // t0 = i * 1
          // t0 = i / 1
          // t0 = i % 1
          else if (((thisCode.getOperator() == MiddleOperator.Multiply) ||
                    (thisCode.getOperator() == MiddleOperator.Divide) ||
                    (thisCode.getOperator() == MiddleOperator.Modulo)) &&
                    (rightSymbol != null) && (rightSymbol.Value is BigInteger) &&
                    (rightSymbol.Value.Equals(BigInteger.One))) {
            newSymbol = leftSymbol;
          }

          if (newSymbol != null) {
            thisCode.setOperator(MiddleOperator.Assign); // i = 0 + j;
            thisCode[1] = newSymbol;                     // i = j;
            thisCode[2] = null;
            m_update = true;
          }
        }
      }
    }

    public static IDictionary<MiddleOperator, MiddleOperator> m_swapMap =
       new Dictionary<MiddleOperator, MiddleOperator>() {
        {MiddleOperator.Equal, MiddleOperator.Equal},
        {MiddleOperator.NotEqual, MiddleOperator.NotEqual},
        {MiddleOperator.LessThan, MiddleOperator.GreaterThan},
        {MiddleOperator.GreaterThan, MiddleOperator.LessThan},
        {MiddleOperator.LessThanEqual, MiddleOperator.GreaterThanEqual},
        {MiddleOperator.GreaterThanEqual, MiddleOperator.LessThanEqual},
      };

    // if 1 < x goto
    // if x > 1 goto
    private void OptimizeRelation() {
      foreach (MiddleCode middleCode in m_middleCodeList) {
        if (middleCode.IsRelation()) {
          Symbol leftSymbol = (Symbol) middleCode[1],
                 rightSymbol = (Symbol) middleCode[2];
          if ((leftSymbol.Value is BigInteger) ||
              (leftSymbol.Type.IsArrayFunctionOrString()) &&
              !rightSymbol.Type.IsArrayFunctionOrString() &&
              !(rightSymbol.Value is BigInteger)) {
            middleCode.setOperator(m_swapMap[middleCode.getOperator()]);
            middleCode[1] = rightSymbol;
            middleCode[2] = leftSymbol;
          }
        }
      }
    }

    // a = 1 + b => a = b + 1
    private void OptimizeCommutative() {
      foreach (MiddleCode middleCode in m_middleCodeList) {
        if (middleCode.IsCommutative()) {
          Symbol leftSymbol = (Symbol)middleCode[1];

          if (leftSymbol.Value is BigInteger) {
            Symbol rightSymbol = (Symbol)middleCode[2];
            middleCode[1] = rightSymbol;
            middleCode[2] = leftSymbol;
          }
        }
      }
    }

    // i = i + 1
    // ++i

    // i = i + (-1)
    // --i

    // i = i - 1
    // --i

    // i = i - (-1)
    // ++i

    // i = i
    // empty

    private void RemoveTemporaryAssign() {
      int outerIndex = 0;
      foreach (MiddleCode outerCode in m_middleCodeList) {
        if (outerCode.getOperator() == MiddleOperator.Assign) {
          Symbol resultSymbol = (Symbol)outerCode[0],
                 assignSymbol = (Symbol)outerCode[1];

          if (resultSymbol.IsTemporary() &&
              assignSymbol.IsTemporary()) {
            int innerIndex = 0;

            foreach (MiddleCode innerCode in m_middleCodeList) {
              if (innerIndex == outerIndex) {
                break;
              }

              if (innerCode[0] == assignSymbol) {
                innerCode[0] = resultSymbol;
              }

              ++innerIndex;
            }

            outerCode.Clear();
            m_update = true;
          }
        }

        ++outerIndex;
      }
    }

    private void RemoveTemporaryAccess() {
      ISet<Symbol> simpleSet = new HashSet<Symbol>(),
                   doubleSet = new HashSet<Symbol>();

      foreach (MiddleCode middleCode in m_middleCodeList) {
        if (middleCode[0] is Symbol resultSymbol) {
          if (simpleSet.Contains(resultSymbol)) {
            doubleSet.Add(resultSymbol);
          }
          else {
            simpleSet.Add(resultSymbol);
          }
        }
      }
      
      int outerIndex = 0;
      foreach (MiddleCode outerCode in m_middleCodeList) {
        if (outerCode.getOperator() == MiddleOperator.Assign) {
          Symbol resultSymbol = (Symbol) outerCode[0],
                 assignSymbol = (Symbol) outerCode[1];

          if (resultSymbol.IsTemporary() &&
              !doubleSet.Contains(resultSymbol)) {
            int minAssignIndex = -1, maxAccessIndex = -1;

            for (int innerIndex = outerIndex + 1;
                 innerIndex < m_middleCodeList.Count; ++innerIndex) {
              MiddleCode innerCode = m_middleCodeList[innerIndex];

              if ((innerCode[0] == assignSymbol) && (minAssignIndex  == -1)) {
                minAssignIndex = innerIndex;
              }

              if (((innerCode[1] == resultSymbol) ||
                   (innerCode[2] == resultSymbol)) &&
                  ((maxAccessIndex  == -1) ||
                   (innerIndex > maxAccessIndex))) {
                maxAccessIndex = innerIndex;
              }
            }

            if ((minAssignIndex == -1) || (minAssignIndex >= maxAccessIndex)){
              for (int innerIndex = outerIndex + 1;
                   innerIndex < m_middleCodeList.Count; ++innerIndex) {
                MiddleCode innerCode = m_middleCodeList[innerIndex];

                if (innerCode[1] == resultSymbol) {
                  innerCode[1] = assignSymbol;
                }
                if (innerCode[2] == resultSymbol) {
                  innerCode[2] = assignSymbol;
                }
              }

              outerCode.Clear();
              m_update = true;
            }
          }
        }

        ++outerIndex;
      }
    }

/*    private static ISet<Symbol> CloneSet(ISet<Symbol> inSet) {
      ISet<Symbol> outSet = new HashSet<Symbol>();

      foreach (Symbol symbol in inSet) {
        outSet.Add(symbol);
      }

      return outSet;
    }

    private void CheckIntegral() {
      Stack<ISet<Symbol>> integralSetStack = new Stack<ISet<Symbol>>();
      ISet<Symbol> integralSet = new HashSet<Symbol>();

      for (int line = 0; line < m_middleCodeList.Count; ++line) {
        MiddleCode middleCode = m_middleCodeList[line];

        object operand0 = middleCode[0],
               operand1 = middleCode[1],
               operand2 = middleCode[2];

        Symbol symbol0 = (operand0 is Symbol) ? ((Symbol) operand0) : null,
               symbol1 = (operand1 is Symbol) ? ((Symbol) operand1) : null,
               symbol2 = (operand2 is Symbol) ? ((Symbol) operand2) : null;

        switch (middleCode.getOperator()) {
          case MiddleOperator.PreCall: {
              middleCode[1] = CloneSet(integralSet);
              integralSetStack.Push(integralSet);
              integralSet = new HashSet<Symbol>();
            }
            break;

          case MiddleOperator.PostCall:
            integralSet = integralSetStack.Pop();
            break;

          case MiddleOperator.Dereference: {
              Symbol resultSymbol = (Symbol) middleCode[0];
              integralSet.Add(resultSymbol.AddressSymbol);
            }
            break;

          case MiddleOperator.Case:
            if (symbol1.AddressSymbol != null) {
              integralSet.Remove(symbol1.AddressSymbol);
            }
            break;

          case MiddleOperator.CaseEnd:
            integralSet.Remove(symbol0);
            break;

          default:
            if ((symbol0 != null) && symbol0.IsTemporary() &&
                (symbol0.AddressSymbol == null) &&
                symbol0.Type.IsIntegralPointerOrArray()) {
              integralSet.Add(symbol0);
            }

            if ((symbol1 != null) && symbol1.IsTemporary() &&
                symbol1.Type.IsIntegralPointerOrArray()) {
              integralSet.Remove(symbol1);
            }

            if ((symbol2 != null) && symbol2.IsTemporary() &&
                symbol2.Type.IsIntegralPointerOrArray()) {
              integralSet.Remove(symbol2);
            }

            if ((symbol0 != null) && (symbol0.AddressSymbol != null)) {
              integralSet.Remove(symbol0.AddressSymbol);
            }

            if ((symbol1 != null) && (symbol1.AddressSymbol != null)) {
              integralSet.Remove(symbol1.AddressSymbol);
            }

            if ((symbol2 != null) && (symbol2.AddressSymbol != null)) {
              integralSet.Remove(symbol2.AddressSymbol);
            }
            break;
        }
      }
    }

    private void CheckFloating() {
      int stackSize = 0;

      for (int line = 0; line < m_middleCodeList.Count; ++line) {
        MiddleCode middleCode = m_middleCodeList[line];

        object operand0 = middleCode[0],
               operand1 = middleCode[1],
               operand2 = middleCode[2];

        Symbol symbol0 = (operand0 is Symbol) ? ((Symbol) operand0) : null,
               symbol1 = (operand1 is Symbol) ? ((Symbol) operand1) : null,
               symbol2 = (operand2 is Symbol) ? ((Symbol) operand2) : null;

        switch (middleCode.getOperator()) {
          case MiddleOperator.PreCall:
            middleCode[2] = stackSize;
            break;

          case MiddleOperator.PushOne:
          case MiddleOperator.PushZero:
          case MiddleOperator.PushFloat:
          case MiddleOperator.IntegralToFloating:
            ++stackSize;
            break;
          
          case MiddleOperator.GetReturnValue:
            if (symbol0.Type.IsFloating()) {
              ++stackSize;
            }
            break;

          case MiddleOperator.PopFloat:
          case MiddleOperator.FloatingToIntegral:
          case MiddleOperator.DecreaseStack:
            --stackSize;
            break;

          case MiddleOperator.Return:
            stackSize = 0;
            break;

          case MiddleOperator.Equal:
          case MiddleOperator.NotEqual:
          case MiddleOperator.LessThan:
          case MiddleOperator.LessThanEqual:
          case MiddleOperator.GreaterThan:
          case MiddleOperator.GreaterThanEqual: {
              if (symbol1.Type.IsFloating()) {
                stackSize -= 2;
              }
            }
            break;

          case MiddleOperator.Add:
          case MiddleOperator.Subtract:
          case MiddleOperator.Multiply:
          case MiddleOperator.Divide:
            if (symbol1.Type.IsFloating()) {
              --stackSize;
            }
            break;

          case MiddleOperator.Parameter:
            if (symbol2.Type.IsFloating()) {
              --stackSize;
            }
            break;

        }
      }
    }*/

    private void RemoveTrivialAssign() {
      foreach (MiddleCode middleCode in m_middleCodeList) {
        MiddleOperator middleOperator = middleCode.getOperator();
      
        if (middleOperator == MiddleOperator.Assign) {
          Symbol resultSymbol = (Symbol) middleCode[0],
                 assignSymbol = (Symbol) middleCode[1];
        
          if (resultSymbol == assignSymbol) {
            middleCode.setOperator(MiddleOperator.Empty);
            m_update = true;
          }
        }
      }
    }

    public void RemoveClearedCode() {
      for (int index = (m_middleCodeList.Count - 1); index >= 0;--index){        
        if (m_middleCodeList[index].getOperator() == MiddleOperator.Empty) {
          foreach (MiddleCode middleCode in m_middleCodeList) {
            if (middleCode.IsRelationCarryOrGoto()) {
              int target = (int) middleCode[0];
            
              if (target > index) {
                middleCode[0] = target - 1;
              }
            }
          }
        
          m_middleCodeList.RemoveAt(index);
        }
      }
    }

    // x = (y + 1) * f(x);

    // t = y + 1
    // parameter x
    // call f
  }
}
