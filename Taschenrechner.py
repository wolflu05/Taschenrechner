import re

def infix_to_rpn(input):
  OPERATORS = {
    "+": 2,
    "-": 2,
    "*": 3,
    "/": 3,
    "^": 4
  }

  output_queue = []
  operator_stack = []

  for token in re.split("(\+|\-|\*|\/|\^|\(|\)| )", input):
    if token.isdigit():
      output_queue.append(int(token))
    elif token == "(":
      operator_stack.append("(")
    elif token == ")":
      while operator_stack[-1] != "(":
        output_queue.append(operator_stack.pop())

      if operator_stack[-1] != "(":
        raise Exception("Fehlende (")

      operator_stack.pop()
    elif token in OPERATORS:
      while len(operator_stack) and operator_stack[-1] != "(" and OPERATORS[operator_stack[-1]] > OPERATORS[token]:
        output_queue.append(operator_stack.pop())
      
      operator_stack.append(token)
  
  while len(operator_stack):
    if operator_stack[-1] == "(":
      raise Exception("Fehlende )")

    output_queue.append(operator_stack.pop())

  return output_queue


def evaluate_rpn(rpn):
  # Deine Aufgabe :)
  pass

rpn = infix_to_rpn("7+(8 * 3^2 + 4 ) ")
print(rpn)
ergebniss = evaluate_rpn(rpn)
print(ergebniss)
