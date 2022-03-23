// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  LAPTOP-7Q575VHS
// DateTime: 2022-03-23 12:14:00
// UserName: stefa
// Input file <ExpressionParser.gppg - 2022-03-23 12:13:21>

// options: lines gplex

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using CCompiler;

namespace CCompiler_Exp
{
public enum Tokens {error=2,EOF=3,DEFINED=4,ADD=5,SUBTRACT=6,
    MULTIPLY=7,DIVIDE=8,MODULO=9,EQUAL=10,NOT_EQUAL=11,LESS_THAN=12,
    LESS_THAN_EQUAL=13,GREATER_THAN=14,GREATER_THAN_EQUAL=15,LEFT_SHIFT=16,RIGHT_SHIFT=17,QUESTION_MARK=18,
    COLON=19,LEFT_PARENTHESIS=20,RIGHT_PARENTHESIS=21,LOGICAL_OR=22,LOGICAL_AND=23,LOGICAL_NOT=24,
    BITWISE_XOR=25,BITWISE_OR=26,BITWISE_AND=27,BITWISE_NOT=28,EOL=29,NAME=30,
    VALUE=31};

public partial struct ValueType
#line 15 "ExpressionParser.gppg"
       {
  public string name;
  public int integer_value;
}
#line default
// Abstract base class for GPLEX scanners
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

// Utility class for encapsulating token information
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public class ScanObj {
  public int token;
  public ValueType yylval;
  public LexLocation yylloc;
  public ScanObj( int t, ValueType val, LexLocation loc ) {
    this.token = t; this.yylval = val; this.yylloc = loc;
  }
}

[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public partial class Parser: ShiftReduceParser<ValueType, LexLocation>
{
  // Verbatim content from ExpressionParser.gppg - 2022-03-23 12:13:21
#line 7 "ExpressionParser.gppg"
  // Empty.
#line default
  // End verbatim content from ExpressionParser.gppg - 2022-03-23 12:13:21

#pragma warning disable 649
  private static Dictionary<int, string> aliases;
#pragma warning restore 649
  private static Rule[] rules = new Rule[42];
  private static State[] states = new State[74];
  private static string[] nonTerms = new string[] {
      "expression", "logical_or_expression", "logical_and_expression", "bitwise_or_expression", 
      "bitwise_xor_expression", "bitwise_and_expression", "equality_expression", 
      "relation_expression", "shift_expression", "add_expression", "multiply_expression", 
      "prefix_expression", "primary_expression", "$accept", };

  static Parser() {
    states[0] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-1,1,-2,3,-3,34,-4,6,-5,35,-6,36,-7,40,-8,69,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{22,4,18,70,3,-2,21,-2,19,-2});
    states[4] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-3,5,-4,6,-5,35,-6,36,-7,40,-8,69,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[5] = new State(-5);
    states[6] = new State(new int[]{26,7,22,-6,18,-6,3,-6,21,-6,19,-6});
    states[7] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-5,8,-6,39,-7,40,-8,69,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[8] = new State(new int[]{25,9,26,-9,22,-9,18,-9,3,-9,21,-9,19,-9});
    states[9] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-6,10,-7,40,-8,69,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[10] = new State(new int[]{27,11,25,-11,26,-11,22,-11,18,-11,3,-11,21,-11,19,-11});
    states[11] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-7,12,-8,69,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[12] = new State(new int[]{10,13,11,41,27,-13,23,-13,25,-13,26,-13,22,-13,18,-13,3,-13,21,-13,19,-13});
    states[13] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-8,14,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[14] = new State(new int[]{12,15,13,43,14,64,15,66,10,-15,11,-15,27,-15,23,-15,25,-15,26,-15,22,-15,18,-15,3,-15,21,-15,19,-15});
    states[15] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-9,16,-10,63,-11,62,-12,61,-13,23});
    states[16] = new State(new int[]{16,17,17,45,12,-18,13,-18,14,-18,15,-18,10,-18,11,-18,27,-18,23,-18,25,-18,26,-18,22,-18,18,-18,3,-18,21,-18,19,-18});
    states[17] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-10,18,-11,62,-12,61,-13,23});
    states[18] = new State(new int[]{5,19,6,47,16,-23,17,-23,12,-23,13,-23,14,-23,15,-23,10,-23,11,-23,27,-23,23,-23,25,-23,26,-23,22,-23,18,-23,3,-23,21,-23,19,-23});
    states[19] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-11,20,-12,61,-13,23});
    states[20] = new State(new int[]{7,21,8,49,9,59,5,-26,6,-26,16,-26,17,-26,12,-26,13,-26,14,-26,15,-26,10,-26,11,-26,27,-26,23,-26,25,-26,26,-26,22,-26,18,-26,3,-26,21,-26,19,-26});
    states[21] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-12,22,-13,23});
    states[22] = new State(-29);
    states[23] = new State(-32);
    states[24] = new State(-37);
    states[25] = new State(-38);
    states[26] = new State(new int[]{30,27,20,28});
    states[27] = new State(-39);
    states[28] = new State(new int[]{30,29});
    states[29] = new State(new int[]{21,30});
    states[30] = new State(-40);
    states[31] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-1,32,-2,3,-3,34,-4,6,-5,35,-6,36,-7,40,-8,69,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[32] = new State(new int[]{21,33});
    states[33] = new State(-41);
    states[34] = new State(-4);
    states[35] = new State(new int[]{25,9,26,-8,22,-8,18,-8,3,-8,21,-8,19,-8});
    states[36] = new State(new int[]{27,11,23,37,25,-10,26,-10,22,-10,18,-10,3,-10,21,-10,19,-10});
    states[37] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-4,38,-5,35,-6,39,-7,40,-8,69,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[38] = new State(new int[]{26,7,22,-7,18,-7,3,-7,21,-7,19,-7});
    states[39] = new State(new int[]{27,11,25,-10,26,-10,22,-10,18,-10,3,-10,21,-10,19,-10});
    states[40] = new State(new int[]{10,13,11,41,27,-12,23,-12,25,-12,26,-12,22,-12,18,-12,3,-12,21,-12,19,-12});
    states[41] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-8,42,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[42] = new State(new int[]{12,15,13,43,14,64,15,66,10,-16,11,-16,27,-16,23,-16,25,-16,26,-16,22,-16,18,-16,3,-16,21,-16,19,-16});
    states[43] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-9,44,-10,63,-11,62,-12,61,-13,23});
    states[44] = new State(new int[]{16,17,17,45,12,-19,13,-19,14,-19,15,-19,10,-19,11,-19,27,-19,23,-19,25,-19,26,-19,22,-19,18,-19,3,-19,21,-19,19,-19});
    states[45] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-10,46,-11,62,-12,61,-13,23});
    states[46] = new State(new int[]{5,19,6,47,16,-24,17,-24,12,-24,13,-24,14,-24,15,-24,10,-24,11,-24,27,-24,23,-24,25,-24,26,-24,22,-24,18,-24,3,-24,21,-24,19,-24});
    states[47] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-11,48,-12,61,-13,23});
    states[48] = new State(new int[]{7,21,8,49,9,59,5,-27,6,-27,16,-27,17,-27,12,-27,13,-27,14,-27,15,-27,10,-27,11,-27,27,-27,23,-27,25,-27,26,-27,22,-27,18,-27,3,-27,21,-27,19,-27});
    states[49] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-12,50,-13,23});
    states[50] = new State(-30);
    states[51] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-12,52,-13,23});
    states[52] = new State(-33);
    states[53] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-12,54,-13,23});
    states[54] = new State(-34);
    states[55] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-12,56,-13,23});
    states[56] = new State(-35);
    states[57] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-12,58,-13,23});
    states[58] = new State(-36);
    states[59] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-12,60,-13,23});
    states[60] = new State(-31);
    states[61] = new State(-28);
    states[62] = new State(new int[]{7,21,8,49,9,59,5,-25,6,-25,16,-25,17,-25,12,-25,13,-25,14,-25,15,-25,10,-25,11,-25,27,-25,23,-25,25,-25,26,-25,22,-25,18,-25,3,-25,21,-25,19,-25});
    states[63] = new State(new int[]{5,19,6,47,16,-22,17,-22,12,-22,13,-22,14,-22,15,-22,10,-22,11,-22,27,-22,23,-22,25,-22,26,-22,22,-22,18,-22,3,-22,21,-22,19,-22});
    states[64] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-9,65,-10,63,-11,62,-12,61,-13,23});
    states[65] = new State(new int[]{16,17,17,45,12,-20,13,-20,14,-20,15,-20,10,-20,11,-20,27,-20,23,-20,25,-20,26,-20,22,-20,18,-20,3,-20,21,-20,19,-20});
    states[66] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-9,67,-10,63,-11,62,-12,61,-13,23});
    states[67] = new State(new int[]{16,17,17,45,12,-21,13,-21,14,-21,15,-21,10,-21,11,-21,27,-21,23,-21,25,-21,26,-21,22,-21,18,-21,3,-21,21,-21,19,-21});
    states[68] = new State(new int[]{16,17,17,45,12,-17,13,-17,14,-17,15,-17,10,-17,11,-17,27,-17,23,-17,25,-17,26,-17,22,-17,18,-17,3,-17,21,-17,19,-17});
    states[69] = new State(new int[]{12,15,13,43,14,64,15,66,10,-14,11,-14,27,-14,23,-14,25,-14,26,-14,22,-14,18,-14,3,-14,21,-14,19,-14});
    states[70] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-1,71,-2,3,-3,34,-4,6,-5,35,-6,36,-7,40,-8,69,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[71] = new State(new int[]{19,72});
    states[72] = new State(new int[]{30,24,31,25,4,26,20,31,5,51,6,53,28,55,24,57},new int[]{-1,73,-2,3,-3,34,-4,6,-5,35,-6,36,-7,40,-8,69,-9,68,-10,63,-11,62,-12,61,-13,23});
    states[73] = new State(-3);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-14, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-2});
    rules[3] = new Rule(-1, new int[]{-2,18,-1,19,-1});
    rules[4] = new Rule(-2, new int[]{-3});
    rules[5] = new Rule(-2, new int[]{-2,22,-3});
    rules[6] = new Rule(-3, new int[]{-4});
    rules[7] = new Rule(-3, new int[]{-6,23,-4});
    rules[8] = new Rule(-4, new int[]{-5});
    rules[9] = new Rule(-4, new int[]{-4,26,-5});
    rules[10] = new Rule(-5, new int[]{-6});
    rules[11] = new Rule(-5, new int[]{-5,25,-6});
    rules[12] = new Rule(-6, new int[]{-7});
    rules[13] = new Rule(-6, new int[]{-6,27,-7});
    rules[14] = new Rule(-7, new int[]{-8});
    rules[15] = new Rule(-7, new int[]{-7,10,-8});
    rules[16] = new Rule(-7, new int[]{-7,11,-8});
    rules[17] = new Rule(-8, new int[]{-9});
    rules[18] = new Rule(-8, new int[]{-8,12,-9});
    rules[19] = new Rule(-8, new int[]{-8,13,-9});
    rules[20] = new Rule(-8, new int[]{-8,14,-9});
    rules[21] = new Rule(-8, new int[]{-8,15,-9});
    rules[22] = new Rule(-9, new int[]{-10});
    rules[23] = new Rule(-9, new int[]{-9,16,-10});
    rules[24] = new Rule(-9, new int[]{-9,17,-10});
    rules[25] = new Rule(-10, new int[]{-11});
    rules[26] = new Rule(-10, new int[]{-10,5,-11});
    rules[27] = new Rule(-10, new int[]{-10,6,-11});
    rules[28] = new Rule(-11, new int[]{-12});
    rules[29] = new Rule(-11, new int[]{-11,7,-12});
    rules[30] = new Rule(-11, new int[]{-11,8,-12});
    rules[31] = new Rule(-11, new int[]{-11,9,-12});
    rules[32] = new Rule(-12, new int[]{-13});
    rules[33] = new Rule(-12, new int[]{5,-12});
    rules[34] = new Rule(-12, new int[]{6,-12});
    rules[35] = new Rule(-12, new int[]{28,-12});
    rules[36] = new Rule(-12, new int[]{24,-12});
    rules[37] = new Rule(-13, new int[]{30});
    rules[38] = new Rule(-13, new int[]{31});
    rules[39] = new Rule(-13, new int[]{4,30});
    rules[40] = new Rule(-13, new int[]{4,20,30,21});
    rules[41] = new Rule(-13, new int[]{20,-1,21});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
