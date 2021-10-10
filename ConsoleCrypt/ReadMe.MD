# Console crypt
## Quick start

In AppSetting.json, in dir_crypt_files, specefy the path to the encrypted file,
in dir_decrypt_files, spacefy the path to the decrypted file. If you  are use Linux, find path useing the pwd command.
Set name in default_crypr_file and default_decrypr_file from entity with paths. 

Start program in loog mode
```sh
ConsoleCrypt loop
```

For get out of the loop mode use "quit" or "q"

Inicialize the starting encrypted file
```sh
cc> init --encryptedfile -p password
```
Now you can see what is in the file. 
``` sh
cc> show --allgroups
cc> show --allblocks
```
Structure file:
- File    
    - Groups
       - Blocs

To search, use search command and key word
```sh
cc> search linked
```

## Features

- Encrypted data storage
- Search in encrypted data storage
- etc.

##Command example
### Create
Create block
```sh
cc> create -b
Entered group name (full or partial)
socia
------start group------
Name: Social wide
Description: description
------end group------

Is this the right group?  [y/n]
y
Fill Title and press "Enter"
Reddit
Fill Description and press "Enter"

Fill Email and press "Enter"
example@gmail.com
Fill UserName and press "Enter"
Ratte222
Fill Password and press "Enter"
12345678
Fill Phone and press "Enter"

Fill AdditionalInfo When you end - write "end". If you want to undo the input - write "cancel"
end
Add a block to group? [y/n]
y
Block "Reddit" created successfully
```
Create group
```sh
cc> create -g
Fill Name and press "Enter"
ExampleGroup
Fill Description and press "Enter"

Add a group? [y/n]
y
Group "ExampleGroup" created successfully
```
###Update
To update a block, you need to know the block name and the group name.
Use command "search" with option "--showgroup"
```sh
cc> update -b redd -g soc

------start group------
Name: Social wide
Description: description
------end group------

Is this the right group?  [y/n]
y
Name: Social wide
Description: description

Title: Reddit
Description:
UserName: Ratte222
Email: example@gmail.com
Password: 12345678
Phone:
AdditionalInfo:
Update field Title? [y/n]
n
Update field Description? [y/n]
y
Fill Description and press "Enter"
Entertainment, news service
Update field Email? [y/n]
n
Update field UserName? [y/n]
n
Update field Password? [y/n]
n
Update field Phone? [y/n]
n
Update field AdditionalInfo? [y/n]
n
Save this block?  [y/n]
y
Block "Reddit" updated successfully
```

###Search
```sh
cc> search redd --showgroup
------start group------
Name: Social wide
Description: description
------end group------

------start block------
Title: Reddit
Description:
UserName: Ratte222
Email: example@gmail.com
Password: 12345678
Phone:
AdditionalInfo:
------end block------
```

## Some undocumented features
Encrypted\decrypted file use asymmetrical algorithm
Work only windows 10!!!
```sh
encdec_asm --createKey --key 12345678

encdec_asm --key 12345678 --encrypt --pathFrom E:\Temp\RO-CA15-79113496.pdf --pathTo E:\Temp\EncryptedFiles\RO-CA15-79113496.pdf
encdec_asm --encrypt --pathFrom E:\Temp\RO-CA15-79113496.pdf --pathTo E:\Temp\EncryptedFiles\RO-CA15-79113496.pdf

encdec_asm --key 12345678 --decrypt --pathFrom E:\Temp\EncryptedFiles\RO-CA15-79113496.pdf.enc --pathTo E:\Temp\DecryptedFiles\RO-CA15-79113496.pdf
encdec_asm --decrypt --pathFrom E:\Temp\EncryptedFiles\RO-CA15-79113496.pdf.enc --pathTo E:\Temp\DecryptedFiles\RO-CA15-79113496.pdf

encdec_asm --exportPublicKey --pathTo E:\Temp\EncryptedFiles\publicKey

encdec_asm --importPublicKey --pathFrom E:\Temp\EncryptedFiles\publicKey
```