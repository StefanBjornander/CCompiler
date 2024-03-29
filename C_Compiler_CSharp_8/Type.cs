﻿//using System;
//using System.Linq;
//using System.Numerics;
//using System.Collections.Generic;

namespace CCompiler {
  public class Type {
    private readonly Sort m_sort;

    public Type(Sort sort) {
      m_sort = sort;
    }

    public Sort Sort {
      get { return m_sort; }
    }

    public bool IsVoid() {
      return (m_sort == Sort.Void);
    }

    public bool IsChar() {
      return (m_sort == Sort.SignedChar) || (m_sort == Sort.UnsignedChar);
    }

    public bool IsShort() {
      return (m_sort == Sort.SignedShortInt) ||
             (m_sort == Sort.UnsignedShortInt);
    }

    public bool IsInteger() {
      return (m_sort == Sort.SignedInt) || (m_sort == Sort.UnsignedInt);
    }

    public bool IsSigned() {
      return (m_sort == Sort.SignedChar) || (m_sort == Sort.SignedShortInt) ||
             (m_sort == Sort.SignedInt) || (m_sort == Sort.SignedLongInt);
    }

    public bool IsUnsigned() {
      return (m_sort == Sort.UnsignedChar) || (m_sort == Sort.UnsignedShortInt)
          || (m_sort == Sort.UnsignedInt) || (m_sort == Sort.UnsignedLongInt);
    }

    public bool IsIntegral() {
      return IsSigned() || IsUnsigned();
    }

    public bool IsFloat() {
      return (m_sort == Sort.Float);
    }
  
    public bool IsFloating() {
      return (m_sort == Sort.Float) || (m_sort == Sort.Double) ||
             (m_sort == Sort.LongDouble);
    }

    public bool IsArithmetic() {
      return IsIntegral() || IsFloating();
    }

    public bool IsString() {
      return (m_sort == Sort.String);
    }

    public bool IsLogical() {
      return (m_sort == Sort.Logical);
    }

    // ------------------------------------------------------------------------

    private readonly ISet<Symbol> m_enumeratorItemSet = null;

    /*public static Type enumeratorType(ISet<Symbol> enumeratorItemSet) {
      Type type = new();
      type.m_sort = Sort.SignedInt;
      type.m_enumeratorItemSet = enumeratorItemSet;
      return type;
    }*/

    public Type(ISet<Symbol> enumeratorItemSet) {
      m_sort = Sort.SignedInt;
      m_enumeratorItemSet = enumeratorItemSet;
    }

    public bool IsEnumerator() {
      return (m_enumeratorItemSet != null);
    }

    public ISet<Symbol> EnumeratorItemSet {
      get { return m_enumeratorItemSet; }
    }

    // ------------------------------------------------------------------------
  
    private BigInteger? m_bitfieldMask = null;

    public bool IsBitfield() {
      return (m_bitfieldMask != null);
    }

    public BigInteger BitfieldMask {
      get {return m_bitfieldMask.Value;}
    }

    public void SetBitfieldMask(int bits) {
      m_bitfieldMask = (BigInteger) (Math.Pow(2, bits) - 1);
    }

    // ------------------------------------------------------------------------
  
    private Type m_pointerType;

    public Type(Type pointerType) {
      m_sort = Sort.Pointer;
      m_pointerType = pointerType;
    }

    public bool IsPointer() {
      return (m_sort == Sort.Pointer);
    }

    public Type PointerType {
      get { return m_pointerType; }
      set { m_pointerType = value; }
    }

    public bool IsIntegralOrPointer() {
      return IsIntegral() || IsPointer();
    }

    public bool IsArithmeticOrPointer() {
      return IsArithmetic() || IsPointer();
    }

    // ------------------------------------------------------------------------
  
    private int m_arraySize;
    private Type m_arrayType;

    public Type(int arraySize, Type arrayType) {
      m_sort = Sort.Array;
      m_arraySize = arraySize;
      m_arrayType = arrayType;
    }

    public bool IsArray() {
      return (m_sort == Sort.Array);
    }

