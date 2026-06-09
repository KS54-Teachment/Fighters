class PasswordGenerator {
    constructor() {
        this.uppercaseChars = 'ABCDEFGHJKLMNPQRSTUVWXYZ';
        this.lowercaseChars = 'abcdefghijkmnopqrstuvwxyz';
        this.numberChars = '23456789';
        this.symbolChars = '!@#$%^&*()_+[]{}<>?';
        
        this.initElements();
        this.attachEvents();
        this.generatePassword();
    }
    
    initElements() {
        this.passwordField = document.getElementById('passwordField');
        this.lengthSlider = document.getElementById('lengthSlider');
        this.lengthValue = document.getElementById('lengthValue');
        this.uppercaseCheck = document.getElementById('uppercase');
        this.lowercaseCheck = document.getElementById('lowercase');
        this.numbersCheck = document.getElementById('numbers');
        this.symbolsCheck = document.getElementById('symbols');
        this.generateBtn = document.getElementById('generateBtn');
        this.copyBtn = document.getElementById('copyBtn');
        this.strengthFill = document.getElementById('strengthFill');
        this.strengthText = document.getElementById('strengthText');
    }
    
    attachEvents() {
        this.lengthSlider.addEventListener('input', () => {
            this.lengthValue.textContent = this.lengthSlider.value;
            this.generatePassword();
        });
        
        const checkboxes = [this.uppercaseCheck, this.lowercaseCheck, this.numbersCheck, this.symbolsCheck];
        checkboxes.forEach(checkbox => {
            checkbox.addEventListener('change', () => this.generatePassword());
        });
        
        this.generateBtn.addEventListener('click', () => this.generatePassword());
        this.copyBtn.addEventListener('click', () => this.copyToClipboard());
    }
    
    getCharacterSet() {
        let chars = '';
        if (this.uppercaseCheck.checked) chars += this.uppercaseChars;
        if (this.lowercaseCheck.checked) chars += this.lowercaseChars;
        if (this.numbersCheck.checked) chars += this.numberChars;
        if (this.symbolsCheck.checked) chars += this.symbolChars;
        return chars;
    }
    
    generatePassword() {
        const length = parseInt(this.lengthSlider.value);
        let charset = this.getCharacterSet();
        
        if (!charset) {
            this.passwordField.value = 'Выберите хотя бы один тип символов';
            this.updateStrength(0);
            return;
        }
        
        let password = '';
        
        for (let i = 0; i < length; i++) {
            const randomIndex = Math.floor(Math.random() * charset.length);
            password += charset[randomIndex];
        }
        
        this.passwordField.value = password;
        this.updateStrength(this.calculateStrength(password));
    }
    
    calculateStrength(password) {
        let score = 0;
        const length = password.length;
        
        if (length >= 8) score += 1;
        if (length >= 12) score += 1;
        if (length >= 16) score += 1;
        
        if (/[A-Z]/.test(password)) score += 1;
        if (/[a-z]/.test(password)) score += 1;
        if (/[0-9]/.test(password)) score += 1;
        if (/[^A-Za-z0-9]/.test(password)) score += 1;
        
        let hasRepeat = /(.)\1{2,}/.test(password);
        if (!hasRepeat && length >= 8) score += 1;
        
        let entropy = length * Math.log2(94);
        if (entropy > 60) score += 1;
        
        return Math.min(score, 10);
    }
    
    updateStrength(score) {
        const percentage = (score / 10) * 100;
        this.strengthFill.style.width = `${percentage}%`;
        
        let color, text;
        if (score <= 3) {
            color = '#f44336';
            text = 'Очень слабый';
        } else if (score <= 5) {
            color = '#ff9800';
            text = 'Слабый';
        } else if (score <= 7) {
            color = '#ffc107';
            text = 'Средний';
        } else if (score <= 9) {
            color = '#4caf50';
            text = 'Хороший';
          } else {
            color = '#2e7d32';
            text = 'Отличный!';
        }
        
        this.strengthFill.style.backgroundColor = color;
        this.strengthText.textContent = text;
        this.strengthText.style.color = color;
    }
    
    copyToClipboard() {
        const password = this.passwordField.value;
        if (!password || password.includes('Выберите')) {
            this.showNotification('❌ Нет пароля для копирования', '#f44336');
            return;
        }
        
        navigator.clipboard.writeText(password).then(() => {
            this.showNotification('✅ Пароль скопирован!', '#4caf50');
            this.animateCopyButton();
        }).catch(() => {
            this.showNotification('❌ Ошибка копирования', '#f44336');
        });
    }
    
    showNotification(message, color) {
        const notification = document.createElement('div');
        notification.textContent = message;
        notification.style.cssText = `
            position: fixed;
            bottom: 20px;
            left: 50%;
            transform: translateX(-50%);
            background: ${color};
            color: white;
            padding: 12px 24px;
            border-radius: 40px;
            font-size: 14px;
            font-weight: 600;
            z-index: 1000;
            box-shadow: 0 4px 12px rgba(0,0,0,0.2);
            animation: fadeInOut 2s ease;
        `;
        
        document.body.appendChild(notification);
        setTimeout(() => notification.remove(), 2000);
    }
    
    animateCopyButton() {
        this.copyBtn.style.transform = 'scale(0.95)';
        setTimeout(() => {
            this.copyBtn.style.transform = '';
        }, 150);
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new PasswordGenerator();
});

const style = document.createElement('style');
style.textContent = `
    @keyframes fadeInOut {
        0% { opacity: 0; transform: translateX(-50%) translateY(20px); }
        15% { opacity: 1; transform: translateX(-50%) translateY(0); }
        85% { opacity: 1; transform: translateX(-50%) translateY(0); }
        100% { opacity: 0; transform: translateX(-50%) translateY(-20px); }
    }
`;
document.head.appendChild(style);
