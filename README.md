# 4n2h0ny.Steam
Steam comment bot developed by 4n2h0ny

## 1. SETUP
Create the following directories and files
### 1.1 START.bat

- Add "*install location*\Google\Chrome Dev\Application" to system variable: "Path"
- Create a startup batch file
```BAT
cd C:\Program Files\Google\Chrome Dev\Application
chrome.exe -remote-debugging-port=xxxx --user-data-dir="xxxx\4n2h0ny.Steam\chromeProfile"
```
### 1.2 chromeProfile directory
- Create an empty folder with the following path: 
`xxxx\4n2h0ny.Steam\4n2h0ny.Steam.GUI\chromeProfile`
### 1.3 Drivers
- Create a Drivers folder with the following path: 
`xxxx\4n2h0ny.Steam\4n2h0ny.Steam.GUI\bin\Debug\net6.0-windows\Drivers`
- Put the chrome driver in the Drivers folder

## 2. How to use
**Run the START.bat script (The bot won't run if it can't hook to the port of the running chrome session)**

### 2.1 Input fields
#### MaxPageIndex
Set the amount of comment pages the bot scrapes

#### DefaultComment
**ChromeDriver only supports characters in the BMP**

Some personaNames aren't supported, the defaultComment is applied in those cases.

#### CommentTemplate
This is the main comment that will be commented. A personaName can be placed within the interpolated string, like this:

`Have a great day {0}! :heart:`

### 2.2 Buttons
#### CLEAR
This wipes the text in the DefaultComment and CommentTemplate textBox.

#### TEST
This fills the commentbox without submitting it.

#### START
This starts scraping for profiles and places comments on the gathered profiles.

## 3. Prerequisites
To run this software you need to install the following:
- [.NET Runtime Desktop 6.0.1](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.1-windows-x64-installer].NET)
- [Google Chrome](https://www.google.com/chrome/)
