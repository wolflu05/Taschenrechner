# Taschenrechner

## 1. Vorwort

### 1.1 Funktionsweise

Ein Taschenrechner welcher die "KHPS" Regel beherscht lässt sich am einfachsten programmieren, indem man die uns bekannte Schreibweise ([infix](#12-infix)) zur Reverse Polish Notation ([RPN](#13-rpn)) [umwandelt](#2-infix-zu-rpn) und dann [evaluiert](#3-rpn-evaluieren).

### 1.2 Infix

Wenn du einen arithmetischen Ausdruck schreibst, verwendest du die so genannte Infix-Notation. Das bedeutet, dass der Operator zwischen den Operanden steht. So wird der Ausdruck `2+3` als "Addiere die Werte 2 und 3" interpretiert.

Wenn du aber komplexere Ausdrücke verwendest, gibt es eine Reihe anerkannter Regeln, die bestimmen, wie die Anweisung ausgewertet wird; die Eselsbrücke KHPS (Klammer vor Hochzahl vor Punkt vor Strich) kann verwendet werden, um sich die Rangfolge der Operatoren zu merken.

Wenn du dir also folgenden Ausdruck vorstellst:

`7 + (8 * 3^2 + 4)`

1. Behandele zunächst den Abschnitt in Klammern: 8 × 3 quadriert + 4
2. 3 hoch 2 (3^2 = 9)
3. Multipliziere 9 mit 8 (8 \* 9 = 72)
4. Addiere 4 (72 + 4 = 76)
5. Addiere 7 (76 + 7 = 83)

### 1.3 RPN

Bei der Postfix-Notation oder Reverse Polish Notation (RPN) folgt der Operator seinen Operanden.

Betrachte den infix Ausdruck: `3 + 2`. In der RPN wird dieser `3 2 +` sein.

Betrachten wir das komplexere Beispiel von oben:

`7 + (8 * 3^2 + 4)` -> `7 8 3 2 ^ * 4 + +`

Beachte, dass es keine Klammern gibt. Sie brauchen auch keine Eselsbrücke, um sich die Rangfolge zu merken. Der Ausdruck wird einfach von links nach rechts ausgewertet.

## 2. Infix zu RPN

_kommt bald_ - siehe [Shunting yard algorithm](https://en.wikipedia.org/wiki/Shunting_yard_algorithm)

## 3. RPN Evaluieren

Um einen Postfix-Ausdruck (RPN) auszuwerten, verwenden wir einen Stapel (Liste). Ein Stapel ist nichts anderes als ein "als letztes rein, als erstes raus". Dies kann in python am einfachsten als Liste dagestellt werden und mit `.append()`/`.pop()` modifiziert werden.

Der Algorithmus funktioniert folgendermaßen (Das RPN wird von links nach rechts evaluiert):

- Wenn du auf einen Operand stößt, füge ihn dem stack hinzu
- Wenn du auf enen Operator stößt, entferne die letzten beiden werte vom stack, evaluiere den operator mit den werten und füge das ergebniss wieder in den stack
- Wenn du fertig bist, entferne die finale Antwort vom Stack und gebe sie zurück.
