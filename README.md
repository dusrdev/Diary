# WPF Diary : Version 1.1.1.0

Hello everyone!

This is a WPF Diary app that uses a local encrypted scalable database.

## Developer update: 2021

The latest version of this project was released on Nov 2019 and is using .net framework 4.7.2 in addition to more outdated packages and techniques.
As of right now consider this application only as a general and very basic presentations of my skills; as I've since progressed to using newer and better technologies, and refined both my skills and every self-made package that this app uses.

## App interface description

+ Login page.

1. Main page - Add new diary entry.
2. Search page - Allows to search all entries by keywords, "Explicit" and [Date] *All.
3. More option page - Option to remove an entry by ID (can be found through search) and remove currently logged user.
4. Info page - Display information about me, the stable release date and the icon author.

## Encryption information and assembly

+ The app will use the .Net framework version 4.7.2 and primarily target 64bit machines.
+ The entire app will be encrypted using custom AES-256 bit encryption.
+ The user database is encrypted with a simple coded key however all user passwords are hashed with Rfc2898DeriveBytes class.
+ Every user will have a separate file for storing his entries and the file will be an encrypted binary file encrypted using his password as the key.
+ This will make the encryption different for every user and make it impossible to decrypt without the user's password.
+ After login only that user's entries are loaded into memory, and the program can't decrypt any file without the password.
+ More on previous point: Only after login in, the program received the Diary decryption key, and the key fits only that user.

### Screenshots

+ Login page:
![alt text](https://github.com/dusrdev/Diary/blob/main/Images/LoginPageScreenshot.png?raw=true)
+ Main page:
![alt text](https://github.com/dusrdev/Diary/blob/main/Images/MainPageScreenshot.png?raw=true)
+ Search page:
![alt text](https://github.com/dusrdev/Diary/blob/main/Images/SearchPageScreenshot.png?raw=true)
+ More option page:
![alt text](https://github.com/dusrdev/Diary/blob/main/Images/MorePageScreenshot.png?raw=true)
+ Info page:
![alt text](https://github.com/dusrdev/Diary/blob/main/Images/InfoPageScreenshot.png?raw=true)