    public int ArraySize {
      get { return m_arraySize; }
      set { m_arraySize = value; }
    }

    public Type ArrayType {
      get { return m_arrayType; }
      set { m_arrayType = value; }
    }

    public bool IsPointerOrArray() {
      return IsPointer() || IsArray();
    }

    public Type PointerOrArrayType {
      get { return IsPointer() ? PointerType : ArrayType; }
    }

    public bool IsPointerArrayOrString() {
      return IsPointerOrArray() || IsString();
    }

    public Type PointerArrayOrStringType {
      get { return IsString() ? Type.SignedCharType : PointerOrArrayType; }
    }

    public bool IsIntegralPointerOrArray() {
      return IsIntegral() || IsPointerOrArray();
    }

    // ------------------------------------------------------------------------
  
    private IDictionary<string,Symbol> m_memberMap;
    private List<Symbol> m_memberList;
    
    public Type(Sort sort, IDictionary<string,Symbol> symbolMap,
                List<Symbol> symbolList) {
      m_sort = sort;
      m_memberMap = symbolMap;
      m_memberList = symbolList;
    }

    public IDictionary<string,Symbol> MemberMap {
      get { return m_memberMap; }
      set { m_memberMap = value; }
    }

    public List<Symbol> MemberList {
      get { return m_memberList; }
      set { m_memberList = value; }
    }

    public bool IsStructOrUnion() {
      return (m_sort == Sort.Struct) || (m_sort == Sort.Union);
    }
  
    // ------------------------------------------------------------------------

    private Type m_returnType;
    private readonly List<string> m_nameList;

    public Type(List<string> nameList) {
      m_sort = Sort.Function;
      m_nameList = nameList;

      HashSet<string> nameSet = new();
      foreach (string name in nameList) {
        Error.Check(nameSet.Add(name), name, Message.Name_already_defined);
      }
    }

    private readonly List<Symbol> m_parameterList;
    private readonly List<Type> m_typeList;
    private readonly bool m_variadic;

    public Type(List<Symbol> parameterList, bool variadic) {
      m_sort = Sort.Function;
      m_parameterList = parameterList;
      m_variadic = variadic;
      m_nameList = null;

      if (m_parameterList != null) {
        m_typeList = new();

        foreach (Symbol symbol in m_parameterList) {
          m_typeList.Add(symbol.Type);
        }
      }
      else {
        m_typeList = null;
      }
    }

    public bool IsFunction() {
      return (m_sort == Sort.Function);
    }

    public bool IsOldStyle() {
      return (m_nameList != null);
    }

    public List<string> NameList {
      get { return m_nameList; }
    }

    public List<Symbol> ParameterList {
      get { return m_parameterList; }
    }

    public List<Type> TypeList {
      get { return m_typeList; }
    }

    public Type ReturnType {
      get { return m_returnType; }
      set { m_returnType = value; }
    }

    public bool IsVariadic() {
      return m_variadic;
    }

    public bool IsArrayFunctionOrString() {
      return IsArray() || IsFunction() || IsString();
    }

    public bool IsPointerArrayStringOrFunction() {
      return IsPointer() || IsArrayFunctionOrString ();
    }

    public bool IsIntegralPointerOrFunction() {
      return IsIntegralOrPointer() || IsFunction();
    }

    public bool IsIntegralPointerArrayOrFunction() {
      return IsIntegralPointerOrArray() || IsFunction();
    }

    public bool IsIntegralPointerArrayStringOrFunction() {
      return IsIntegralPointerArrayOrFunction() || IsString();
    }

    // ------------------------------------------------------------------------

    public int Size() {
      switch (m_sort) {
        case Sort.Array:
          return m_arraySize * m_arrayType.Size();

        case Sort.Struct:
            if (m_memberMap != null) {
              int size = 0;

              foreach (Symbol symbol in m_memberMap.Values) {
                size += symbol.Type.Size();
              }

              return size;
            }        

            return 0;

        case Sort.Union:
            if (m_memberMap == null) {
              int size = 0;

              foreach (Symbol symbol in m_memberMap.Values) {
                size = Math.Max(size, symbol.Type.Size());
              }

              return size;
            }

            return 0;

        case Sort.Logical:
            return TypeSize.SignedIntegerSize;

        default:
          return TypeSize.Size(m_sort);
      }
    }

