# Руководство по развёртыванию приложения Yandex weather API


## Требования для развёртывания и запуска программы

* ОС == Windows. Другие ОС не поддерживаются

## Входные данные
Для запуска приложения необходимо заполнить конфиг файл, который содержит следующие поля:
```
db — название БД, к которой будет подключаться приложение
db_user — имя пользователя БД
db_password — пароль пользователя БД
api_key — ключ API
latitude — широта
longitude — долгота
delay — задержка обновления информации
```
Таблица в БД создаётся самим приложением, если её не было создано ранее
## Выходные данные
Результат работы приложения сохраняется в БД и также демонстриурется в консоли

## Шаги настройки и запуска приложения

А) **Запуск через исполняемый файл .exe**

1. **Подготовка файла конфигурации** 
> Необходимо заполнить файл конфигурации TestTask.dll.config перед запуском программы, который находится по следующему пути:
```
TestTask\bin\Release\net8.0\
```
2. **Запуск приложения**
> Для запуска программы необходимо запустить исполняемый файл, который находится по тому же пути
```
TestTask.exe
```

Б) **Запуск через Visual Studio**

1. **Подготовка файла конфигурации** 
> Необходимо заполнить файл конфигурации TestTask.config перед запуском программы, который находится по следующему пути:
```
TestTask\
```
2. **Запуск приложения**
> Для запуска программы необходимо запустить исполняемый файл, который находится папкой выше
```
TestTask.sln
```
