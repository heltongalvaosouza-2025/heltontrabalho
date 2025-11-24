// =============================
// Tema claro/escuro
// =============================
// Comentário: preserva a escolha do usuário usando localStorage
(function initTheme() {
  const saved = localStorage.getItem('theme');
  if (saved === 'dark') {
    document.documentElement.classList.add('dark');
  }
})();

document.addEventListener('click', (ev) => {
  const target = ev.target;

  // Alterna menu mobile (hamburguer)
  if (target.closest('.nav-toggle')) {
    const btn = target.closest('.nav-toggle');
    const nav = document.querySelector('.nav');
    const expanded = btn.getAttribute('aria-expanded') === 'true';
    btn.setAttribute('aria-expanded', (!expanded).toString());
    nav.classList.toggle('open');
    return;
  }

  // Fecha menu ao clicar em link
  if (target.tagName === 'A' && target.closest('.nav__list')) {
    const nav = document.querySelector('.nav');
    const btn = document.querySelector('.nav-toggle');
    if (nav && nav.classList.contains('open')) {
      nav.classList.remove('open');
      if (btn) btn.setAttribute('aria-expanded', 'false');
    }
  }

  // Alterna tema
  if (target.id === 'themeToggle') {
    document.documentElement.classList.toggle('dark');
    const isDark = document.documentElement.classList.contains('dark');
    localStorage.setItem('theme', isDark ? 'dark' : 'light');
    // Atualiza ícone visual
    target.textContent = isDark ? '🌞' : '🌙';
  }

  // Fecha modal
  if (target.hasAttribute('data-close-modal')) {
    closeModal();
  }
});

// Ajusta ícone do tema ao carregar
window.addEventListener('DOMContentLoaded', () => {
  const toggle = document.getElementById('themeToggle');
  if (toggle) {
    const isDark = document.documentElement.classList.contains('dark');
    toggle.textContent = isDark ? '🌞' : '🌙';
  }

  // Inicializa validação do formulário se existir na página
  const form = document.getElementById('contactForm');
  if (form) {
    initContactForm(form);
  }
});

// =============================
// Modal utilitário
// =============================
function openModal(message, title = 'Mensagem') {
  const modal = document.getElementById('modal');
  const msg = document.getElementById('modal-message');
  const t = document.getElementById('modal-title');
  if (!modal || !msg || !t) return;
  t.textContent = title;
  msg.textContent = message;
  modal.classList.add('open');
  modal.setAttribute('aria-hidden', 'false');
}

function closeModal() {
  const modal = document.getElementById('modal');
  if (!modal) return;
  modal.classList.remove('open');
  modal.setAttribute('aria-hidden', 'true');
}

// =============================
// Validação do formulário de contato
// =============================
// Regras: todos os campos obrigatórios e e-mail válido
function initContactForm(form) {
  const nome = form.querySelector('#nome');
  const email = form.querySelector('#email');
  const mensagem = form.querySelector('#mensagem');

  const erroNome = form.querySelector('#erro-nome');
  const erroEmail = form.querySelector('#erro-email');
  const erroMensagem = form.querySelector('#erro-mensagem');

  form.addEventListener('submit', (ev) => {
    ev.preventDefault(); // impede envio real
    let ok = true;

    // Limpa erros prévios
    [erroNome, erroEmail, erroMensagem].forEach(e => e.textContent = '');

    // Nome
    if (!nome.value.trim()) {
      erroNome.textContent = 'Por favor, informe seu nome.';
      ok = false;
    }

    // E-mail com regex simples
    const emailVal = email.value.trim();
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailVal) {
      erroEmail.textContent = 'Por favor, informe seu e-mail.';
      ok = false;
    } else if (!emailRegex.test(emailVal)) {
      erroEmail.textContent = 'Formato de e-mail inválido. Ex: usuario@dominio.com';
      ok = false;
    }

    // Mensagem
    if (!mensagem.value.trim()) {
      erroMensagem.textContent = 'Por favor, escreva sua mensagem.';
      ok = false;
    }

    if (!ok) {
      // Feedback de erro
      openModal('Há campos inválidos no formulário. Verifique os avisos em vermelho.', 'Erro de validação');
      return;
    }

    // Simulação de envio: limpar campos e mostrar confirmação
    nome.value = '';
    email.value = '';
    mensagem.value = '';
    openModal('Mensagem enviada com sucesso!', 'Sucesso');
  });

  // Feedback ao digitar (opcional)
  [nome, email, mensagem].forEach((input) => {
    input.addEventListener('input', () => {
      const id = input.id;
      const erro = form.querySelector('#erro-' + id);
      if (erro && input.value.trim()) {
        erro.textContent = '';
      }
    });
  });
}