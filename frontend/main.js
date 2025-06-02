document.querySelectorAll('a[href^="#"]').forEach(anchor => {
  anchor.addEventListener('click', function (e) {
    e.preventDefault();
    document.querySelector(this.getAttribute('href')).scrollIntoView({
      behavior: 'smooth'
    });
  });
});

const sections = document.querySelectorAll('section');
const navLinks = document.querySelectorAll('header nav a');

window.onscroll = () => {
  let current = '';
  sections.forEach(section => {
    const sectionTop = section.offsetTop;
    if (pageYOffset >= sectionTop - 60) {
      current = section.getAttribute('id');
    }
  });

  navLinks.forEach(link => {
    link.classList.remove('active', 'text-sky-600', 'font-semibold');
    if (link.getAttribute('href').includes(current)) {
      link.classList.add('active', 'text-sky-600', 'font-semibold');
    } else {
      link.classList.add('text-slate-700');
    }
  });
};

function toggleDetails(elementId) {
  const detailsElement = document.getElementById(elementId);
  const isOpening = !(detailsElement.style.maxHeight && detailsElement.style.maxHeight !== '0px');

  if (isOpening) {
    detailsElement.style.maxHeight = detailsElement.scrollHeight + "px";
  } else {
    detailsElement.style.maxHeight = '0px';
  }
}

const ctx = document.getElementById('alertasChart').getContext('2d');
const alertasChart = new Chart(ctx, {
  type: 'bar',
  data: {
    labels: ['Enchentes', 'Deslizamentos', 'Vendavais', 'Secas', 'Incêndios'],
    datasets: [{
      label: 'Número de Alertas (Exemplo Anual)',
      data: [120, 75, 90, 40, 60],
      backgroundColor: [
        'rgba(59, 130, 246, 0.7)',
        'rgba(16, 185, 129, 0.7)',
        'rgba(234, 179, 8, 0.7)',
        'rgba(249, 115, 22, 0.7)',
        'rgba(239, 68, 68, 0.7)'
      ],
      borderColor: [
        'rgba(59, 130, 246, 1)',
        'rgba(16, 185, 129, 1)',
        'rgba(234, 179, 8, 1)',
        'rgba(249, 115, 22, 1)',
        'rgba(239, 68, 68, 1)'
      ],
      borderWidth: 1
    }]
  },
  options: {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      y: { beginAtZero: true, title: { display: true, text: 'Quantidade de Alertas' } },
      x: { title: { display: true, text: 'Tipos de Eventos Climáticos' } }
    },
    plugins: {
      legend: { display: true, position: 'top' },
      tooltip: { callbacks: { label: function (context) { return context.dataset.label + ': ' + context.parsed.y + ' alertas'; } } },
      title: { display: true, text: 'Tipos de Alertas Climáticos Comuns (Dados Fictícios)', padding: { top: 10, bottom: 20 }, font: { size: 16 } }
    }
  }
});

function updateAdminStats() {
  document.getElementById('alertasAtivos').textContent = Math.floor(Math.random() * 10) + 1;
  document.getElementById('pedidosAjuda').textContent = Math.floor(Math.random() * 30) + 5;
  document.getElementById('voluntariosDisponiveis').textContent = Math.floor(Math.random() * 50) + 10;
}
setInterval(updateAdminStats, 3000);
updateAdminStats();
