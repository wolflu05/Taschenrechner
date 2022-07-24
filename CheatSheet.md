# Python Cheat Sheet

## 1. Daten Typen

### 1.1 Standard

| Typ     | Beschreibung                     | Beispiel               |
| ------- | -------------------------------- | ---------------------- |
| `int`   | Ganzzahlen                       | `my_int = 314`         |
| `float` | Kommazahlen (Trennzeichen Punkt) | `my_float = 3.14`      |
| `str`   | Zeichenkette (Strings)           | `my_string = "ABCDEF"` |
| `bool`  | Wahrheitswert (`True`/`False`)   | `my_bool = False`      |

### 1.2 Erweitert

#### 1.2.1 list

Eine Liste kann eine unbeschränkte Anzahl an Elementen beinhalten, welche auch im nachhinein noch modifiziert werden können.

```py
my_list = [1,2,3,4,5]

print(my_list[2]) # 3
my_list[2] = "Hello World"
print(my_list[2]) # "Hello World"

print(len(my_list)) # 5

my_list.pop()
print(my_list) # [1,2,"Hello World",4]

my_list.append(10)
print(my_list) # [1,2,"Hello World",4,10]
```

##### Funktionen

| Name           | Beschreibung                                                                 | Beispiel             |
| -------------- | ---------------------------------------------------------------------------- | -------------------- |
| `.append(123)` | Fügt ein Element am Ende der Liste an                                        | `my_list.append(10)` |
| `.pop(idx)`    | Entfernt ein Element der List am idx (wenn nicht angegeben, letztes Element) | `my_list.pop()`      |

## 2. Operatoren

### 2.1 Arithmetische Operatoren

| Operator | Beschreibung                                   | Beispiel         |
| -------- | ---------------------------------------------- | ---------------- |
| `+`      | Addition (Strings/Zahlen)                      | `1 + 2 # -> 3`   |
| `-`      | Subtraktion                                    | `1 - 2 # -> -1`  |
| `*`      | Multiplikation                                 | `2 * 3 # -> 6`   |
| `/`      | Division                                       | `1 / 2 # -> 0.5` |
| `//`     | Ganzzahlige Division (schneided nach Komma ab) | `2 // 3 # -> 0`  |
| `**`     | Potenzieren                                    | `2 ** 3 # -> 8`  |

### 2.2 Vergleichs Operatoren

| Operator | Beschreibung        | Beispiel            |
| -------- | ------------------- | ------------------- |
| `==`     | Gleich              | `1 == 2 # -> False` |
| `<`      | kleiner als         | `1 < 2 # -> True`   |
| `>`      | größer als          | `2 > 3 # -> False`  |
| `<=`     | kleiner oder gleich | `1 <= 1 # -> True`  |
| `>=`     | größer oder gleich  | `2 >= 3 # -> False` |
| `!=`     | nicht gleich        | `1 != 2 # -> True`  |

### 2.2 Logische Operatoren

| Operator | Beschreibung  | Beispiel                      |
| -------- | ------------- | ----------------------------- |
| `and`    | Logisch und   | `False and True # -> False`   |
| `or`     | Logisch oder  | `False or True # -> True`     |
| `is`     | Logisch ist   | `True is True # -> True`      |
| `not`    | Logisch nicht | `True is not False # -> True` |

## 3 Kontrolstrukturen

### 3.1 if/elif/else

Führt den code mit der als erstes zutreffenden Bedingung aus. Ist keine der Bedingungen wahr, so wird der `else` Teil ausgeführt. Es können so viel `elif` Bedingungen wie gewollt verwendet werden.

```py
alter = 28
if alter >= 18:
  print("Du darfst Auto fahren")
elif alter >= 17:
  print("Du darfst mit Begleitung Auto fahren")
elif alter >= 16.5:
  print("Du kannst mit dem Führerschein bald anfangen")
else:
  print("Du darfst noch kein Auto fahren")
```

### 3.2 while

Wiederhohlt so lange die Bedingung Wahr ist den nachfolenden code.

```py
x = 0
while x <= 10:
  print(x)
  x += 1
```

## 4 Funktionen

### 4.1 Eingebaute Funktionen

| Name      | Beschreibung                                                                              | Beispiel                                 |
| --------- | ----------------------------------------------------------------------------------------- | ---------------------------------------- |
| `print()` | Gibt die übergebenen Parameter in der Konsole aus                                         | `print("Hello World")`                   |
| `input()` | Fordert den Benutzer zur Eingabe in der Konsole auf. Gibt das Ergebniss als String zurück | `rueckgabe = input("Wie geht es dir? ")` |
| `type()`  | Gibt den Daten Typ des übergebenen Parameters in der Klammer zurück                       | `type("Hello") # -> str`                 |
| `int()`   | Versucht den Datentyp in eine Ganzzahl umzuwandeln                                        | `int("123") # -> 123`                    |
| `float()` | Versucht den Datentyp in eine Kommazahl umzuwandeln                                       | `str("1.23") # -> 1.23`                  |
| `str()`   | Wandelt den Datentyp in einen String um                                                   | `str(123) # -> "123"`                    |
| `len()`   | Gibt die Länge der Zeichenkette/Liste welche als Parameter übergeben wurde zurück         | `len("Hello") # -> 5`                    |

### 4.2 Eigene Funktionen

Mithilfe einer Funktion kann man code zusammenfassen und diesen öfters aufrufen. Funktionen können parameter und rückgabewerte haben.

```py
def meine_funktion(parameter1, parameter2):
  print(parameter1)
  return parameter2

ergebniss = mein_funktion("Hello", "World") # Gibt "Hello" aus
print(ergebniss) # Gibt "World" aus
```
