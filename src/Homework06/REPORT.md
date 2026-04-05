# Homework 06

## Игра

В проекте реализована консольная игра "Угадай число". Программа:

- загружает диапазон чисел и количество попыток из `gameSettings.json`;
- генерирует случайное число в указанном диапазоне;
- подсказывает пользователю "Больше" или "Меньше" после каждой неудачной попытки;
- завершает игру победой при угадывании числа или проигрышем при исчерпании попыток.

## SOLID

### S: Single Responsibility Principle

- `GuessGame` отвечает только за игровой сценарий: запуск раундов, чтение попыток, вывод результата.
- `JsonGameSettingsProvider` отвечает только за чтение настроек из файла.
- `GameSettingsValidator` отвечает только за проверку корректности настроек.
- `RandomNumberGenerator` и `ConsoleInputReader`/`ConsoleOutputWriter` инкапсулируют конкретные инфраструктурные детали.

### O: Open/Closed Principle

- Механизм проверки догадки расширяется через `IGuessRule`.
- Чтобы добавить новый тип подсказки или отдельное правило, достаточно создать ещё одну реализацию `IGuessRule` и передать её в `GuessEvaluator`.
- Код `GuessGame` и `GuessEvaluator` при этом менять не требуется.

### L: Liskov Substitution Principle

- `GuessGame` работает с абстракцией `INumberGenerator`.
- `RandomNumberGenerator` можно заменить на `FixedNumberGenerator`, не меняя игровой сценарий и не нарушая его контракт.
- Это позволяет использовать разные стратегии генерации числа, включая предсказуемые сценарии для демонстрации или тестирования.

### I: Interface Segregation Principle

- Вместо одного "толстого" интерфейса ввод и вывод разделены на `IInputReader` и `IOutputWriter`.
- Генерация числа вынесена в отдельный интерфейс `INumberGenerator`.
- Чтение настроек и их проверка разделены на `IGameSettingsProvider` и `IGameSettingsValidator`.
- Классы зависят только от тех операций, которые им действительно нужны.

### D: Dependency Inversion Principle

- `GuessGame` зависит от абстракций `IInputReader`, `IOutputWriter`, `INumberGenerator`, `IGuessEvaluator`, а не от `Console` и `Random` напрямую.
- `Program` выступает в роли composition root: именно там создаются конкретные реализации и передаются в игру.
- Благодаря этому прикладная логика не привязана к способу ввода, вывода, генерации числа и источнику настроек.

## Ссылка на проект

- Репозиторий: https://github.com/Nikita-Seomin/otus-csharp-professional-labs
- Проект: https://github.com/Nikita-Seomin/otus-csharp-professional-labs/tree/main/src/Homework06

## Время выполнения

Оценка затраченного времени: около 1 часа.