    public int ReturnSize() {
      switch (m_sort) {
        case Sort.Struct:
        case Sort.Union:
        case Sort.Array:
          return TypeSize.PointerSize;

        default:
          return Size();
      }
    }

    public int SizeAddress() {
      switch (m_sort) {
        case Sort.Array:
        case Sort.Function:
        case Sort.String:
          return TypeSize.PointerSize;

        default:
          return Size();
      }
    }

    public bool IsComplete() {
      switch (m_sort) {
        case Sort.Array:
          return (m_arraySize > 0);

        case Sort.Struct:
        case Sort.Union:
          return (m_memberMap != null);

        default:
          return true;
      }
    }

    // ------------------------------------------------------------------------

    private bool m_constant, m_volatile;

    public bool Constant {
      get { return m_constant; }
      set { m_constant = value; }
    }
  
    public bool Volatile {
      get { return m_volatile; }
      set { m_volatile = value; }
    }

    public bool IsConstantRecursive() {
      if (m_constant) {
        return true;
      }  
      else if (IsStructOrUnion() && (m_memberMap != null)) {
        foreach (Symbol symbol in m_memberMap.Values) {
          if (symbol.Type.IsConstantRecursive()) {
            return true;
          }
        }
      }
    
      return false;
    }

    // ------------------------------------------------------------------------
  
    public override int GetHashCode() {
      return base.GetHashCode();
    }

    public override bool Equals(object obj) {
      if (obj is Type type) {
        if ((m_constant == type.m_constant) &&
            (m_volatile == type.m_volatile) && (m_sort == type.m_sort)) {
          switch (m_sort) {
            case Sort.Pointer:
              return m_pointerType.Equals(type.m_pointerType);

            case Sort.Array:
              return ((m_arraySize == 0) || (type.m_arraySize == 0) ||
                      (m_arraySize == type.m_arraySize)) &&
                      m_arrayType.Equals(type.m_arrayType);

            case Sort.Struct:
            case Sort.Union:
              return ((m_memberMap == null) && (type.m_memberMap == null)) ||
                     ((m_memberMap != null) && (type.m_memberMap != null) &&
                      m_memberMap.Equals(type.m_memberMap));

            case Sort.Function:
              return m_returnType.Equals(type.m_returnType) &&
                     (((m_typeList == null) && (type.m_typeList == null)) ||
                      ((m_typeList != null) && (type.m_typeList != null) &&
                     m_typeList.SequenceEqual(type.m_typeList))) &&
                     (m_variadic == type.m_variadic);

            default:
              return true;
          }
        }
      }

      return false;
    }

    // ------------------------------------------------------------------------
  
    public override string ToString() {
      return Enum.GetName(typeof(Sort), m_sort).ToLower().
             Replace("short", "short ").Replace("long", "long ").
             Replace("signed", "signed ");
    }

    public static Type SignedShortIntegerType = new(Sort.SignedShortInt);
    public static Type UnsignedShortIntegerType = new(Sort.UnsignedShortInt);
    public static Type SignedIntegerType = new(Sort.SignedInt);
    public static Type UnsignedIntegerType = new(Sort.UnsignedInt);
    public static Type SignedLongIntegerType = new(Sort.SignedLongInt);
    public static Type UnsignedLongIntegerType = new(Sort.UnsignedLongInt);
    public static Type FloatType = new(Sort.Float);
    public static Type DoubleType = new(Sort.Double);
    public static Type LongDoubleType = new(Sort.LongDouble);
    public static Type SignedCharType = new(Sort.SignedChar);
    public static Type UnsignedCharType = new(Sort.UnsignedChar);
    public static Type StringType = new(Sort.String);
    public static Type IntegerPointerType = new(SignedIntegerType);
    public static Type VoidPointerType = new(new Type(Sort.Void));
    public static Type LogicalType = new(Sort.Logical);
  }
}
