import { solve } from "./Taschenrechner.bundle.js";

const calculatorE = document.getElementById("calculator");
const calculatorOutE = document.getElementById("calculator-output");
const calculatorInE = document.getElementById("calculator-input");

const state = { output: "" };
var stateProxy = new Proxy(state, {
  set: function (target, key, value) {
    target[key] = value;
    calculatorOutE.innerText = value;
    return true;
  },
});

const buttons = [
  [
    { name: "(", value: "(" },
    { name: ")", value: ")" },
    { name: "^", value: "^" },
    { name: "AC", value: "", run: () => (stateProxy.output = "") },
  ],
  [
    { name: "7", value: "7" },
    { name: "8", value: "8" },
    { name: "9", value: "9" },
    { name: "÷", value: "/" },
  ],
  [
    { name: "4", value: "4" },
    { name: "5", value: "5" },
    { name: "6", value: "6" },
    { name: "×", value: "*" },
  ],
  [
    { name: "1", value: "1" },
    { name: "2", value: "2" },
    { name: "3", value: "3" },
    { name: "-", value: "-" },
  ],
  [
    { name: "0", value: "0" },
    { name: ".", value: "." },
    {
      name: "=",
      value: "",
      run: () => {
        console.log(stateProxy);
        stateProxy.output = solve(stateProxy.output);
      },
      onCreate: (el) => {
        el.classList.add("equal");
      },
    },
    { name: "+", value: "+" },
  ],
];

for (const row of buttons) {
  for (const btn of row) {
    const btnDiv = document.createElement("div");
    btnDiv.innerText = btn.name;
    btnDiv.addEventListener("click", () => {
      stateProxy.output += btn.value;
      btn.run?.();
    });
    btnDiv.classList.add("calculator-input-btn");
    btn.onCreate?.(btnDiv);
    calculatorInE.appendChild(btnDiv);
  }
}
