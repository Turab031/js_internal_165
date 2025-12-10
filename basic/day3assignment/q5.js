let arr = [10, 40, 25, 80, 15];

let max = arr[0]; 

for (let i = 1; i < arr.length; i++) {
    if (arr[i] > max) {
        max = arr[i];
    }
}

console.log(`Maximum number: ${max}`);