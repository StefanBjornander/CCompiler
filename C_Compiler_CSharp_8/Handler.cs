    private delegate void MiddleCodeHandler(MiddleCode middleCode, int middleIndex = 0);

    private static  Dictionary<MiddleOperator, MiddleCodeHandler> m_handlerMap = new() {
      { MiddleOperator.PreCall, FunctionPreCall },
      { MiddleOperator.Call, FunctionCall },
      { MiddleOperator.PostCall, FunctionPostCall },
      { MiddleOperator.Return, Return },
      { MiddleOperator.Exit, Exit },
      { MiddleOperator.Jump, Goto },

      { MiddleOperator.AssignRegister, LoadToRegister },
      { MiddleOperator.InspectRegister, InspectRegister },
      { MiddleOperator.JumpRegister, JumpToRegister },
      { MiddleOperator.Interrupt, Interrupt },
      { MiddleOperator.SysCall, SystemCall },

      { MiddleOperator.Initializer, Initializer },
      { MiddleOperator.InitializerZero, InitializerZero },
      { MiddleOperator.AssignInitSize, StructUnionAssignInit },

      { MiddleOperator.BitwiseAnd, IntegralBinary },
      { MiddleOperator.BitwiseXOr, IntegralBinary },
      { MiddleOperator.ShiftLeft, IntegralBinary },
      { MiddleOperator.ShiftRight, IntegralBinary },

      { MiddleOperator.Carry, CarryExpression },
      { MiddleOperator.NotCarry, CarryExpression },

      { MiddleOperator.Case, Case },
      { MiddleOperator.CaseEnd, CaseEnd },

      { MiddleOperator.Address, Address },
      { MiddleOperator.Dereference, Dereference },

      { MiddleOperator.IntegralToIntegral, IntegralToIntegral },
      { MiddleOperator.IntegralToFloating, IntegralToFloating },
      { MiddleOperator.FloatingToIntegral, FloatingToIntegral },

      { MiddleOperator.ParameterInitSize, StructUnionParameterInit },
      { MiddleOperator.DecreaseStack, DecreaseStack },

      { MiddleOperator.StackTop, StackTop },
      { MiddleOperator.PushZero, PushZero },
      { MiddleOperator.PushOne, PushOne },
      { MiddleOperator.PushFloat, PushFloat },
      { MiddleOperator.TopFloat, TopFloat },
      { MiddleOperator.PopEmpty, PopEmpty },
      { MiddleOperator.PopFloat, PopFloat },
    };
