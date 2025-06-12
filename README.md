# Misc services

## filehasher

Утилита рассчета хеша содержимого файла на основе выбранного алгоритма.

### Использование:

Ключи:
* -f - путь к файлу.
* -a - алгоритм подсчета хеша. По умолчанию *MD5|SHA1*.

Пример:
```shell
#Input:
filehasher -f /path/to/file -a MD5 -a SHA256
#Output:
MD5: F5B3E8AE5F7A95E460D9672E67C8A3FB
SHA256: 3DF3FCC567A96C250C8028DD6385DAE53A5C7D7E7DA30A093E44931E565F8153
```

Если указан 1 алгоритм, то вывод в *stdout* будет без указания названия алгоритма.

Пример:

```shell
#Input:
filehasher -f /path/to/file -a MD5
#Output:
B60683E0DB9F5D521212DD41360E8AF8
```

## hashcomparer

Утилита проверки хешей файлов.

Ключи:
* -i - путь до файла, хеш которого нужно посчитать.
* -a - алгоритм подсчета хеша.
* -h - путь до файла с хешем. Опционально. Если не указано, то считывает из *stdin*.

Пример:

```shell
#Input 1:
cat /path/to/hash.md5 | hashcomparer -i /path/to/file -a MD5
#Output 1:
Hashes are equal!
File: B60683E0DB9F5D521212DD41360E8AF8
Hash: B60683E0DB9F5D521212DD41360E8AF8
#Input 2:
hashcomparer -i filehasher -a MD5 -h hash.md5
#Output 2:
Hashes are not equal!
File: B60683E0DB9F5D521212DD41360E8AF8
Hash: A12383E0DB9F5D521212DD41360E8AF8
```

Опционально, если не указывать ключ *-a*, можно передать файл формата:
```
MD5: 9D2518B153C8C7716B13E0BA437574D7
SHA1: 45C1B80ED826322C8EE937C47B85CBD3219660A3
```
Для сравнения разных хешей.

Пример:
```shell
#Input:
cat path/to/hash/file | hashcomparer -i path/to/file
#Output:
MD5 Hashes are equal!
File: 9D2518B153C8C7716B13E0BA437574D7
Hash: 9D2518B153C8C7716B13E0BA437574D7
SHA1 Hashes are equal!
File: 45C1B80ED826322C8EE937C47B85CBD3219660A3
Hash: 45C1B80ED826322C8EE937C47B85CBD3219660A3
```
Или аналогично с указанием ключа *-h*.

## passgen

Утилита генерации случайных паролей.

### Использование:

Ключи:
* -l - длина генерируемого пароля (*0 < length <= 255*). По умолчанию 8.
* -s - использование спецсимволов. По умолчанию *true*.

Пример:
```shell
#Input:
passgen -l 12 -s true
#Output:
NCUS*dHhh9-u
```

# Сборка

В репозитории представлены *bash* скрипты для сборки *deb* пакетов. Пакеты устанавливаются по пути */usr/bin*
