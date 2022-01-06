# 4n2h0ny.Steam
Steam comment bot developed by 4n2h0ny

## Create the following directories and files
### START.BAT

- Add "*install location*\Google\Chrome Dev\Application" to system variable: "Path"
- Create a startup batch file
```BAT
cd C:\Program Files\Google\Chrome Dev\Application
chrome.exe -remote-debugging-port=xxxx --user-data-dir="xxxx\4n2h0ny.Steam\chromeProfile"
```
### chromeProfile directory
- Create an empty folder with the following path: xxxx\4n2h0ny.Steam\4n2h0ny.Steam.GUI\chromeProfile

### Drivers
- Create a Drivers folder with the following path: xxxx\4n2h0ny.Steam\4n2h0ny.Steam.GUI\bin\Debug\net6.0-windows\Drivers
- Put the chrome driver in the Drivers folder