#pragma warning disable 162, 1522
    switch (action)
    {
      case 2: // expression -> logical_or_expression
#line 38 "ExpressionParser.gppg"
                           {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
      Preprocessor.PreProcessorResult = CurrentSemanticValue.integer_value;
    }
#line default
        break;
      case 3: // expression -> logical_or_expression, QUESTION_MARK, expression, COLON, 
              //               expression
#line 43 "ExpressionParser.gppg"
                                           {
      CurrentSemanticValue.integer_value = (ValueStack[ValueStack.Depth-5].integer_value != 0) ? ValueStack[ValueStack.Depth-3].integer_value : ValueStack[ValueStack.Depth-1].integer_value;
      Preprocessor.PreProcessorResult = CurrentSemanticValue.integer_value;
    }
#line default
        break;
      case 4: // logical_or_expression -> logical_and_expression
#line 49 "ExpressionParser.gppg"
                            {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 5: // logical_or_expression -> logical_or_expression, LOGICAL_OR, 
              //                          logical_and_expression
#line 52 "ExpressionParser.gppg"
                                                            {
      CurrentSemanticValue.integer_value = ((ValueStack[ValueStack.Depth-3].integer_value != 0) || (ValueStack[ValueStack.Depth-1].integer_value != 0)) ? 1 : 0;
    }
#line default
        break;
      case 6: // logical_and_expression -> bitwise_or_expression
#line 57 "ExpressionParser.gppg"
                           {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 7: // logical_and_expression -> bitwise_and_expression, LOGICAL_AND, 
              //                           bitwise_or_expression
#line 60 "ExpressionParser.gppg"
                                                             {
      CurrentSemanticValue.integer_value = ((ValueStack[ValueStack.Depth-3].integer_value != 0) && (ValueStack[ValueStack.Depth-1].integer_value != 0)) ? 1 : 0;
    }
#line default
        break;
      case 8: // bitwise_or_expression -> bitwise_xor_expression
#line 65 "ExpressionParser.gppg"
                            {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 9: // bitwise_or_expression -> bitwise_or_expression, BITWISE_OR, 
              //                          bitwise_xor_expression
#line 68 "ExpressionParser.gppg"
                                                            {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-3].integer_value | ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 10: // bitwise_xor_expression -> bitwise_and_expression
#line 73 "ExpressionParser.gppg"
                            {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 11: // bitwise_xor_expression -> bitwise_xor_expression, BITWISE_XOR, 
               //                           bitwise_and_expression
#line 76 "ExpressionParser.gppg"
                                                              {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-3].integer_value ^ ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 12: // bitwise_and_expression -> equality_expression
#line 81 "ExpressionParser.gppg"
                        {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 13: // bitwise_and_expression -> bitwise_and_expression, BITWISE_AND, 
               //                           equality_expression
#line 84 "ExpressionParser.gppg"
                                                           {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-3].integer_value & ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 14: // equality_expression -> relation_expression
#line 89 "ExpressionParser.gppg"
                        {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 15: // equality_expression -> equality_expression, EQUAL, relation_expression
#line 92 "ExpressionParser.gppg"
                                                  {      
      CurrentSemanticValue.integer_value = (ValueStack[ValueStack.Depth-3].integer_value == ValueStack[ValueStack.Depth-1].integer_value) ? 1 : 0;
    }
#line default
        break;
      case 16: // equality_expression -> equality_expression, NOT_EQUAL, relation_expression
#line 95 "ExpressionParser.gppg"
                                                      {      
      CurrentSemanticValue.integer_value = (ValueStack[ValueStack.Depth-3].integer_value != ValueStack[ValueStack.Depth-1].integer_value) ? 1 : 0;
    }
#line default
        break;
      case 17: // relation_expression -> shift_expression
#line 100 "ExpressionParser.gppg"
                      { CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value; }
#line default
        break;
      case 18: // relation_expression -> relation_expression, LESS_THAN, shift_expression
#line 101 "ExpressionParser.gppg"
                                                   {
      CurrentSemanticValue.integer_value = (ValueStack[ValueStack.Depth-3].integer_value < ValueStack[ValueStack.Depth-1].integer_value) ? 1 : 0;
    }
#line default
        break;
      case 19: // relation_expression -> relation_expression, LESS_THAN_EQUAL, shift_expression
#line 104 "ExpressionParser.gppg"
                                                         {
      CurrentSemanticValue.integer_value = (ValueStack[ValueStack.Depth-3].integer_value <= ValueStack[ValueStack.Depth-1].integer_value) ? 1 : 0;
    }
#line default
        break;
      case 20: // relation_expression -> relation_expression, GREATER_THAN, shift_expression
#line 107 "ExpressionParser.gppg"
                                                      {
      CurrentSemanticValue.integer_value = (ValueStack[ValueStack.Depth-3].integer_value > ValueStack[ValueStack.Depth-1].integer_value) ? 1 : 0;
    }
#line default
        break;
      case 21: // relation_expression -> relation_expression, GREATER_THAN_EQUAL, 
               //                        shift_expression
#line 110 "ExpressionParser.gppg"
                                                            {
      CurrentSemanticValue.integer_value = (ValueStack[ValueStack.Depth-3].integer_value >=ValueStack[ValueStack.Depth-1].integer_value) ? 1 : 0;
    }
#line default
        break;
      case 22: // shift_expression -> add_expression
#line 115 "ExpressionParser.gppg"
                   {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 23: // shift_expression -> shift_expression, LEFT_SHIFT, add_expression
#line 118 "ExpressionParser.gppg"
                                               {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-3].integer_value << ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 24: // shift_expression -> shift_expression, RIGHT_SHIFT, add_expression
#line 121 "ExpressionParser.gppg"
                                                {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-3].integer_value >> ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 25: // add_expression -> multiply_expression
#line 126 "ExpressionParser.gppg"
                        {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 26: // add_expression -> add_expression, ADD, multiply_expression
#line 129 "ExpressionParser.gppg"
                                           {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-3].integer_value + ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 27: // add_expression -> add_expression, SUBTRACT, multiply_expression
#line 132 "ExpressionParser.gppg"
                                                {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-3].integer_value - ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 28: // multiply_expression -> prefix_expression
#line 137 "ExpressionParser.gppg"
                      {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 29: // multiply_expression -> multiply_expression, MULTIPLY, prefix_expression
#line 140 "ExpressionParser.gppg"
                                                   {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-3].integer_value * ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 30: // multiply_expression -> multiply_expression, DIVIDE, prefix_expression
#line 143 "ExpressionParser.gppg"
                                                 {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-3].integer_value / ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 31: // multiply_expression -> multiply_expression, MODULO, prefix_expression
#line 146 "ExpressionParser.gppg"
                                                 {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-3].integer_value % ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 32: // prefix_expression -> primary_expression
#line 151 "ExpressionParser.gppg"
                       {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value; 
    }
#line default
        break;
      case 33: // prefix_expression -> ADD, prefix_expression
#line 154 "ExpressionParser.gppg"
                          {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 34: // prefix_expression -> SUBTRACT, prefix_expression
#line 157 "ExpressionParser.gppg"
                               {
      CurrentSemanticValue.integer_value = -ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 35: // prefix_expression -> BITWISE_NOT, prefix_expression
#line 160 "ExpressionParser.gppg"
                                  {
      CurrentSemanticValue.integer_value = ~ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 36: // prefix_expression -> LOGICAL_NOT, prefix_expression
#line 163 "ExpressionParser.gppg"
                                  {
      CurrentSemanticValue.integer_value = (ValueStack[ValueStack.Depth-1].integer_value == 0) ? 1 : 0;
    }
#line default
        break;
      case 37: // primary_expression -> NAME
#line 168 "ExpressionParser.gppg"
         {
      CurrentSemanticValue.integer_value = 1;
    }
#line default
        break;
      case 38: // primary_expression -> VALUE
#line 171 "ExpressionParser.gppg"
          {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-1].integer_value;
    }
#line default
        break;
      case 39: // primary_expression -> DEFINED, NAME
#line 174 "ExpressionParser.gppg"
                 {
      CurrentSemanticValue.integer_value = m_macroMap.ContainsKey(ValueStack[ValueStack.Depth-1].name) ? 1 : 0;
    }
#line default
        break;
      case 40: // primary_expression -> DEFINED, LEFT_PARENTHESIS, NAME, RIGHT_PARENTHESIS
#line 177 "ExpressionParser.gppg"
                                                    {
      CurrentSemanticValue.integer_value = m_macroMap.ContainsKey(ValueStack[ValueStack.Depth-2].name) ? 1 : 0;
    }
#line default
        break;
      case 41: // primary_expression -> LEFT_PARENTHESIS, expression, RIGHT_PARENTHESIS
#line 180 "ExpressionParser.gppg"
                                                  {
      CurrentSemanticValue.integer_value = ValueStack[ValueStack.Depth-2].integer_value; 
    }
#line default
        break;
    }
#pragma warning restore 162, 1522
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliases != null && aliases.ContainsKey(terminal))
        return aliases[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

}
}
