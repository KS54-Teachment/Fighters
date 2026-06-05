// БАЗА ЗНАНИЙ (массив статей)
const knowledgeArticles = [
    { 
        title: "Как получить электронную подпись?", 
        content: "Для получения электронной подписи можно обратиться в аккредитованный удостоверяющий центр (например, в центры при МФЦ). Через портал «Госуслуги» подайте заявление на сертификат. Также возможна упрощённая подпись в мобильном приложении «Госключ». Наш центр помогает с выбором и установкой." 
    },
    { 
        title: "Запись к врачу через Госуслуги", 
        content: "Авторизуйтесь на портале Госуслуги → раздел «Здоровье» → выберите поликлинику и врача → выберите свободное время → подтвердите запись. При необходимости мы поможем загрузить полис и прикрепиться к поликлинике." 
    },
    { 
        title: "Восстановление доступа к Госуслугам", 
        content: "Если забыли пароль, на странице входа нажмите «Восстановить пароль». Введите СНИЛС или номер телефона, затем следуйте инструкции (доступ через подтверждённый номер). При утере доступа к номеру обратитесь в центр обслуживания — мы предоставим алгоритм." 
    },
    { 
        title: "Регистрация на портале Работа России", 
        content: "Перейдите на trudvsem.ru, нажмите «Войти через Госуслуги». Заполните профиль, загрузите резюме. Центр поддержки поможет пройти верификацию и настроить уведомления о вакансиях." 
    },
    { 
        title: "Как оплатить налоги онлайн?", 
        content: "Личный кабинет налогоплательщика (lkfl.nalog.ru) или через приложение «Налоги ФЛ». Оплата возможна по QR-коду, картой или через СБП. Также можно настроить автоплатеж. Если возникают ошибки, напишите нам — предоставим пошаговые скриншоты." 
    },
    { 
        title: "Настройка двухфакторной аутентификации", 
        content: "Для большей безопасности аккаунта на Госуслугах перейдите в «Безопасность» → «Двухфакторная аутентификация», выберите приложение-аутентификатор (Google Authenticator, Яндекс Ключ) или SMS. Сохраните резервные коды. Мы готовы провести короткий вебинар по настройке." 
    },
    { 
        title: "Оформление пособий через соцзащиту онлайн", 
        content: "Через портал Госуслуги в разделе «Пособия» можно подать заявление на детские выплаты, единое пособие. Система автоматически проверит данные. При возникновении ошибок в справках обращайтесь в поддержку." 
    },
    { 
        title: "Как пользоваться виртуальной инфраструктурой центра?", 
        content: "Наш центр подготовил облачный стенд для обучения: демо-доступ к сервисам. Запросите логин через форму контактов — специалист выдаст временный доступ к симулятору госуслуг." 
    }
];

// Рендер карточек базы знаний
function renderKnowledgeBase(filterText = "") {
    const grid = document.getElementById("knowledgeGrid");
    if (!grid) return;
    
    const filtered = knowledgeArticles.filter(article => 
        article.title.toLowerCase().includes(filterText.toLowerCase()) || 
        article.content.toLowerCase().includes(filterText.toLowerCase())
    );
    
    if (filtered.length === 0) {
        grid.innerHTML = `<div style="grid-column:1/-1; text-align:center; padding:2rem;">📭 Ничего не найдено. Попробуйте другой запрос.</div>`;
        return;
    }
    
    grid.innerHTML = filtered.map((article, idx) => `
        <div class="card knowledge-card" data-article-index="${idx}">
            <i class="fas fa-book-open"></i>
            <h3 style="margin: 0.5rem 0 0.75rem;">${escapeHtml(article.title)}</h3>
            <p style="color: #475569; font-size: 0.9rem;">${escapeHtml(article.content.substring(0, 85))}...</p>
            <span style="color: var(--primary); font-size: 0.8rem; display: inline-block; margin-top: 0.75rem;">Подробнее →</span>
        </div>
    `).join("");

    // Добавляем слушатели на новые карточки
    document.querySelectorAll(".knowledge-card").forEach(card => {
        card.addEventListener("click", (e) => {
            const idxAttr = card.getAttribute("data-article-index");
            if (idxAttr !== null) { 
              const filteredArr = knowledgeArticles.filter(article => 
                    article.title.toLowerCase().includes(filterText.toLowerCase()) ||
                    article.content.toLowerCase().includes(filterText.toLowerCase())
                );
                const realIdx = parseInt(idxAttr, 10);
                if (filteredArr[realIdx]) {
                    openModal(filteredArr[realIdx].title, filteredArr[realIdx].content);
                }
            }
        });
    });
}

function escapeHtml(str) {
    return str.replace(/[&<>]/g, function(m) {
        if(m === '&') return '&amp;';
        if(m === '<') return '&lt;';
        if(m === '>') return '&gt;';
        return m;
    });
}

// Модальное окно
const modal = document.getElementById("knowledgeModal");
const modalTitle = document.getElementById("modalTitle");
const modalBody = document.getElementById("modalBody");

function openModal(title, content) {
    modalTitle.textContent = title;
    modalBody.innerHTML = content.replace(/\n/g, '<br>');
    modal.style.display = "flex";
}

function closeModal() {
    modal.style.display = "none";
}

// Поиск
const searchInput = document.getElementById("knowledgeSearch");
if(searchInput) {
    searchInput.addEventListener("input", (e) => {
        renderKnowledgeBase(e.target.value);
    });
}

// Мобильное меню
const menuBtn = document.getElementById("menuBtn");
const navLinks = document.getElementById("navLinks");

if(menuBtn && navLinks) {
    menuBtn.addEventListener("click", () => {
        navLinks.classList.toggle("show");
    });
    
    navLinks.querySelectorAll("a").forEach(link => {
        link.addEventListener("click", () => {
            navLinks.classList.remove("show");
        });
    });
}

// Кнопка "Наверх"
const backBtn = document.getElementById("backToTop");

window.addEventListener("scroll", () => {
    if(window.scrollY > 400) {
        backBtn.classList.add("show");
    } else {
        backBtn.classList.remove("show");
    }
});

if(backBtn) {
    backBtn.addEventListener("click", () => {
        window.scrollTo({ top: 0, behavior: "smooth" });
    });
}

// Форма обратной связи
const form = document.getElementById("callbackForm");
if(form) {
    form.addEventListener("submit", (e) => {
        e.preventDefault();
        const name = document.getElementById("nameInput")?.value.trim() || "";
        const email = document.getElementById("emailInput")?.value.trim() || "";
        const msg = document.getElementById("messageInput")?.value.trim() || "";
        
        if(name && email && msg) {
            alert(`✅ Спасибо, ${name}! Ваше сообщение принято. Наши специалисты ответят на ${email} в ближайшее время.`);
            form.reset();
        } else {
            alert("❌ Пожалуйста, заполните все поля формы.");
        }
    });
}

// Плавная прокрутка для якорей
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function(e) {
        const href = this.getAttribute('href');
        if(href === "#" || href === "") return;
        const target = document.querySelector(href);
        if(target) {
            e.preventDefault();
            target.scrollIntoView({ behavior: "smooth" });
        }
    });
});

// Закрытие модального окна
document.getElementById("closeModalBtn")?.addEventListener("click", closeModal);
window.addEventListener("click", (e) => { 
    if(e.target === modal) closeModal(); 
});

// Инициализация базы знаний
renderKnowledgeBase();
```
