# texxt-editor
The dark text editor

texxt editor supports executing commands with **IELang**. See more below.

<hr>

## Commands
texxt editor uses the custom command language **IELang**. In order to access this feature click F6, you must close any non-script files you have open first. <br>
all args must be passed inside `''` or they will be ignored by the execution algorithm <br>
you can also pass pre-existing variables as args, using this syntax: `--commandname(':variablename')`
<br>
<br>
`--set-variable()` <br>
args: <br>
name = name of the variable to create/update <br>
value = value to store 
<br>
<br>
`--get-cmd-args()` <br>
return types: <br>
csv, str <br>
args: <br>
name = the name of the variable to store the results in. if the variable doesn't exist, it will create one.
index (optional) = the index (zero based) of the cmd arg value to retrieve. if not specified, it will return a csv value based off of the args
