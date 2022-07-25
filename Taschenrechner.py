import re
import math
from collections import namedtuple

Operator = namedtuple("Operator", "precedence arg func")
Function = namedtuple("Function", "arg func")

OPERATORS = {
    "+": Operator(2, 2, float.__add__),
    "-": Operator(2, 2, float.__sub__),
    "*": Operator(3, 2, float.__mul__),
    "/": Operator(3, 2, float.__truediv__),
    "^": Operator(4, 2, float.__pow__),
    "!": Operator(4, 1, lambda x: math.gamma(x + 1))
}

CONSTANTS = {
    "pi": math.pi,
    "e": math.e,
    "tau": math.tau,
    "inf": math.inf,
    "c": 300_000_000
}

FUNCTIONS = {
    "sin": Function(1, math.sin),
    "asin": Function(1, math.asin),
    "cos": Function(1, math.cos),
    "acos": Function(1, math.acos),
    "tan": Function(1, math.tan),
    "atan": Function(1, math.atan),
    "ceil": Function(1, math.ceil),
    "floor": Function(1, math.floor),
    "sqrt": Function(1, math.sqrt),
    "gcd": Function(2, lambda a, b: math.gcd(int(a), int(b))),
    "max": Function(2, max),
    "min": Function(2, min),
    "abs": Function(1, abs),
}


def is_float(num):
  try:
    float(num)
    return True
  except ValueError:
    return False


def split_by_list(string, chars):
  r = f"({'|'.join(map(re.escape, chars))})"
  return re.split(r, string)


def infix_to_rpn(inp):
  output_queue = []
  operator_stack = []

  for token in split_by_list(inp, [*OPERATORS.keys(), "(", ")", ",", " "]):
    if is_float(token):
      output_queue.append(float(token))
    elif token in CONSTANTS:
      output_queue.append(CONSTANTS[token])
    elif token == "(":
      operator_stack.append("(")
    elif token == ")":
      while operator_stack[-1] != "(":
        output_queue.append(operator_stack.pop())

      if operator_stack[-1] != "(":
        raise Exception("Fehlende (")

      operator_stack.pop()

      if operator_stack[-1] in FUNCTIONS:
        output_queue.append(operator_stack.pop())
    elif token in FUNCTIONS:
      operator_stack.append(token)
    elif token in OPERATORS:
      while len(operator_stack) and operator_stack[-1] != "(" and OPERATORS[
          operator_stack[-1]].precedence >= OPERATORS[token].precedence:
        output_queue.append(operator_stack.pop())

      operator_stack.append(token)

  while len(operator_stack):
    if operator_stack[-1] == "(":
      raise Exception("Fehlende )")

    output_queue.append(operator_stack.pop())

  return output_queue


def evaluate_rpn(rpn_list):
  stack = []
  OPERATIONS = {**OPERATORS, **FUNCTIONS}

  for token in rpn_list:
    if token in OPERATIONS:
      operator = OPERATIONS[token]
      args = [stack.pop() for _ in range(operator.arg)]
      stack.append(operator.func(*args[::-1]))
    else:
      stack.append(token)

  return stack.pop()


def solve(exp):
  rpn = infix_to_rpn(exp)
  print(rpn)
  ergebnis = evaluate_rpn(rpn)
  return ergebnis


while (inp := input("> ")) != "quit()":
  try:
    print(solve(inp))
  except Exception as e:
    print("Fehler:", e)
