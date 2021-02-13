# texxt-editor
The dark text editor

texxt editor supports executing commands with **IELang**. See more below.

<hr>

## IELang
texxt editor uses the custom command language **IELang**. In order to access this feature click `F6`, you must close any non-script files you have open first. <br>
if you want to execute any commands, click `F5` <br>
in order to execute, you must either have a .txxt/.exxe file open, or script mode enabled.

## Command File types
texxt editor supports two file types for executing your command scripts. <br>
`.txxt` - the default command file. when opened the file does not execute, it only executes when you click F5
`.exxe` - similar to that of .txxt, however when this file is opened, it will execute all commands saved in the file immediately. it can also be executed using F5

## Commands
all args must be passed inside `''` or they will be ignored by the execution algorithm <br>
you can also pass pre-existing variables as args, using this syntax: `--commandname(':variablename')` <br>
you can have comments too, by appending `//` to the line.
<br>
<br>
`--set-variable()` <br>
creates a new variable or updates an already existing one. <br>
args: <br>
name = name of the variable to create/update <br>
value = value to store 
<br>
<br>
`--get-cmd-args()` <br>
gets the command line args passed when the application starts. <br>
return types: <br>
csv, str <br>
args: <br>
name = the name of the variable to store the results in. if the variable doesn't exist, it will create one.
index (optional) = the index (zero based) of the cmd arg value to retrieve. if not specified, it will return a csv value based off of the args
<br>
<br>
`--print()` <br>
prints text to the user. currently just a messagebox. <br>
args: <br>
value = string to print
<br>
<br>
`--exec()` <br>
executes a process. <br>
args: <br>
path = the path (or command/name) to execute. an example of a path could be "C:\text.txt" or "notepad" <br>
args (optional) = arguments to send with the execution. example - --exec('notepad', 'C:\text.txt')
<br>
<br>
`--clean()` <br>
disposes of **all** variables stored in memory. it is best practice to execute this command once you are finished with all your variables.
<br>
<br>
`--delete()` <br>
disposes of a variable. <br>
args: <br>
name = the name of the variable to delete. it is best practice to execute this command when you are finished with a variable.
<br>
<br>
`--increment()` <br>
increments a variable. the variable must be convertable to a double. <br>
args: <br>
name = name of the variable to increment
<br>
<br>
`--decrement()` <br>
decrements a variable. the variable must be convertable to a double. <br>
args: <br>
name = name of the variable to decrement
<br>
<br>
`--close()` <br>
closes texxt editor. **do not use this command when debugging. the command does not save your work and bypasses the "do you want to save" popup. only use at the end of a script and if your work is saved.** <br>

## Example
this example executes a process using a name, which is stored in a variable
```
// stores "noter" in a variable called "execpath"
--set-variable('execpath', 'noter')

// executes the value stored in "execpath"
--exec(':execpath')

// disposes all the variables
--clean()

// closes the application
--close()
```
