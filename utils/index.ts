export const escapeRegexStr = (str: string) =>
  str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");

export const generateSplitRegex = (delimiters: string[]) =>
  new RegExp(`(${delimiters.map((x) => escapeRegexStr(x)).join("|")})`, "g");

export const splitByChars = (str: string, delimiters: string[]) => {
  const splitted = str.split(generateSplitRegex(delimiters));

  if (splitted[splitted.length - 1] === "") {
    return splitted.slice(0, -1);
  }

  return splitted;
};

export const isNumber = (num: any) => {
  if (typeof num !== "string") return false;
  return !isNaN(+num) && !isNaN(parseFloat(num));
};

export class Stack<T> extends Array<T> {
  get last() {
    return this.at(-1);
  }
}
