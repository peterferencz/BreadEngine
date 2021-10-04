# 🍞 BreadEngine 🍞
A terminal-ui engine

! This project is a work in progress !

###  Goal:
Generate the barebone ui-s of a tui application from a layout file as such.
building this would give you a working console application with the navigation
already in place, buttons and nput fields already working

### A layout file
```
:LAYOUT
0 0 0 0 2
0 0 0 0 2
1 1 1 1 2

:NAV
0 1 2 3 4

:0
TEXT(Some text inside of a textcomponent)
BUTTON(You can click me)

:1
TEXT(Spacers are cool)
SPACER
TEXT(They seperate text)
```

Feel free to message me with ideas, or commit a pull request


### Screenshots
![Screenshot1](https://github.com/Chicken112/BreadEngine/blob/main/images/breadengine.png)