function someFunction(someArgument0, otherArgument)
 return someArgument0 + otherArgument
end
var functionResult = someFunction(3, 5)
functionPointer = someFunction
functionPointer(7, 8)
var x = 0
var x_ref = &x
x_ref = 1
if x == 1 then
 print(x)
 print($x_ref)
end
