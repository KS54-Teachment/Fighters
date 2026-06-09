import random
import re

LOWERCASE = 'abcdefghijklmnopqrstuvwxyz'
UPPERCASE = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
DIGITS = '0123456789'
SPECIAL = '!@#$%^&*()_+-=[]{}|;:,.<>?/'

def generate_password(length, use_special=True):
    if length < 4:
        raise ValueError("Длина пароля должна быть не менее 4 символов для гарантии использования всех типов.")
    password_chars = [
        random.choice(LOWERCASE),
        random.choice(UPPERCASE),
        random.choice(DIGITS)
    ]
    if use_special:
        password_chars.append(random.choice(SPECIAL))
    all_chars = LOWERCASE + UPPERCASE + DIGITS + (SPECIAL if use_special else '')
    remaining_length = length - len(password_chars)
    password_chars += [random.choice(all_chars) for _ in range(remaining_length)]
    random.shuffle(password_chars)
    return ''.join(password_chars)

def check_password_strength(password):
    if len(password) < 8:
        return False
    has_lowercase = bool(re.search(r'[a-z]', password))
    has_uppercase = bool(re.search(r'[A-Z]', password))
    has_digit = bool(re.search(r'\d', password))
    has_special = bool(re.search(r'[!@#$%^&*()_+=\-$$$${}|;:,.<>?/]', password))
    return has_lowercase and has_uppercase and has_digit and has_special

def main():
    try:
        length = int(input("Введите желаемую длину пароля (рекомендуется от 12): "))
        use_special_input = input("Использовать специальные символы? (да/нет): ").strip().lower()
        use_special = use_special_input in ['да', 'yes', 'y']
        password = generate_password(length, use_special)
        print(f"\nСгенерированный пароль: {password}")
        if check_password_strength(password):
            print("Оценка надежности: НАДЕЖНЫЙ")
        else:
            print("Оценка надежности: НЕНАДЕЖНЫЙ")
            print("Совет: Для повышения надежности используйте пароль длиной от 8 символов, включающий строчные и заглавные буквы, цифры и специальные символы.")
    except ValueError as e:
        print(f"Ошибка ввода: {e}")

if __name__ == "__main__":
    main()
