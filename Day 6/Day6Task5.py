from datetime import datetime

def create_report(team_name, requests_list):
    """
    Создаёт отчёт и сохраняет его в файл report.txt.

    Параметры:
    team_name (str): название команды
    requests_list (list): список обращений
    """
    
    current_date = datetime.now().strftime("%d.%m.%Y %H:%M:%S")

    
    processed_count = len(requests_list)

    
    report_content = f"""Название команды: {team_name}
Дата: {current_date}
Список обращений:
"""

    
    for i, request in enumerate(requests_list, 1):
        report_content += f"  {i}. {request}\n"

    report_content += f"Количество обработанных заявок: {processed_count}"

    
    with open("report.txt", "w", encoding="utf-8") as file:
        file.write(report_content)

    print("Отчёт успешно сохранён в файл 'report.txt'")


if __name__ == "__main__":
    
    team_name = input("Введите название команды: ")

    print("Введите список обращений (для завершения ввода оставьте строку пустой):")
    requests_list = []
    while True:
        request = input(f"Обращение {len(requests_list) + 1}: ")
        if request == "":
            break
        requests_list.append(request)

    
    create_report(team_name, requests_list)
