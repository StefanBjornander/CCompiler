    enum  CallSort{Integral, Floating, StructOrUnion};

    private static Dictionary<(MiddleOperator, CallSort), String> m_handlerSortMap = new() {
      { (MiddleOperator.Assign, CallSort.Integral), "IntegralAssign" },
      { (MiddleOperator.Assign, CallSort.Floating), "FloatingAssign" },
      { (MiddleOperator.Assign, CallSort.StructOrUnion), "StructUnionAssign" },

      { (MiddleOperator.Add, CallSort.Floating), "FloatingBinary" },
      { (MiddleOperator.Add, CallSort.Integral), "IntegralBinary" },
      { (MiddleOperator.Subtract, CallSort.Floating), "FloatingBinary" },
      { (MiddleOperator.Subtract, CallSort.Integral), "IntegralBinary" },

      { (MiddleOperator.Multiply, CallSort.Floating), "FloatingBinary" },
      { (MiddleOperator.Multiply, CallSort.Integral), "IntegralMultiply" },
      { (MiddleOperator.Divide, CallSort.Floating), "FloatingBinary" },
      { (MiddleOperator.Divide, CallSort.Integral), "IntegralMultiply" },
      { (MiddleOperator.Modulo, CallSort.Integral), "IntegralMultiply" },

      { (MiddleOperator.Equal, CallSort.Floating), "FloatingRelation" },
      { (MiddleOperator.Equal, CallSort.Integral), "IntegralRelation" },
      { (MiddleOperator.NotEqual, CallSort.Floating), "FloatingRelation" },
      { (MiddleOperator.NotEqual, CallSort.Integral), "IntegralRelation" },
      { (MiddleOperator.LessThan, CallSort.Floating), "FloatingRelation" },
      { (MiddleOperator.LessThan, CallSort.Integral), "IntegralRelation" },
      { (MiddleOperator.LessThanEqual, CallSort.Floating), "FloatingRelation" },
      { (MiddleOperator.LessThanEqual, CallSort.Integral), "IntegralRelation" },
      { (MiddleOperator.GreaterThan, CallSort.Floating), "FloatingRelation" },
      { (MiddleOperator.GreaterThan, CallSort.Integral), "IntegralRelation" },
      { (MiddleOperator.GreaterThanEqual, CallSort.Floating), "FloatingRelation" },
      { (MiddleOperator.GreaterThanEqual, CallSort.Integral), "IntegralRelation" },

      { (MiddleOperator.Minus, CallSort.Floating), "FloatingUnary" },
      { (MiddleOperator.Minus, CallSort.Integral), "IntegralUnary" },
      { (MiddleOperator.BitwiseNot, CallSort.Floating), "FloatingUnary" },
      { (MiddleOperator.BitwiseNot, CallSort.Integral), "IntegralUnary" },

      { (MiddleOperator.Parameter, CallSort.Floating), "FloatingParameter" },
      { (MiddleOperator.Parameter, CallSort.StructOrUnion), "StructUnionParameter" },
      { (MiddleOperator.Parameter, CallSort.Integral), "IntegralParameter" },

      { (MiddleOperator.GetReturnValue, CallSort.Floating), "FloatingGetReturnValue" },
      { (MiddleOperator.GetReturnValue, CallSort.StructOrUnion), "StructUnionGetReturnValue" },
      { (MiddleOperator.GetReturnValue, CallSort.Integral), "IntegralGetReturnValue" },
    };

    private static Dictionary<MiddleOperator, String> m_handlerMap = new() {
      { MiddleOperator.PreCall, "FunctionPreCall" },
      { MiddleOperator.Call, "FunctionCall" },
      { MiddleOperator.PostCall, "FunctionPostCall" },
      { MiddleOperator.Return, "Return" },
      { MiddleOperator.Exit, "Exit" },
      { MiddleOperator.Jump, "Goto" },

      { MiddleOperator.AssignRegister, "LoadToRegister" },
      { MiddleOperator.InspectRegister, "InspectRegister" },
      { MiddleOperator.JumpRegister, "JumpToRegister" },
      { MiddleOperator.Interrupt, "Interrupt" },
      { MiddleOperator.SysCall, "SystemCall" },

      { MiddleOperator.Initializer, "InitializerX" },
      { MiddleOperator.InitializerZero, "InitializerZeroX" },
      { MiddleOperator.AssignInitSize, "StructUnionAssignInit" },

      { MiddleOperator.BitwiseAnd, "IntegralBinary" },
      { MiddleOperator.BitwiseXOr, "IntegralBinary" },
      { MiddleOperator.ShiftLeft, "IntegralBinary" },
      { MiddleOperator.ShiftRight, "IntegralBinary" },

      { MiddleOperator.Carry, "CarryExpression" },
      { MiddleOperator.NotCarry, "CarryExpression" },

      { MiddleOperator.Case, "Case" },
      { MiddleOperator.CaseEnd, "CaseEnd" },

      { MiddleOperator.Address, "Address" },
      { MiddleOperator.Dereference, "Dereference" },

      { MiddleOperator.IntegralToIntegral, "IntegralToIntegral" },
      { MiddleOperator.IntegralToFloating, "IntegralToFloating" },
      { MiddleOperator.FloatingToIntegral, "FloatingToIntegral" },

      { MiddleOperator.ParameterInitSize, "StructUnionParameterInit" },
      { MiddleOperator.DecreaseStack, "DecreaseStack" },

      { MiddleOperator.PushZero, "PushZero" },
      { MiddleOperator.PushOne, "PushOne" },
      { MiddleOperator.PushFloat, "PushFloat" },
      { MiddleOperator.TopFloat, "TopFloat" },
      { MiddleOperator.PopEmpty, "PopEmpty" },
      { MiddleOperator.PopFloat, "PopFloat" },
      { MiddleOperator.StackTop, "StackTop" },
      { MiddleOperator.FunctionEnd, "FunctionEnd" }
    };
