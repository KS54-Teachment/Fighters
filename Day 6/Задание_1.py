from datetime import datetime

# Запрашиваем имя пользователя
name = input("Введите ваше имя: ")

# Получаем текущую дату
current_date = datetime.now().strftime("%d.%m.%Y")

# Выводим информацию
print("Добро пожаловать в Центр цифровой поддержки")
print(f"Здравствуйте, {name}!")
print(f"Сегодня: {current_date}")
