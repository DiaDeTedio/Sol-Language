# Sol-Language
A scripting language for C# use, has reference operators, and support to C# bindings.
Sol language is based on Lua, and made by a brazillian too.

Example Code:
 // Comment
  function sumFunction(argA, argB)
   return argA + argB;
  end
  var x = 38
  var y = 59
  var z = sumFunction(x, y)
  print(z)
  if z == 38+59 then print("Ok, it is right!!!")
  // Sample reference args:
  function noReturn(argA, argB, storeResult)
   storeResult = argA * argB
  end
  noReturn(x, y, &z)
  // reference variables:
  var z_ptr = &z
  var z_value = $z_ptr
  if z == $z_ptr then if z == z_value then print("Ok, pointers is working.")
  
  Ok, thanks for using my scripting language.
