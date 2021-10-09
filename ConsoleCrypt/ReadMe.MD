# Console crypt
## Quic start

In AppSetting.json, in dir_crypt_files, specefy the path to the encrypted file,
in dir_decrypt_files, spacefy the path to the decrypted file. If you  are use Linux, find path useing the pwd command.
Set name in default_crypr_file and default_decrypr_file from entity with paths. 

Start program in loog mode
```sh
ConsoleCrypt loop
```

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

