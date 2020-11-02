# WPF Diary

Hello everyone!

This is a WPF Diary app that uses a local encrypted scalable database.

### App interface description:

+ Login page.
1. Main page - Add new diary entry.
2. Search page - Allows to search all entries by keywords.
3. More option page - Option to remove an entry by ID (can be found throught search) and remove currently logged user.
4. Info page - Display information about me, the stable release date and the icon author.

### Encryption information and assembly

+ The app will use the .Net framework version 4.7.2 and primarily target 64bit machines.
+ The entire app will be encrypted using custom AES-256 bit encryption.
+ The user database is encrypted with a simple coded key however all user passwords are hashed with Rfc2898DeriveBytes class.
+ Every user will have a seperate file for storing his entries and the file will be an encrypted binary file encrypted using his password as the key.
+ This will make the encryption different for every user and make it impossible to decrypt without the user's password.
+ After login only that user's entries are loaded into memory, and the program can't decrypt any file without the password.
* More on previous point: Only after login in, the program received the Diary decryption key, and the key fits only that user.

### Possible future changes that I might add

* Exporting user diary to xml (Backend already done).

### Notes

If anyone would like to help it is more than welcome, if you have suggestions I would love to hear them.