const DEBUG = true;
export const debug = (...args) => DEBUG && console.log(...args);
export const escapeRegexStr = (str) => str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
export const generateSplitRegex = (delimiters) => new RegExp(`(${delimiters.map((x) => escapeRegexStr(x)).join("|")})`, "g");
export const splitByChars = (str, delimiters) => {
    const splitted = str.split(generateSplitRegex(delimiters));
    if (splitted[splitted.length - 1] === "") {
        return splitted.slice(0, -1);
    }
    return splitted;
};
export const isNumber = (num) => {
    if (typeof num !== "string")
        return false;
    return !isNaN(+num) && !isNaN(parseFloat(num));
};
export class Stack extends Array {
    get last() {
        return this[this.length - 1];
    }
}
export const OPs = [
    { name: "+", precedence: 2, associativity: "left", eval: (a, b) => a + b },
    { name: "-", precedence: 2, associativity: "left", eval: (a, b) => a - b },
    { name: "*", precedence: 3, associativity: "left", eval: (a, b) => a * b },
    { name: "/", precedence: 3, associativity: "left", eval: (a, b) => a / b },
    { name: "^", precedence: 4, associativity: "right", eval: (a, b) => a ** b },
    // special operators
    { name: "(", precedence: Infinity },
    { name: ")", precedence: Infinity },
];
const OPMap = Object.fromEntries(OPs.map((op) => [op.name, op]));
export const generateTokens = (expr) => {
    const tokens = splitByChars(expr, [...Object.keys(OPMap), " "]).filter((token) => !["", " ", "\n", "\r", "\n\r"].includes(token));
    return tokens;
};
export const generateRPN = (tokens) => {
    const outputQueue = [];
    const operatorStack = new Stack();
    for (const token of tokens) {
        if (isNumber(token)) {
            outputQueue.push(+token);
        }
        else if (token === "(") {
            operatorStack.push(OPMap["("]);
        }
        else if (token === ")") {
            while (operatorStack.last && operatorStack.last.name !== "(") {
                outputQueue.push(operatorStack.pop());
            }
            if (operatorStack.last?.name !== "(") {
                throw new Error("Missing (");
            }
            operatorStack.pop();
        }
        else if (token in OPMap) {
            const op = OPMap[token];
            while (operatorStack.last &&
                operatorStack.last.name !== "(" &&
                (operatorStack.last.precedence > op.precedence ||
                    (operatorStack.last.precedence === op.precedence &&
                        operatorStack.last.associativity === "left"))) {
                outputQueue.push(operatorStack.pop());
            }
            operatorStack.push(op);
        }
        debug(outputQueue, operatorStack);
    }
    while (operatorStack.length) {
        if (operatorStack.last?.name === "(") {
            throw new Error("Missing )");
        }
        outputQueue.push(operatorStack.pop());
    }
    return outputQueue;
};
export const evalRPN = (rpn) => {
    const stack = new Stack();
    for (const op of rpn) {
        if (typeof op === "number") {
            stack.push(op);
        }
        else {
            const ev = op.eval;
            if (!ev) {
                throw new Error(`${op.name} has not implemented eval`);
            }
            const a = stack.pop();
            const b = stack.pop();
            if (typeof a !== "number") {
                throw new Error(`${op.name} got ${typeof a} as first argument, expected a number`);
            }
            if (typeof b !== "number") {
                throw new Error(`${op.name} got ${typeof b} as second argument, expected a number`);
            }
            stack.push(ev(b, a));
        }
        debug(stack);
    }
    return stack.pop();
};
export const solve = (expr) => {
    const tokens = generateTokens(expr);
    debug(tokens);
    const rpn = generateRPN(tokens);
    debug(rpn.map((x) => (typeof x === "number" ? x : x.name)));
    const res = evalRPN(rpn);
    return res;
};
