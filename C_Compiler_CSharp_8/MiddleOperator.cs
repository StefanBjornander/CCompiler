namespace CCompiler {
  public enum MiddleOperator
   {AssignRegister, InspectRegister, StackTop,
    JumpRegister, SysCall, Interrupt,

    PushZero, PushOne, PushFloat, PopFloat, TopFloat, PopEmpty,

    Initializer, InitializerZero, AssignInitSize, Assign,

    LogicalOr, LogicalAnd,
    BitwiseNot, BitwiseOr, BitwiseXOr, BitwiseAnd,
    ShiftLeft, ShiftRight,

    Compare, Equal, NotEqual, LessThan,
    LessThanEqual, GreaterThan,
    GreaterThanEqual,

    Carry, NotCarry, Case, CaseEnd, Jump,

    Plus, Minus, Add, Subtract,

    Multiply, Divide, Modulo,

    Dot, Address, Dereference,

    PreCall, Call, PostCall, DecreaseStack,

    ParameterInitSize, Parameter,
    GetReturnValue, /*SetReturnValue,*/ Return, Exit,

    IntegralToIntegral, IntegralToFloating, FloatingToIntegral,

    FunctionEnd, Empty};
}