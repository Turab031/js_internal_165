
let user = {name: "Aman", age: 21, course: "JS"};


let userJSON = JSON.stringify(user);
console.log("JSON String:", userJSON);
console.log("Type:", typeof userJSON);


let userObject = JSON.parse(userJSON);
console.log("\nParsed Object:", userObject);
console.log("Type:", typeof userObject);
console.log("Name:", userObject.name);
console.log("Age:", userObject.age);
console.log("Course:", userObject.course);