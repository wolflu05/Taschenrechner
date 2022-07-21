import readline from "node:readline";
import chalk from "chalk";

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout,
});
const question = (text: string) => new Promise((res) => rl.question(text, res));

const run = async () => {
  const a = parseInt((await question("Gebe eine Zahl ein: ")) as string, 10);
  const operator = await question("Gebe einen Operator ein (+,-,*,/): ");
  const b = parseInt(
    (await question("Gebe noch eine Zahl ein: ")) as string,
    10
  );

  let res;

  switch (operator) {
    case "+":
      res = a + b;
      break;
    case "-":
      res = a - b;
      break;
    case "*":
      res = a * b;
      break;
    case "/":
      res = a / b;
      break;
    default:
      console.log(`Der Operator ${operator} ist nicht bekannt`);
  }

  console.log(chalk`Das Ergebniss: {green ${a} ${operator} ${b} = ${res}}`);

  rl.close();
};

run();
